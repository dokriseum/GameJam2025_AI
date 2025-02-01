using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ConsoleApp.Models;

namespace ConsoleApp.Controllers;

public class APIController
{
    private readonly HttpClient _client = new HttpClient();

    public async Task<DeepSeekResponse?> FetchResponse(string model, string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
        request.Headers.Add("Accept", "application/json");

        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            model = model,
            prompt = prompt,
            stream = false
        }), Encoding.UTF8, "application/json");

        request.Content = content;

        try
        {
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return DeepSeekResponse.FromJson(result);
            }
            else
            {
                Console.WriteLine($"API Error: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Request Error: {ex.Message}");
        }

        return null;
    }
}