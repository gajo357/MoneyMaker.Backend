module MoneyMaker.CanopyScraper.CanopyScraper

open GamePageReading
open NavigationHelpers
open MoneyMaker.Dto
open MoneyMaker.Common

let downloadGameInfos (sports: SportDto seq) date =
    async {
        let! gamesHtmls = 
            sports
            |> Seq.map (fun s -> async {
                let! document = navigateAndGetHtml(sportDateUrl date s.Sport)
                return (s, document)})
            |> Async.Parallel
        return 
            gamesHtmls
            |> Seq.collect(fun (s, document) -> gamesFromRegularPage s document)
            |> Seq.map Mapping.toGameDto
    } |> Async.StartAsTask

let readGameFromLink bookies gameLink =
    async {
        let! html = navigateAndGetHtml gameLink
        let gameRows =  readGameRows bookies gameLink html
        return gameRows |> Mapping.toGameDtoFromRows
    } |> Async.StartAsTask

let logIn username password = 
    async {
        let! resultLink = agent.Login username password
        printfn "%A" resultLink
        return "https://www.oddsportal.com/settings/" = resultLink
    } |> Async.StartAsTask