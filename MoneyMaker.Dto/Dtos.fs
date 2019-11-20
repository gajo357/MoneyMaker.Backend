namespace MoneyMaker.Dto

[<CLIMutable>]
type BookieWidgetDto = { Name: string; Widget: string }

[<CLIMutable>]
type LeagueGaussDto = { 
    Sport: string; Country: string; League: string; 
    WinMu: float; WinSigma: float; WinCountMu: float; 
    LoseMu: float; LoseSigma: float; LoseCountMu: float 
    }

[<CLIMutable>]
type LeagueDto = { Country: string; League: string }

[<CLIMutable>]
type SportDto = { Sport: string; Leagues: LeagueDto[] }

[<CLIMutable>]
type GameOddsDto = { Home: float; Draw: float; Away: float; Bookie: string }

[<CLIMutable>]
type GameInfoDto = 
    { 
        HomeTeam: string; AwayTeam: string; 
        Date: System.DateTime; 
        GameLink: string 
        Sport: string; 
        Country: string; 
        League: string 
    }

[<CLIMutable>]
type GameDto = 
    { 
        Info: GameInfoDto

        Odds: GameOddsDto[]
    }

[<CLIMutable>]
type GameWithBetDto = 
    { 
        Info: GameInfoDto

        MyOdds: GameOddsDto
        BookieOdds: GameOddsDto
        Kellies: GameOddsDto
    }