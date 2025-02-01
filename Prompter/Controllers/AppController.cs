using ConsoleApp.Models;
using ConsoleApp.Views;
using ConsoleApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp.Controllers;

public class AppController
{
    private readonly APIController _apiController = new APIController();

    public async Task Run(string[] args)
    {
        var arguments = args.Select(arg => arg.Split('='))
                            .ToDictionary(parts => parts[0].ToLower(), parts => parts.Length > 1 ? parts[1] : "");

        string cFrage = arguments.GetValueOrDefault("-pfrage", "");
        string cStil = arguments.GetValueOrDefault("-pstil", "");
        string cInfos = arguments.GetValueOrDefault("-pinfos", "");
        string cModel = arguments.GetValueOrDefault("-pmodel", "default-model");
        string outputPath = arguments.GetValueOrDefault("-poutput", Directory.GetCurrentDirectory());

        string prompt = Prompts.PROMPT_001(cFrage, cStil, cInfos);

        var response = await _apiController.FetchResponse(cModel, prompt);
        if (response != null)
        {
            Console.WriteLine("\n\n" + prompt + "\n\n");
            Console.WriteLine(response.Response);
            SaveOutput(prompt, response.Response, cModel, outputPath);
        }
    }

    private void SaveOutput(string prompt, string response, string model, string outputPath)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss");
        string jsonFilename = Path.Combine(outputPath, $"{timestamp}.json");
        string xmlFilename = Path.Combine(outputPath, $"{timestamp}.xml");

        var outputData = new OutputData
        {
            Timestamp = timestamp,
            Model = model,
            Prompt = prompt,
            Response = response
        };

        File.WriteAllText(jsonFilename, System.Text.Json.JsonSerializer.Serialize(outputData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OutputData));
        using (TextWriter writer = new StreamWriter(xmlFilename))
        {
            serializer.Serialize(writer, outputData);
        }
    }
}
