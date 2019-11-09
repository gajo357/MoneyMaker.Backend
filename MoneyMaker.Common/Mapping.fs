module MoneyMaker.Common.Mapping

let toGameOddsDto (odds: GameOdds): MoneyMaker.Dto.GameOddsDto =
    {
        Home = odds.Home
        Draw = odds.Draw
        Away = odds.Away
    }

let toGameDto (game: Game): MoneyMaker.Dto.GameDto =
    {
        HomeTeam = game.HomeTeam; AwayTeam = game.AwayTeam
        Date = game.Date
        GameLink = game.GameLink;
        Sport = game.Sport; Country = game.Country; League = game.League
        Odds = toGameOddsDto game.Odds
        MeanOdds = toGameOddsDto game.MeanOdds
        NoMean = game.NoMean
    }
    

