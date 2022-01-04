using SystemdTemplate;
using Spectre.Console;
using System.Diagnostics;

if (args.Any() && args[0] == "install")
{
    string appName = nameof(SystemdTemplate);
    string processPath = Environment.ProcessPath!;
    string processFileName = Path.GetFileName(processPath);
    Console.WriteLine($"Installing {processFileName} as a systemd service");

    var unitFileContents = @$"[Unit]
Description={nameof(SystemdTemplate)}

[Service]
Type=notify
ExecStart={processPath}

[Install]
WantedBy=multi-user.target";

    var unitFilePath = $"/etc/systemd/system/{appName.ToLower()}.service";

    try
    {
        File.WriteAllText(unitFilePath, unitFileContents);
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] write failed to {unitFilePath}. Did you forget to use sudo?");
        AnsiConsole.WriteException(ex.Demystify());
        return 1;
    }

    return 0;
}

IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
return 0;