module MoneyMaker.Common.Common
open System.Text.RegularExpressions
open System

let integerInString input =
    let m = Regex.Match(input, "\\d+") 
    if (m.Success) then Some (m.Groups.[0].Value |> int) else None
    
let remove oldValue (input:string) = 
    input.Replace(oldValue, System.String.Empty)

let removeSeq input valuesToRemove =
    valuesToRemove |> Seq.fold (fun acc year -> remove year acc) input
        
let split (separator:string) (input:string) = 
    input.Split([|separator|], System.StringSplitOptions.RemoveEmptyEntries)
        
let join separator (input:string[]) = 
    System.String.Join(separator, input)
    
let joinList separator (array: Object list) = 
    String.Join(separator, array)

let joinCsv = join ","

let contains value (input:string) = input.Contains(value)
    
let notEmpty input = System.String.IsNullOrEmpty(input) |> not

let startsWith value (input:string) = input.StartsWith(value)

let tryParseWith tryParseFunc =
    tryParseFunc >> function
    | true, value -> Some value
    | false, _ -> None
    
let tryParseDateTime = tryParseWith System.DateTime.TryParse
let tryParseDouble = tryParseWith System.Double.TryParse

let convertOptionToNullable = function
    | None -> new System.Nullable<_>()
    | Some value -> new System.Nullable<_>(value)

let isNonEmptyString input =
    System.String.IsNullOrEmpty(input) = false

let tryAndForget action =
    try
        action()
    with
    | _ -> ignore

let invokeRepeatedIfFailed actionToRepeat =
    let rec repeatedAction timesTried actionToRepeat =
        if timesTried < 2 then
            use timer = new System.Timers.Timer()
            timer.Interval <- 5000.
            try
                timer.Elapsed.Add(fun _ -> raise(Exception("")))
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
