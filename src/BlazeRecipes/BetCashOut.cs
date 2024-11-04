using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public class BetCashOut
    {
        [JsonProperty("room_id")]
        public int RoomId {get; set;}
        [JsonProperty("wallet_id")]
        public long WalletId {get; set;}

        internal string GetCorrectJson()
        {
            var json = JsonConvert.SerializeObject(this);

            return json.Replace("RoomId", "room_id").Replace("WalletId", "wallet_id");
        }
    }
}