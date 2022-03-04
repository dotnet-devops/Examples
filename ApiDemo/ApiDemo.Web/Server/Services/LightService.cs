namespace ApiDemo.Web.Server.Services
{
    public class LightService
    {
        private readonly HttpClient _http;
        public LightService()
        {
            _http = new();
            _http.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BaseAddress"));
            _http.DefaultRequestHeaders.Add("authority", "10.0.0.207");
            _http.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
        }

        public async Task ChangeColor(string color)
        {
            await _http.PutAsJsonAsync("state", new { on = true });
            await _http.PutAsJsonAsync("state", new
            {
                xy = color switch
                {
                    "blue" => new[] { 0.167, 0.04 },
                    "cyan" => new[] { 0.2857, 0.2744 },
                    "green" => new[] { 0.4091, 0.518 },
                    "magenta" => new[] { 0.3826, 0.1597 },
                    "orangered" => new[] { 0.6725, 0.3230 },
                    "red" => new[] { 0.675, 0.322 },
                    "white" => new[] { 0.3227, 0.3290 },
                    _ => new[] { 0.3227, 0.3290 }

                }
            });
        }

        private async Task ColorfyMeCaptain()
        {
            await _http.PutAsJsonAsync("state", new { on = true });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { xy = new[] { 0.2857, 0.2744 } });
            await Task.Delay(500);
            await _http.PutAsJsonAsync("state", new { on = false });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { on = true });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { xy = new[] { 0.167, 0.04 } });
            await Task.Delay(500);
            await _http.PutAsJsonAsync("state", new { on = false });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { on = true });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { xy = new[] { 0.167, 0.04 } });
            await Task.Delay(500);
            await _http.PutAsJsonAsync("state", new { on = false });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { on = true });
            await Task.Delay(125);
            await _http.PutAsJsonAsync("state", new { xy = new[] { 0.5754, 0.3480 } });
            await Task.Delay(500);
        }

        public async Task Off()
        {
            await _http.PutAsJsonAsync("state", new { on = false });
        }

        public async Task On()
        {
            await _http.PutAsJsonAsync("state", new { on = true });
        }

        public async Task TweetNotification()
        {
            try
            {
                await _http.PutAsJsonAsync("state", new { on = true });
                int pass = 0;
                do
                {
                    await ColorfyMeCaptain();
                    pass += 1;
                } while (pass < 5);

                await _http.PutAsJsonAsync("state", new { on = false });
            }
            catch
            {
                Console.WriteLine(); // Used for a salty breakpoint.
            }
        }
    }
}
