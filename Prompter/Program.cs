using ConsoleApp.Controllers;

namespace ConsoleApp;

// dotnet clean Prompter.sln && dotnet build Prompter.sln -c Release && ./Prompter/bin/Release/net9.0/Prompter -pFrage="Wie wollen Sie die erneuerbaren Energien stärker fördern?" -pStil="Leugnung" -pInfos="Der menschengemachte Klimawandel kann man ja nicht bestreiten. Der basiert ja auf Fakten und ist real existierend." -pModel="hf.co/Undi95/Toppy-M-7B-GGUF:Q8_0"
class Program
{
    static async Task Main(string[] args)
    {
        var app = new AppController();
        await app.Run(args);
    }
}