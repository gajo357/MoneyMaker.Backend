module MoneyMaker.WidgetScraper.WidgetScraper

open FSharp.Data
open MoneyMaker.Common
open ScrapingParts
open NodeOperations
open TableOperations
open OddsManipulation

type GameProvider = HtmlProvider<"https://widgets.oddsportal.com/e2eb0fe27b471bb/s/">

let widgetHtmlAsync link = async {
        let! page = GameProvider.AsyncLoad(link)
        return page.Html
    }

let widgetTableAsync link = async {
        let! html = widgetHtmlAsync link
        return getFirstElement "table" (html.Body())
    }

let createWidgetLink = (sprintf "https://widgets.oddsportal.com/%s/s/")
let getWidgetId bookie =
    match bookie with
    | "bet365" -> "e2eb0fe27b471bb"
    | "bwin" -> "5f5c1365462bfec"
    | "pinnacle" -> "098c02809003dcd"
    | "williamHill" -> "db03999b5cfdac1"
    | _ -> failwith (sprintf "Bookie \"%s\" is unknown" bookie)

let getWidgetLink = getWidgetId >> createWidgetLink

let widgetGamesAsync bookie = async {
        let link = getWidgetLink bookie
        let! table = widgetTableAsync link
        return getGameInfosFromTable table, bookie
    }

let calculatePsychForGame games =
    games
    |> Seq.groupBy (fun g -> g.GameLink)
    |> Seq.map (fun (_, gs) -> 
        let gs = gs |> Seq.toArray
        let means = 
            gs 
            |> Array.map (oddsFromGame >> normalizeGameOdds) 
            |> calculateMeans
            |> applyPsychNorm
        { (gs |> Array.head) with 
            MeanOdds = means
            NoMean = gs.Length})

let getWidgetGames myBookie meanBookies = async {
    let! meanGames = 
        meanBookies 
        |> Seq.map widgetGamesAsync 
        |> Async.Parallel

    let bet365Games = meanGames |> Array.find (fun (_, b) -> b = myBookie) |> fst |> Seq.toArray

    let meanGames = meanGames |> Seq.collect fst |> calculatePsychForGame

    return 
        meanGames 
        |> Seq.map (fun meanGame -> 
            let bg = bet365Games |> Array.tryFind(fun g -> g.GameLink = meanGame.GameLink)
            match bg with
            | None -> None // can't find bet365 odds for this game
            | Some g -> Some { g with 
                                MeanOdds = meanGame.MeanOdds
                                NoMean = meanGame.NoMean }
        )
        |> Seq.choose id
        |> Seq.toArray
}

let DownloadFromWidgetAsync (sports: MoneyMaker.Dto.SportDto seq) myBookie meanBookies =
    async {
        let! games = getWidgetGames myBookie meanBookies
        return
            sports
            |> Seq.collect (fun s -> 
                games |> Seq.filter (fun g -> isGameLinkFromAnyLeague s g.GameLink))
            |> Seq.map Mapping.toGameDto
            |> Seq.toArray
    } |> Async.StartAsTask