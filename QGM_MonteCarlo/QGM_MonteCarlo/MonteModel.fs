namespace MonteCarlo

module MonteModel = 
    open System

    type Team = 
        | Us
        | Them

    type GameStatus = 
        {
            PossessionsLeft : int;
            WinAmount : int;
        }

    type Player =
        {
            Prop3 : float;
            TwoPTPct : float;
            ThreePTPct : float;
            TwoFTRate : float;
            FTPct : float;
            BPct : float;
            SPct : float;
            DRBRate : float;
            ORBRate : float;
        }

    let rand = System.Random()

    let next (poss : Team) = 
        match poss with
        | Us -> Them
        | Them -> Us

    let runModel (usPl : Player) (themPl : Player) = 
        let usAdj = 
            {usPl with DRBRate = usPl.DRBRate / (usPl.DRBRate + themPl.ORBRate); ORBRate = usPl.ORBRate / (usPl.ORBRate + themPl.DRBRate)};

        let themAdj = 
            {themPl with DRBRate = themPl.DRBRate / (themPl.DRBRate + usPl.ORBRate); ORBRate = themPl.ORBRate / (themPl.ORBRate + usPl.DRBRate)};

        let startStatus = {PossessionsLeft = 560; WinAmount = 0; }

        let rec runModelRec (poss : Player) (opp : Player) (status : GameStatus) = 
            let sbs = rand.NextDouble()
            let reb = rand.NextDouble()
            let fga = rand.NextDouble()
            let fta = rand.NextDouble()
            let gft = rand.NextDouble()
            let ch3 = rand.NextDouble()

            match status with
            | { PossessionsLeft = 0; WinAmount = amt } -> 
                if amt > 0 then 1.0
                elif amt = 0 then 0.5
                else 0.0
            | _ ->
                let (netPoss : int), (tov : bool), (points : int) = 
                    if sbs < opp.BPct then 
                        if reb < poss.ORBRate then
                            (-1, false, 0)
                        else
                            (-1, true, 0)
                    elif sbs < opp.BPct + opp.SPct then
                        (-1, true, 0)
                    else
                        if ch3 < poss.Prop3 then
                            if fga < poss.ThreePTPct then
                                (-status.PossessionsLeft, true, 150)
                            else
                                if reb < poss.ORBRate then
                                    (-1, false, 0)
                                else
                                    (-1, true, 0)
                        else
                            if gft < poss.TwoFTRate then
                                if fga < poss.TwoPTPct then
                                    if fta < poss.FTPct then
                                        (-1, true, 15)
                                    else
                                        if reb < poss.ORBRate then
                                            (-1, false, 10)
                                        else
                                            (-1, true, 10)
                                else
                                    if fta < poss.FTPct then
                                        (-1, true, 5)
                                    else
                                        if reb < poss.ORBRate then
                                            (-1, false, 0)
                                        else
                                            (-1, true, 0)
                            else
                                if fga < poss.TwoPTPct then
                                    (-1, true, 10)
                                else
                                    if reb < poss.ORBRate then
                                        (-1, false, 0)
                                    else
                                        (-1, true, 0)
            
                let newStatus =
                    if tov then 
                        {status with PossessionsLeft = status.PossessionsLeft - netPoss; WinAmount = -(status.WinAmount + points)}
                    else
                        {status with PossessionsLeft = status.PossessionsLeft - netPoss; WinAmount = status.WinAmount + points}
                    
                let newPoss = 
                    if tov then opp
                    else poss
                
                let newOpp = 
                    if tov then poss
                    else opp

                runModelRec newPoss newOpp newStatus

        runModelRec usAdj themAdj startStatus
