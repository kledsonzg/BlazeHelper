using Newtonsoft.Json;
using System.Text;
using BlazeHelper.BlazeRecipes;
using BlazeHelper.BlazeConnection;

namespace BlazeHelper.Config
{
    public class User
    {
        // Email ou CPF
        public string UserIdentification { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        internal Tokens? Tokens {get; set;}
        internal UserInfo? Info {get; set;}
        internal UserExperience? Experience {get; set;}
        internal Wallet[] Wallets {get; set;} = Array.Empty<Wallet>();

        internal async Task<Task?> LoginAsync()
        {
            var json = JsonConvert.SerializeObject(this);

            json = json.Replace("\"UserIdentification\":", "\"username\":").Replace("\"Password\":", "\"password\":");
            Program.Print(json);
            
            // Trecho para obter os tokens no ato do login.
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(BlazeAPI.BlazeLoginLink)
            };
            //request.Headers.Add("Content-Type", "application/json");
            var response = await client.SendAsync(request);
            try
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Program.Print(responseBody);
                var tokens = JsonConvert.DeserializeObject<Tokens>(responseBody);
                Tokens = tokens;
                Program.Print(tokens);
            }
            catch(Exception exception)
            {
                Program.Print($"{exception.Message} | {exception.StackTrace}");
            }

            // Trecho para obter as informações da conta.
            try
            {
                if(Tokens is null)
                {
                    throw new NullReferenceException("Os tokens precisam ser setados. Possível erro no login.");
                }

                request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BlazeAPI.BLAZE_GET_USER_PERSONAL_INFO)
                };

                request.Headers.Add("Authorization", $"Bearer {Tokens.Refresh_Token}");
                //request.Headers.Add("Content-Type", "application/json");
                response = await client.SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                Program.Print(responseBody);
                var info = JsonConvert.DeserializeObject<UserInfo>(responseBody);
                if(info is null)
                {
                    throw new Exception("'UserInfo' obtido da Blaze é nulo.");
                }
                Info = info;
                Program.Print(info);
            }
            catch(Exception exception)
            {
                Program.Print($"{exception.Message} | {exception.StackTrace}");
            }

            // Trecho para obter a experiência do usuário.
            try
            {
                if(Tokens is null)
                {
                    throw new NullReferenceException("Os tokens precisam ser setados. Possível erro no login.");
                }

                request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BlazeAPI.BLAZE_GET_USER_EXPERIENCE)
                };
                request.Headers.Add("Authorization", $"Bearer {Tokens.Refresh_Token}");
                response = await client.SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                Program.Print(responseBody);
                var xp = JsonConvert.DeserializeObject<UserExperience>(responseBody);
                if(xp is null)
                {
                    throw new Exception("'UserExperience' obtido da Blaze é nulo.");
                }
                Experience = xp;
                Program.Print(xp);
            }
            catch(Exception exception)
            {
                Program.Print($"{exception.Message} | {exception.StackTrace}");
            }

            // Trecho para obter as carteiras do usuário.
            try
            {
                if(Tokens is null)
                {
                    throw new NullReferenceException("Os tokens precisam ser setados. Possível erro no login.");
                }

                request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BlazeAPI.BLAZE_GET_USER_WALLETS)
                };
                request.Headers.Add("Authorization", $"Bearer {Tokens.Refresh_Token}");
                response = await client.SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                Program.Print(responseBody);
                var wallets = JsonConvert.DeserializeObject<Wallet[]>(responseBody);
                if(wallets is null)
                {
                    throw new Exception("'Wallet' obtido da Blaze é nulo.");
                }
                Wallets = wallets;
                Program.Print(wallets);
                return Task.CompletedTask;
            }
            catch(Exception exception)
            {
                Program.Print($"{exception.Message} | {exception.StackTrace}");
            }
            return null;
        }

        internal async Task<BetResult?> Bet(double amount, double? autoCashOut)
        {
            if(Tokens is null)
            {
                throw new NullReferenceException($"Os tokens precisam ser setados. Para fazer isso, use o método 'Login()'.");
            }

            if(Wallets.Length == 0)
            {
                Program.Print("Falha ao tentar apostar pois não foi encontrado nenhuma identificação de carteira.");
                return null;
            }

            var bet = new Bet(this, Wallets[0], amount);
            bet.AutoCashOutAt = autoCashOut?.ToString("0.00").Replace(',', '.');

            var json = bet.GetCorrectJson();
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(BlazeAPI.BLAZE_BET_ENTER),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {Tokens.Refresh_Token}");
            //request.Headers.Add("Content-Type", "application/json");

            try
            {
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