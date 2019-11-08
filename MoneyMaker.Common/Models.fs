namespace MoneyMaker.Common

type Bet = Home | Draw | Away
type League = { Country: string; League: string }
type Sport = { Sport: string; Leagues: League list }
type GameOdds = { Home: float; Draw: float; Away: float }

type Game = 
    { 
        HomeTeam: string; AwayTeam: string; 
        Date: System.DateTime; 
        GameLink: string 
        Sport: string; 
        Country: string; 
        League: string 
        MeanOdds: GameOdds
        Odds: GameOdds
        NoMean: int
    }
