using System.Buffers.Text;
using System.Text;
using System.Text.Encodings.Web;
using BlazeHelper.GameManager;
using BlazeHelper.Testing;
using Newtonsoft.Json;
using System.Web;
using BlazeHelper.UI;
using BlazeHelper.BlazeConnection;

namespace BlazeHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Program.Print("Hello World!");
            new Thread( async () => 
            {
                await Connection.Connect();
            } ).Start(); 
            
            Config.Configuration.Start();

            Crash.WebView = webView;
            webView.WebMessageReceived += MessageReceived;

            Program.Print(Config.Configuration.ConfigurationFolder);
            webView.GotFocus += Loaded;
            var indexFilePath = "file:///" + Path.Combine(Config.Configuration.WebFilesFolder, "index.html");
            Program.Print("indexFilePath: " + indexFilePath);
            webView.Source = new(indexFilePath);
        }

        private async void Loaded(object? sender, EventArgs eventArgs)
        {
            //await Test.RunAsync();
        }

        private async void MessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs eventArgs)
        {
            //Program.Print("message received");
            string script = "";
            var json = eventArgs.WebMessageAsJson;
            var webEvent = JsonConvert.DeserializeObject<WebEvent>(json);
            
            if(webEvent is null)
                return;
            //Program.Print(webEvent.ClickedElementId.Length);
            if(webEvent.ClickedElementId.Length == 0)
            {
                var log = Convert.ToBase64String(Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(Crash.Log) ) ) ;
                script = $"showMessage('{log}', '')";
                Program.Print(script);
                await webView.Invoke(async () => await webView.ExecuteScriptAsync(script) );
                return;
            }

            await UIEventHandler.OnEvent(webEvent);
        }
    }
}
