module MoneyMaker.WidgetScraper.NodeOperations

open FSharp.Data


let getText (node:HtmlNode) =
    node.InnerText().Trim()

let getElements (name: string) (node:HtmlNode) =
    node.Descendants(name)
    
let getAttribute attribute (node:HtmlNode) = 
    node.AttributeValue(attribute)

let getFirstElement name node =
    getElements name node
    |> Seq.head

let getTableRows = getElements "tr"

let getTdsFromRow = getElements "td"

let getHref = getAttribute "href"
let getAllHrefElements = getElements "a"
