namespace BlazeHelper.BlazeRecipes
{
    public class Wallet
    {
        public Currency Currency {get; set;} = new();
        public DepositCurrency DepositCurrency {get; set;} = new();
        public int Id {get; set;}
        public bool Primary {get; set;}
        public string Balance {get; set;} = "0.0";
        public string BonusBalance {get; set;} = "0.0";
        public string RealBalance {get; set;} = "0.000";
        public string CurrencyType {get; set;} = "BRL";
    }
}