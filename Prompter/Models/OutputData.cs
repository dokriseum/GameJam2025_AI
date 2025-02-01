namespace ConsoleApp.Models;

[Serializable]
public class OutputData
{
    public string Timestamp { get; set; }
    public string Model { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }

    public OutputData() { } // Parameterloser Konstruktor für Serialisierung
}