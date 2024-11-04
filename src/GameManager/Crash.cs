using System.Text;
using BlazeHelper.BlazeRecipes;
using BlazeHelper.Config;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;

namespace BlazeHelper.GameManager
{
    internal static class Crash
    {
        internal static User? User { get; set; }
        internal static List<GameEvent> GameTicks = new();
        internal static List<GameEvent> BetTicks = new();
        internal static WebView2? WebView {get; set;}
        internal static string Log {private set; get; } = string.Empty;
        internal static async Task<Task> OnWebsocketMessage(string msg)
        {
            //Program.Print(msg);
            try
            {
                var evt = GameEvent.ByWebSocketMessage(msg);
                if(evt is null)
                    return Task.CompletedTask;
                
                Log += $"{msg}\n";

                //Program.Print(evt.Id);
                
                if(evt.IsTickEvent() )
                {
                    var ticks = new List<GameEvent>(GameTicks);
                    lock(GameTicks)
                    {
                        // Se maior que 0, significa que não houve troca de status do jogo.
                        if(GameTicks.Count(s => s.Payload.Id == evt.Payload.Id && s.Payload.Status == evt.Payload.Status) > 0)
                            return Task.CompletedTask;

                        GameTicks.Add(evt);      
                    }

                    await UpdateGameStatus(evt, true);
                    return Task.CompletedTask;
                }
                else if(evt.IsBetsEvent() )
                {
                    var bets = evt.Payload.Bets;
                    // Program.Print("\nBETS: ");
                                
                    // if(bets is not null) foreach(var bet in bets)
                    // {
                    //     Program.Print($"({bet.Id}) {bet.User.Username} : {bet.Amount}. Cashed Out At: {bet.CashedOutAt ?? "No"}.");
                    // }
                    // Program.Print("\nEOF BETS: \n");
                    var base64Json = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bets) ) );

                    if(WebView is null)
                        return Task.CompletedTask;
                    await WebView.Invoke(async () => await WebView.ExecuteScriptAsync($"updateBetList('{base64Json}')"));
                }
            }
            catch(Exception exception)
            {
                Program.Print("Falha ao processar mensagem WebSocket. Motivo: " + $"{exception.Message} | {exception.StackTrace}");
            }

            return Task.CompletedTask;
        }

        private static async Task UpdateGameStatus(GameEvent tick, bool clearBetList)
        {
            if(WebView is null)
                throw new NullReferenceException("Antes de atualizar o status do jogo, é necessário 'setar' a instância da WebView.");
            
            var status = tick.Payload.StatusCode;
            var msg = $"{(status == 2 ? $"'{tick.Payload.Id} : {tick.Payload.CrashPoint}X'" : "''")}";

            var script = $"setGameStatus({status}, {msg})";
            if(clearBetList)
                script += "; clearBetList()";
            
            //Program.Print($"script: {script} | crash: {tick.Payload.CrashPoint}");
            await WebView.Invoke(async () => await WebView.ExecuteScriptAsync(script) );
        }

        internal static async Task<string> GetJavascriptResult(string script)
        {
            string result = string.Empty;
            if(WebView is null)
                return string.Empty;

            await WebView.Invoke(async () => result = await WebView.ExecuteScriptAsync(script) );
            if(result.Length > 2)
                return result.Substring(1, result.Length - 2);
            return string.Empty;
        }
    }
}