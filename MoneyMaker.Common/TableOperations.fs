module MoneyMaker.Common.TableOperations

open NodeOperations 
open OptionBuilder
open Common
open ScrapingParts
open OddsManipulation

let rowOdds tds =
    tds 
    |> Array.map getText 
    |> Array.choose tryParseDouble
    |> List.ofArray
    |> convertOddsListTo1x2

let getGameInfoFromRow row =
    option {
        let tds = row |> getTdsFromRow |> Seq.toArray

        let! timeTd = tds |> Seq.tryHead
        let! time = timeTd |> getText |> tryParseDateTime

        let! hrefElem = 
            tds 
            |> Seq.collect getAllHrefElements
            |> Seq.tryFind (fun a -> a |> getText |> (contains "-"))
        let link = hrefElem |> getHref
        let (homeTeam, awayTeam) = hrefElem |> getText |> ((split "-") >> (fun p -> (p.[0], p.[1])))
        let (sport, country, league) = extractSportCountryAndLeagueFromLink link
        let odds = rowOdds tds

        return { 
                    Date = time; 
                    HomeTeam = homeTeam; AwayTeam = awayTeam;
                    GameLink =  prependBaseWebsite link;
                    Sport = sport; Country = country; League = league;
                    Odds = odds; Bookie = ""
               }
    }

let getGameInfosFromTable = getTableRows >> Seq.choose getGameInfoFromRow
