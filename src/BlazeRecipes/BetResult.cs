using System.Text;
using BlazeHelper.Config;
using BlazeHelper.Exceptions;
using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public class BetResult : Bet
    {
        private BetResult(User user, Wallet wallet, double betAmout) : base(user, wallet, betAmout){}
        public BetResult(){}

        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("cashed_out_at")]
        public string? CashedOutAt { get; set; }
        [JsonProperty("currency_type")]
        public string CurrencyType { get; set; } = string.Empty;
        [JsonProperty("is_bonus_round")]
        public bool? IsBonusRound { get; set; }
        [JsonProperty("win_amount")]
        public string? WinAmount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
        [JsonProperty("user")]
        public BetUser User { get; set;} = new();
        internal new long WalletId
        {
            get => throw new NotImplementedException("Propriedade não disponível nesta classe.");
            set => throw new NotImplementedException("Propriedade não disponível nesta classe.");
        }

        // https://blaze.com/api/singleplayer-originals/originals/crash_v2/round/cashout
        internal async Task<BetResult?> CashOut(Tokens tokens, long walletId)
        {
            if(Status != "Created")
            {
                throw new NotSupportedBetStatusException("Apenas apostas com o status 'Created' podem usar este método. this: " + JsonConvert.SerializeObject(this) );
            }

            try
            {
                var json = new BetCashOut()
                {
                    RoomId = this.RoomId,
                    WalletId = walletId
                }.GetCorrectJson();

                Program.Print(json);

                var client  = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://blaze.com/api/singleplayer-originals/originals/crash_v2/round/cashout")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {tokens.Refresh_Token}");
                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                Program.Print(responseBody);
                return JsonConvert.DeserializeObject<BetResult>(responseBody);
            }
            catch(Exception exception)
            {
                Program.Print($"{exception.Message} | {exception.StackTrace}");
            }
  
            return null;
        }
    }
}