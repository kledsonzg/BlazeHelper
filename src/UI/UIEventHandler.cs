using System.Text;
using BlazeHelper.Config;
using BlazeHelper.GameManager;

namespace BlazeHelper.UI
{
    internal static class UIEventHandler
    {
        private static Func<WebEvent, Task>[] actions = 
        {
            DoLogin,
            Logout,
            Bet
        };
        internal static async Task OnEvent(WebEvent webEvent)
        {
            foreach(var action in actions)
            {
                await action(webEvent);
            }
        }

        private static async Task Bet(WebEvent webEvent)
        {
            if(webEvent.ClickedElementId != "btn-bet")
                return;
            if(Crash.User == null)
            {
                await Crash.GetJavascriptResult("alert('Login necessário!'); openLoginWindow();");
                return;
            }

            
        }

        private static async Task Logout(WebEvent webEvent)
        {
            if(webEvent.ClickedElementId != "btn-logout")
                return;
            Crash.User = null;
            await Crash.GetJavascriptResult("showLoginNavbar()");
        }

        private static async Task DoLogin(WebEvent webEvent)
        {
            //Program.Print(webEvent);
            if(webEvent.ClickedElementId != "btn-do-login")
                return;
            await ShowMessage("FAZENDO LOGIN...");
            string email = await Crash.GetJavascriptResult("game.inputs.email.value");
            string pwd = await Crash.GetJavascriptResult("game.inputs.password.value");
            Program.Print($"DoLogin: user: {email} | pwd: {pwd}");
            var user = new User()
            {
                UserIdentification = email,
                Password = pwd
            };
            var task = await user.LoginAsync();
            if(task is null)
            {
                await ShowMessage("Falha no login.");
                return;
            }
            Crash.User = user;
            
            foreach(var wallet in user.Wallets)
            {
                await Crash.GetJavascriptResult(@$"
                    let option = document.createElement('option');
                    option.value = '{wallet.Id}';
                    option.innerText = '{wallet.Currency.Symbol} {wallet.Balance}';
                    document.querySelector('#select-wallet').appendChild(option);
                ");
            }
            string script = @$"
                document.querySelector('#txt-username').innerText = '{user.Info?.Username}';
                closeLoginWindow();
                showLoggedUserNavbar();
            ";
            Program.Print(script);
            await Crash.GetJavascriptResult(script);
            await HideMessageContainer();
        }

        internal static async Task HideMessageContainer()
        {
            await Crash.GetJavascriptResult("document.querySelector('#container-msg').classList.toggle('visible', false)");
        }

        internal static async Task ShowMessage(string msg)
        {
            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(msg) );
            await Crash.GetJavascriptResult($"showMessage('{base64String}', '')");
        }
    }
}