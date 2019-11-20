module MoneyMaker.CanopyScraper.GamePageReading

open FSharp.Data
open MoneyMaker.Common
open ScrapingParts
open TableOperations
open NodeOperations
open OptionBuilder
open BettingCalculations
open OddsManipulation
open Common
open Models

type GameProvider = HtmlProvider<"http://www.oddsportal.com/soccer/england/premier-league-2016-2017/arsenal-everton-SGPa5fvr/">

let parseGameHtml gameHtml =
    (GameProvider.Parse gameHtml).Html

let getGameInfosFromTable = getTableRows >> Seq.choose getGameInfoFromRow

let gamesFromGameTable sportInfo gamesTable = 
    gamesTable
    |> getGameInfosFromTable
    |> Seq.filter (fun g -> isGameLinkFromAnyLeague sportInfo g.GameLink)

let gamesFromGameTablePage tableId sportInfo gamesHtml = 
    gamesHtml
    |> getElementById tableId
    |> gamesFromGameTable sportInfo

let gamesFromRegularPage sportInfo gamesHtml = 
    gamesFromGameTablePage "#table-matches" sportInfo gamesHtml

let getParticipantsAndDateElement = getElementById "#col-content"

let readParticipantsNames = 
    (getFirstElement "h1") >> getText  >> (split " - ")>> fun parts -> (parts.[0], parts.[1])

let readGameDate = 
    (getFirstElement "p") >> getText >> (split ",") >> (fun n -> n.[1..2]) >> (join ", ") >> tryParseDateTime

let getDateOrDefault gameDate =
    match gameDate with
    | Some d -> d
    | None -> System.DateTime.MinValue

let convertStringToOdd input =
    match tryParseDouble input with
    | Some v -> v
    | None -> 0.

let getOddsFromRow node =
    let tds = getTdsFromRow node |> Seq.toArray
    match tds with
    | [||] -> None
    | _ ->
        let name = tds |> Seq.head |> getText |> remove "\n"
        let oddTds = 
            tds 
            |> Seq.filter (classAttributeContains "right odds")
            |> Seq.toArray

        match oddTds with
        | [||] -> None
        | _ ->
            let odds = 
                oddTds
                |> Seq.map getText
                |> Seq.map convertStringToOdd
                |> Seq.toList

            let deactivated = 
                oddTds
                |> Seq.exists (classAttributeContains "dark")

            Some { Bookie = name; Odds = odds; Deactivated = deactivated}

let getOddsFromGamePage gameHtml = 
    match gameHtml |> (getElementById "#odds-data-table") |> (getElements "tbody") |> Seq.tryHead with
    | None -> [||]
    | Some head -> 
        head
        |> getTableRows
        |> Seq.filter (classAttributeContains "lo")
        |> Seq.choose getOddsFromRow
        |> Seq.toArray

let readGameRows bookies gameLink gameHtml =
    let odds = gameHtml |> getOddsFromGamePage
        
    let participantsAndDateElement = getParticipantsAndDateElement gameHtml
    let (homeTeam, awayTeam) = readParticipantsNames participantsAndDateElement
    let gameDate = participantsAndDateElement |> (readGameDate >> getDateOrDefault)
    let (sport, country, league) = extractSportCountryAndLeagueFromLink gameLink

    odds 
    |> Seq.filter (fun o -> not o.Deactivated)
    |> Seq.filter (fun o -> bookies |> Seq.contains o.Bookie)
    |> Seq.map (fun o -> 
        {
            HomeTeam = homeTeam; AwayTeam = awayTeam
            Date = gameDate; GameLink = gameLink
            Sport = sport; Country = country; League = league
            Odds = convertOddsListTo1x2 o.Odds; Bookie = o.Bookie
        }
    )
