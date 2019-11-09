module MoneyMaker.CanopyScraper.Models

type OddsRow = {
    Bookie: string
    Odds: float list
    Deactivated: bool
}