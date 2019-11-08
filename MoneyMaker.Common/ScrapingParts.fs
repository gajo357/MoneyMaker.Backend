module MoneyMaker.Common.ScrapingParts

open Common

let BaseWebsite = "http://www.oddsportal.com"
let BaseSafeWebsite = "https://www.oddsportal.com"

let prependBaseWebsite href = 
    if (href |> contains BaseWebsite || href |> contains BaseSafeWebsite) then href
    else sprintf "%s%s" BaseWebsite href

let getLinkParts = split "/"

let extractSportCountryAndLeagueFromLink gameLink =
    let parts = getLinkParts (gameLink |> remove BaseWebsite |> remove BaseSafeWebsite)
    (parts.[0], parts.[1], parts.[2])

let isGameLinkFromAnyLeague (sportInfo: Sport) gameLink =
    if ((gameLink |> (remove BaseWebsite) |> getLinkParts |> Array.length) < 3) then false
    else
        let (sport, country, league) = extractSportCountryAndLeagueFromLink gameLink
        if sportInfo.Sport <> sport then false
        else if (sportInfo.Leagues |> Seq.exists (fun s -> s.League = league && s.Country = country)) then true
        else false
