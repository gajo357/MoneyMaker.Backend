module MoneyMaker.Common.Mapping

let toGameOddsDto bookie (odds: GameOdds): MoneyMaker.Dto.GameOddsDto =
    {
        Home = odds.Home
        Draw = odds.Draw
        Away = odds.Away
        Bookie = bookie
    }

let toGameDto (game: GameRow): MoneyMaker.Dto.GameDto =
    {
        Info = { HomeTeam = game.HomeTeam; AwayTeam = game.AwayTeam;
                    Date = game.Date; GameLink = game.GameLink;
                    Sport = game.Sport; Country = game.Country; League = game.League}

        Odds = [| game.Odds |> toGameOddsDto game.Bookie |]
    }
    
let toGameDtoFromRows (gameRows: GameRow seq): MoneyMaker.Dto.GameDto =
    let game = gameRows |> Seq.head
    { (toGameDto game) with
        Odds = gameRows |> Seq.map ((fun g -> g.Odds) >> toGameOddsDto game.Bookie) |> Seq.toArray
    }
    

