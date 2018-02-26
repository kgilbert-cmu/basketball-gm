namespace MonteCarlo

module MonteOutput = 
    open System
    open MathNet.Numerics.Distributions
    open System.IO
    open MonteModel

    type ParameterRange = 
        Band of double * double

    type QGMSamplingParameters =
        {
            Prop3Pct : ParameterRange;
            ThreePTPct : ParameterRange;
            TwoPTPct : ParameterRange;
            FTPct : ParameterRange;
            FTPer2FGA : ParameterRange;
            BPct : ParameterRange;
            SPct : ParameterRange;
            ORBRate : ParameterRange;
            DRBRate : ParameterRange; 
        }
    
    type SampleDistributions = 
        {
            Prop3Dist : ContinuousUniform;
            ThreePCTDist : ContinuousUniform;
            TwoPCTDist : ContinuousUniform;
            FTPCTDist : ContinuousUniform;
            FTPer2Dist : ContinuousUniform;
            BPctDist : ContinuousUniform;
            SPctDist : ContinuousUniform;
            ORBRateDist : ContinuousUniform;
            DRBRateDist : ContinuousUniform;
        }

    let writeCSVHeader (writer : System.IO.TextWriter) = 
        writer.WriteLine("3PA/FGA,3P%,2P%,FT%,FT/2PA,B%,S%,ORBR,DRBR,Wins")

    let writeSampleResult (writer : System.IO.TextWriter) (player : Player) (score : double) = 
        match player with
        | { Prop3 = prop3; TwoPTPct = twoPCT; ThreePTPct = threePCT; TwoFTRate = ftPer2; FTPct = ftPCT; BPct = bPCT; SPct = sPCT; ORBRate = orbR; DRBRate = drbR; } ->
            fprintfn writer "%.3f,%.3f,%.3f,%.3f,%.3f,%.3f,%.3f,%.3f,%.3f,%.1f" prop3 threePCT twoPCT ftPCT ftPer2 bPCT sPCT orbR drbR score

    let ctFromBand (pr : ParameterRange) = 
        match pr with
        | Band (prMin, prMax) -> new ContinuousUniform(prMin, prMax)

    let createDistributionsFromParameters (sampleParams : QGMSamplingParameters) = 
        match sampleParams with
        | {Prop3Pct = prop3PCT; ThreePTPct = threePCT; TwoPTPct = twoPCT; FTPct = ftPCT; FTPer2FGA = ftPer2; BPct = bPCT; SPct = sPCT; ORBRate = orbRate; DRBRate = drbRate; } ->
            {
                Prop3Dist = ctFromBand prop3PCT;
                ThreePCTDist = ctFromBand threePCT;
                TwoPCTDist = ctFromBand twoPCT;
                FTPCTDist = ctFromBand ftPCT;
                FTPer2Dist = ctFromBand ftPer2;
                BPctDist = ctFromBand bPCT;
                SPctDist = ctFromBand sPCT;
                ORBRateDist = ctFromBand orbRate;
                DRBRateDist = ctFromBand drbRate;
            }

    let createPlayerSampleFromDistributions (sampleDist : SampleDistributions) = 
        match sampleDist with
        | { Prop3Dist = prop3Dist; ThreePCTDist = threeDist; TwoPCTDist = twoDist; FTPCTDist = ftDist;
            FTPer2Dist = ftPerDist; BPctDist = bDist; SPctDist = sDist; ORBRateDist = orbDist; DRBRateDist = drbDist; } ->
            {
                Player.Prop3 = prop3Dist.Sample();
                Player.ThreePTPct = threeDist.Sample();
                Player.TwoPTPct = twoDist.Sample();
                Player.FTPct = ftDist.Sample();
                Player.TwoFTRate = ftPerDist.Sample();
                Player.BPct = bDist.Sample();
                Player.SPct = sDist.Sample();
                Player.ORBRate = orbDist.Sample();
                Player.DRBRate = drbDist.Sample();
            }

    let runMonte (sampleParams : QGMSamplingParameters) (samples : int) (rounds : int) = 
        let dists = createDistributionsFromParameters sampleParams

        let players = 
            Seq.init samples (fun _ -> createPlayerSampleFromDistributions dists)

        let writer = System.IO.File.CreateText("results.csv")
        
        players
        |> Seq.iter 
            (fun p1 -> 
                players
                |> Seq.sumBy (fun p2 -> runModel p1 p2)
                |> writeSampleResult writer p1)

        do writer.Dispose()