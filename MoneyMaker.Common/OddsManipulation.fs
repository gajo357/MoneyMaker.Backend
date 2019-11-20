module MoneyMaker.Common.OddsManipulation

open BettingCalculations

let normalizeGameOdds odds =
    let (h, d, o) = normalizeOdds odds.Home odds.Draw odds.Away
    { odds with 
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
    { Home = h; Draw = d; Away = a}

let emptyOdds = { Home = 0.; Draw = 0.; Away = 0. }
let emptyGame = {
    HomeTeam = ""; AwayTeam = ""; Date = System.DateTime.MinValue;
    GameLink = ""; Sport = ""; Country = ""; League = "";
    Odds = emptyOdds; Bookie = "" 
}

let oddsFromGame g = g.Odds
