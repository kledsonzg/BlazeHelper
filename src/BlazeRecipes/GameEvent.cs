using Newtonsoft.Json;

namespace BlazeHelper.BlazeRecipes
{
    public class GameEvent
    {
        public string Id {get; set;} = string.Empty;
        public GamePayload Payload {get; set;} = new();

        internal static GameEvent? ByWebSocketMessage(string msg)
        {
            //Program.Print(msg);
            msg = msg.Substring(msg.IndexOf('{'));
            msg = msg.Substring(0, msg.LastIndexOf('}') + 1);
            //Program.Print(msg);
            return JsonConvert.DeserializeObject<GameEvent>(msg);
        }

        internal bool IsBetsEvent() => Id == "crash.tick-bets";
        internal bool IsTickEvent() => Id == "crash.tick";
    }
}