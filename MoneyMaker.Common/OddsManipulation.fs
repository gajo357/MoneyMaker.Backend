module MoneyMaker.Common.OddsManipulation

open BettingCalculations

let normalizeGameOdds odds =
    let (h, d, o) = normalizeOdds odds.Home odds.Draw odds.Away
    { 
        Home = h
        Draw = d
        Away = o
    }

let convertOddsListTo1x2 odds = 
    let h, d, a = 
        match odds with
        | [home; away] -> (home, 0.0, away)
        | [home; draw; away] -> (home, draw, away)
        | [home; draw; away; _] -> (home, draw, away)
        | _ -> (0.0, 0.0, 0.0)
    { Home = h; Draw = d; Away = a }

let emptyOdds = { Home = 0.; Draw = 0.; Away = 0.}
let emptyGame = {
    HomeTeam = ""; AwayTeam = ""; Date = System.DateTime.MinValue;
    GameLink = ""; Sport = ""; Country = ""; League = "";
    Odds = emptyOdds
    MeanOdds = emptyOdds
    NoMean = 0
}

let oddsFromGame g = g.Odds

let calculateMeans odds = 
    { Home = odds |> meanFromFunc (fun g-> g.Home);
      Draw = odds |> meanFromFunc (fun g-> g.Draw);
      Away = odds |> meanFromFunc (fun g-> g.Away) }

let applyPsychNorm odds =
    {
        Home = psychOdds odds.Home 
        Draw = psychOdds odds.Draw
        Away = psychOdds odds.Away
    } |> normalizeGameOdds