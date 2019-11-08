﻿module MoneyMaker.Common.BettingCalculations
    
let round (n: float) = System.Math.Round(n, 2)
let pow = System.Math.Pow

let mean (values: float seq) = 
    if values |> Seq.isEmpty then 1.
    else values |> Seq.average
let meanFromFunc propFunc = (Seq.map propFunc) >> mean

let kelly myOdd bookerOdd = 
    if (myOdd = 0.) then 0.
    else if (bookerOdd = 1.) then 0.
    else (bookerOdd/myOdd - 1.) / (bookerOdd - 1.)

let moneyToBet kelly amount =
    let m = kelly * amount
    if m < 2.0 then 2.0
    else m
    
let invert = (/) 1.

let normalizePct h d a =
    let whole = h + d + a
    (h/whole, d/whole, a/whole)
let normalizeOdds h d a =
    let (h, d, a) = normalizePct (invert h) (invert d) (invert a)
    ((invert h), (invert d), (invert a))

let psychFunc v = 1.3794 * pow(v, 4.) - 0.1194 * pow(v, 3.) - 1.959 * pow(v, 2.) + 1.6147 * v + 0.0344
    
let psychOdds = invert >> psychFunc >> invert

let getAmountToBet amount myOdd bookerOdd =
    let myOdd = myOdd |> invert |> psychFunc |> invert
    if myOdd <= 3.05 || myOdd >= 3.15 then 0.
    else
        let k = kelly myOdd bookerOdd
        if k > 0. then moneyToBet k amount
        else 0.
    

