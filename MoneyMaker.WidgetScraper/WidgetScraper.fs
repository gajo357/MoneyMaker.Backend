module MoneyMaker.WidgetScraper

open FSharp.Data
open MoneyMaker.Common
open NodeOperations
open TableOperations

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

let widgetGamesAsync (bookie: MoneyMaker.Dto.BookieWidgetDto) = async {
        let link = createWidgetLink bookie.Widget
        let! table = widgetTableAsync link
        return table |> getGameInfosFromTable |> Seq.map (fun g -> {g with Bookie = bookie.Name})
    }

let getWidgetGames bookies = async {
    let! meanGames = 
        bookies 
        |> Seq.map widgetGamesAsync 
        |> Async.Parallel

    let meanGames = meanGames |> Seq.collect id

    return 
        meanGames
        |> Seq.groupBy (fun g -> g.GameLink)
        |> Seq.map (snd >> Mapping.toGameDtoFromRows)
        |> Seq.toArray
}