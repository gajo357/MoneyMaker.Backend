module MoneyMaker.Common.BettingCalculations

open MoneyMaker.Common.OptionBuilder
open MoneyMaker.Dto
    
let round (n: float) = System.Math.Round(n, 2)
let pow = System.Math.Pow

let mean (values: float seq) = 
    if values |> Seq.isEmpty then None
    else values |> Seq.average |> Some
let meanFromFunc propFunc = (Seq.map propFunc) >> mean

let kelly myOdd bookerOdd = 
    if (myOdd = 0.) then 0.
    else if (bookerOdd = 1.) then 0.
    else (bookerOdd/myOdd - 1.) / (bookerOdd - 1.)

let moneyToBet kelly amount =
    let m = kelly * amount
    if m < 2.0 then 2.0
    else m
    
let invert v = 
    if v = 0. then 0.
    else 1. / v

let normalizePct h d a =
    let whole = h + d + a
    (h/whole, d/whole, a/whole)
let normalizeOdds h d a =
    let (h, d, a) = normalizePct (invert h) (invert d) (invert a)
    ((invert h), (invert d), (invert a))

let gaus mu sigma x =
    let variance = sigma ** 2.
    1. / sqrt (2. * System.Math.PI * variance) * exp (-((x - mu) ** 2.) / (2. * variance))

let muStdDev data = option {
    let! mu = mean data
    let variance = data |> List.averageBy (fun x -> (x - mu) ** 2.)

    return mu, sqrt (variance)
}

let psychFunc v = 1.3794 * pow(v, 4.) - 0.1194 * pow(v, 3.) - 1.959 * pow(v, 2.) + 1.6147 * v + 0.0344
    
let psychOdds = invert >> psychFunc >> invert

let getAmountToBet amount myOdd bookerOdd =
    let myOdd = myOdd |> invert |> psychFunc |> invert
    if myOdd <= 3.05 || myOdd >= 3.15 then 0.
    else
        let k = kelly myOdd bookerOdd
        if k > 0. then moneyToBet k amount
        else 0.

let isInRange (league: LeagueGaussDto) myOdd =
    let wg = league.WinCountMu * gaus league.WinMu league.WinSigma myOdd
    let lg = league.LoseCountMu * gaus league.LoseMu league.LoseSigma myOdd

    (wg > lg * 2.) && (league.WinMu - league.WinSigma <= myOdd) && (league.WinMu + league.WinSigma >= myOdd)

let kellyForBet league myOdd bookerOdd =
    match isInRange league myOdd with
    | false -> 0.
    | true -> 
        let k = kelly myOdd bookerOdd
        if k > 0. then k
        else 0.

let calculateMeans (odds: GameOddsDto seq) = option {
    let! h = odds |> meanFromFunc (fun o -> o.Home)
    let! d = odds |> meanFromFunc (fun o -> o.Draw)
    let! a = odds |> meanFromFunc (fun o -> o.Away)
    
    return { Home = h; Draw = d; Away = a; Bookie = ""}
}

let calculatePsychs meanOdds = 
    let h = psychOdds meanOdds.Home
    let d = psychOdds meanOdds.Draw
    let a = psychOdds meanOdds.Away

    { Home = h; Draw = d; Away = a; Bookie = meanOdds.Bookie}
    
let normalize meanOdds = 
    let (h, d, a) = normalizeOdds meanOdds.Home meanOdds.Draw meanOdds.Away
    
    { Home = h; Draw = d; Away = a; Bookie = meanOdds.Bookie}

let calculateKellies league meanOdds bookieOdd = 
    {
        Home = kellyForBet league meanOdds.Home bookieOdd.Home
        Draw = kellyForBet league meanOdds.Draw bookieOdd.Draw
        Away = kellyForBet league meanOdds.Away bookieOdd.Away
        Bookie = ""
    }
    

    

