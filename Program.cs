using CliWrap;
using SystemdTemplate;
using Spectre.Console;
using System.Diagnostics;

if (args.Any() && args[0] == "install")
{
    string serviceName = nameof(SystemdTemplate).ToLower();
    string processPath = Environment.ProcessPath!;
    string processFileName = Path.GetFileName(processPath);
    Console.WriteLine($"Installing {processFileName} as a systemd service");

    var unitFileContents = @$"[Unit]
Description={serviceName}

[Service]
Type=notify
ExecStart={processPath}

[Install]
WantedBy=multi-user.target";

    var unitFilePath = $"/etc/systemd/system/{serviceName}.service";

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

    await (Cli.Wrap("systemctl").WithArguments($"enable {serviceName}") |
        (Console.WriteLine, Console.Error.WriteLine)).ExecuteAsync();

    await (Cli.Wrap("systemctl").WithArguments($"start {serviceName}") |
        (Console.WriteLine, Console.Error.WriteLine)).ExecuteAsync();

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