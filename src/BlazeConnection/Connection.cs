using System.Net.WebSockets;
using System.Text;

namespace BlazeHelper.BlazeConnection
{
    internal static class Connection
    {
        private static bool _connected = false;
        private static readonly Uri _url = new(BlazeAPI.BLAZE_CRASH_GAME);
        internal static bool Connected { get => _connected; }
        internal static void Disconnect()
        {
            _connected = false;
        }

        internal static async Task Connect()
        {
            ClientWebSocket client = new();
            Program.Print("Conectando-se ao servidor websocket...");
            await client.ConnectAsync(_url, CancellationToken.None );
            Program.Print("Conectado!");
            _connected = true;

            var reader = Task.Run(async () => 
            {
                while(client.State == WebSocketState.Open && _connected)
                {
                    WebSocketReceiveResult result;
                    MemoryStream stream = new();
                    do
                    {
                        var buffer = new byte[4096];
                        result = await client.ReceiveAsync(buffer, CancellationToken.None );
                        stream.Write(buffer, (int) stream.Position, result.Count);
                    }
                    while(!result.EndOfMessage);

                    var receivedMsg = Encoding.UTF8.GetString(stream.ToArray() );
                    await GameManager.Crash.OnWebsocketMessage(receivedMsg);
                }
            } );

            while(client.State == WebSocketState.Open && _connected)
            {
                var json = "420[\"cmd\", {\"id\": \"subscribe\",\"payload\": {\"room\": \"crash_room_4\"} } ]";
                ArraySegment<byte> bytes = new(Encoding.UTF8.GetBytes(json) );
                await client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None );
                Thread.Sleep(30 * 1000);
            }

            Program.Print("Conexão fechada com o servidor websocket.");
            _connected = false;

            await reader;
        }
    }
}