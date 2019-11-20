namespace MoneyMaker.Common

type League = { Country: string; League: string }
type Sport = { Sport: string; Leagues: League list }
type GameOdds = { Home: float; Draw: float; Away: float; }

type GameRow = 
    { 
        HomeTeam: string; AwayTeam: string; 
        Date: System.DateTime; 
        GameLink: string 
        Sport: string; 
        Country: string; 
        League: string 
        Odds: GameOdds
        Bookie: string
    }
