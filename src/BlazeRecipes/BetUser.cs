using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public class BetUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("id_str")]
        public string IdStr {get; set; } = string.Empty;
        [JsonProperty("username")]
        public string Username {get; set;} = string.Empty;
        [JsonProperty("rank")]
        public string Rank {get; set;} = string.Empty;
    }
}