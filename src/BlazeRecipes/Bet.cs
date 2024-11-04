using System.Text.Json;
using BlazeHelper.Config;
using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public  class Bet
    {
        [JsonProperty("amount")]
        public string Amount { get; set; } = "0.00";
        [JsonProperty("auto_cashout_at")]
        public string? AutoCashOutAt {get; set;} 
        [JsonProperty("rank")]
        public string Rank {get; set;} = string.Empty;
        [JsonProperty("room_id")]
        public int RoomId {get; set;} = 4;
        [JsonProperty("type")]
        public string Type {get; set;} = "BRL";
        [JsonProperty("username")]
        public string Username {get; set;} = string.Empty;
        [JsonProperty("wallet_id")]
        public int WalletId {get; set;}

        internal string GetCorrectJson()
        {
            return JsonConvert.SerializeObject(this);

            //return json.Replace("\"Amount\":", "\"amount\":").Replace("\"AutoCashOutAt\":", "\"auto_cashout_at\":").Replace("\"Rank\":", "\"rank\":").Replace("\"RoomId\":", "\"room_id\":").Replace("\"Type\":","\"type\":").Replace("\"Username\":", "\"username\":").Replace("\"WalletId\":", "\"wallet_id\":");
        }

        internal Bet(){}
        internal Bet(User user, Wallet wallet, double betAmout)
        {
            if(user.Info == null)
            {
                throw new NullReferenceException("A propriedade 'Info' é nula.");
            }
            if(user.Experience == null)
            {
                throw new NullReferenceException("A propriedade 'Experience' é nula.");
            }

            Username = user.Info.Username;
            Rank = user.Experience.Rank;
            WalletId = wallet.Id;
            Amount = betAmout.ToString("0.00").Replace(',', '.');
        }
    }
}