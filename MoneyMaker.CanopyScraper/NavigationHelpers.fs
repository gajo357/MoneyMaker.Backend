module MoneyMaker.CanopyScraper.NavigationHelpers

open CanopyAgent
open GamePageReading
open MoneyMaker.Common
open ScrapingParts

let getDateAsString (date: System.DateTime) = System.String.Format("{0:yyyyMMdd}", date)

let sportDateUrl date sport = 
    "/matches/" + sport + "/" + getDateAsString date + "/"
    |> prependBaseWebsite

let agent = CanopyAgent()
let navigateAndGetHtml link = async {
    let! gameHtmlString = agent.GetPageHtml link
    return parseGameHtml gameHtmlString 
}