module MoneyMaker.CanopyScraper.CanopyNavigation

open canopy
open canopy.classic
open MoneyMaker.Common
open OptionBuilder

let initialize() = start classic.chrome

let navigateToPage link = 
    url link

let loginToOddsPortalWithData username password =
    url (ScrapingParts.prependBaseWebsite "/login/")
    "#login-username1" << username
    "#login-password1" << password
    click (last (text "Login"))

let getPageHtml link =
    url link
    js "return document.documentElement.outerHTML" |> string

let invokeRepeatedIfFailed actionToRepeat =
    let rec repeatedAction timesTried actionToRepeat =
        if timesTried < 2 then
            use timer = new System.Timers.Timer()
            timer.Interval <- 5000.
            try
                timer.Elapsed.Add(fun _ -> failsWith "")
                timer.Start()
                let result = actionToRepeat()
                timer.Stop()

                Some result
            with
            | _ ->
                timer.Stop()
                repeatedAction (timesTried + 1) actionToRepeat
        else 
            None

    repeatedAction 0 actionToRepeat

let navigateAndReadGameHtml link =
    option {
        let! gameHtml = invokeRepeatedIfFailed (fun () -> getPageHtml link)
        return gameHtml |> GamePageReading.parseGameHtml
    }

let getCurrentUrl = currentUrl