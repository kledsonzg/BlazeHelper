using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public class GamePayload
    {
        public DateTime? UpdatedAt {get; set;}
        public BetResult[]? Bets {get; set;} = Array.Empty<BetResult>();
        public string Id {get; set;} = string.Empty;
        public string? Status {get; set;} = "waiting";
        [JsonProperty("crash_point")]
        public string? CrashPoint {get; set;}
        [JsonProperty("is_bonus_round")]
        public bool? IsBonusRound {get; set;}
        [JsonProperty("total_eur_bet")]
        public object? TotalEurBet {get; set;} = string.Empty;
        [JsonProperty("total_bets_placed")]
        public string? TotalBetsPlaced {get; set;} = string.Empty;
        [JsonProperty("total_eur_won")]
        public double? TotalEurWon {get; set;}
        public int? RoomId {get; set;}

        internal int StatusCode 
        {   get 
            {
                return Status == "waiting" ? 0 : Status == "complete" ? 2 : Status == "graphing" ? 1 : -1;
            }
        }
    }
}