namespace MoneyMaker.Dto

[<CLIMutable>]
type LeagueDto = { Country: string; League: string }

[<CLIMutable>]
type SportDto = { Sport: string; Leagues: LeagueDto array }

[<CLIMutable>]
type GameOddsDto = { Home: float; Draw: float; Away: float }

[<CLIMutable>]
type GameDto = 
    { 
        HomeTeam: string; AwayTeam: string; 
        Date: System.DateTime; 
        GameLink: string 
        Sport: string; 
        Country: string; 
        League: string 
        MeanOdds: GameOddsDto
        Odds: GameOddsDto
        NoMean: int
    }