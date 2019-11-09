module MoneyMaker.Common.NodeOperations

open FSharp.Data
open Common

let getElementById (name: string) (node:HtmlDocument) =
    node.CssSelect(name).Head

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

let getClassAttribute node = getAttribute "class" node

let classAttributeContains text node =
    node|> getClassAttribute |> contains text