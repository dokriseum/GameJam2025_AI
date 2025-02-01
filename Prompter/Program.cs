using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp;

class Program
{
    private static string cName, cFrage, cArgumentationsstil;
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        var prompt = "Schlüpfe in die Rolle eines populistischen Politikers ohne dies zu erwähnen, der mit einfachen Lösungen auf komplexe Probleme reagiert. Verwende emotionale Sprache, 'Wir gegen die'-Rhetorik und kritisiere die politische Elite, während du einfache, aber polarisierende Lösungen für gesellschaftliche Probleme anbietest.\n\n"; // Erstelle die HTTP-Request-Meldung
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
        
        // Füge den Accept-Header hinzu
        request.Headers.Add("Accept", "application/json");

        // Promptvariablen füttern
        //request.Content = new StringContent(prompt, Encoding.UTF8, "application/json");
        cName = args[0].Replace("\"", "");
        cFrage = args[1].Replace("\"", "");
        cArgumentationsstil = args[2].Replace("\"", "");
        prompt +=  $"Dein Name ist {cName} und kandidierst für eine rechtsextreme und rechtspopulistische Partei. Du bist in deinen Äußerungen sehr bedacht, sodass du den gesetzlichen Rahmen stark ausschöpfst. Jetzt wirst du in eine Talkshow eingeladen und sollst über deine Positionen reden. Du benutzt bewusst Falschaussagen und deine eigene Faktenlagen, die auf Pseudowissenschaften beruhen. Du bist ein Klimawandelleugner, nationalistisch, menschenverachtend und generell driftest du in populistisch-faschistischen Themen ab. In der Talkshow versuchst du möglichst viele potentielle Wähler von dir zu überzeugen. Sei bewusst sehr populistisch, treffe einfache Aussagen für komplexe Themen, die sich trotzdem sachlich anhören. Der Moderator stellt dir die Frage \"{cFrage}\". Beantworte die Frage kurz und knapp in maximal zwei Sätzen und im Stil {cArgumentationsstil}.";

        // Erstelle den JSON-Inhalt und setze die Content-Type-Header über das StringContent-Objekt
        var content = new StringContent(
            JsonConvert.SerializeObject(new
            {
                model = "deepseek-r1:1b",
                prompt = prompt,
                stream = false
            }), 
            Encoding.UTF8, 
            "application/json"
        );
        
        // Setze den Inhalt der Anfrage
        request.Content = content;

        try
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Fertig!");
    }
}