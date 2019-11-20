module MoneyMaker.Workflows

open MoneyMaker
open MoneyMaker.Dto
open MoneyMaker.Common.OptionBuilder
open MoneyMaker.Common.BettingCalculations

let DownloadFromWidgetAsync (leagues: LeagueGaussDto seq) bookie bookies =
    async {
        let! games = WidgetScraper.getWidgetGames bookies
        
        return games
        |> Array.map (fun g -> option {
            let! league = leagues |> Seq.tryFind (fun l -> l.Sport = g.Info.Sport && l.Country = g.Info.Country && l.League = g.Info.League)
            let! bookieOdds = g.Odds |> Seq.tryFind (fun o -> o.Bookie = bookie)
            let! meanOdds = g.Odds |> Array.map normalize |> calculateMeans
            
            let meanOdds = meanOdds |> calculatePsychs |> normalize
            let kellies = calculateKellies league meanOdds bookieOdds

            return {
                Info = g.Info

                MyOdds = meanOdds
                BookieOdds = bookieOdds
                Kellies = kellies
            }
        })
        |> Array.choose id

    } |> Async.StartAsTask

let WidgetsActivityProbeAsync bookies =
    async {
        let! results = 
            bookies
            |> Seq.map (fun bookie -> async {
                let! games = WidgetScraper.getWidgetGames [bookie]
                return games |> Array.isEmpty, bookie.Name
            })
            |> Async.Parallel
        
        return results |> Seq.filter fst |> Seq.map snd
    } |> Async.StartAsTask