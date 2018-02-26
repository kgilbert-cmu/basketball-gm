// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open MonteCarlo
open MonteOutput

[<EntryPoint>]
let main argv = 
    let qparams = 
        {
            Prop3Pct = Band(0.0, 0.4);
            ThreePTPct = Band(0.0, 0.08);
            TwoPTPct = Band(0.0, 0.2);
            FTPct = Band(0.15, 0.6);
            FTPer2FGA = Band(0.0, 0.6);
            BPct = Band(0.0, 0.25);
            SPct = Band(0.0, 0.30);
            ORBRate = Band(0.0, 0.22);
            DRBRate = Band(0.0, 0.65);
        }

    MonteOutput.runMonte qparams 1000 1

    0 // return an integer exit code
