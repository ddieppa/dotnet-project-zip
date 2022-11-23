using CliWrap;
using Spectre.Console;



//var currDir = AnsiConsole.Ask<string>("Enter [green]directory[/] to list files in:");
var currDir = $"C:\\servicechannel\\dev";
IEnumerable<string> directoriesToIgnore = new List<string> { "node_modules", "bin", "obj", ".idea" };
var childDirectories = Directory.GetDirectories(currDir);
var directories = childDirectories.Where(x => !directoriesToIgnore.Any(x.Contains));

// Progress
await AnsiConsole.Progress()
    .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new SpinnerColumn())
    .StartAsync(async ctx =>
    {
        await Task.WhenAll(directories.Select(async directoryName =>
        {
            var task = ctx.AddTask(directoryName, new ProgressTaskSettings
            {
                AutoStart = false
            });

            await GitArchiveDirectory(task, directoryName);
        }));
    });
AnsiConsole.MarkupLine("Done!");


async Task GitArchiveDirectory(ProgressTask task, string directory)
{
    try
    {
        task.StartTask();
        var gitArchiveCmd = Cli.Wrap("git")
            .WithWorkingDirectory(directory)
            .WithValidation(CommandResultValidation.None)
            .WithArguments(args => args
                .Add("archive")
                .Add("HEAD")
                .Add("--format=zip")
                .Add("-o")
                .Add($"{directory}.zip")
            );
        await gitArchiveCmd.ExecuteAsync();
        AnsiConsole.MarkupLine($"[green]Archived[/] {directory}");
        
    }
    catch (Exception e)
    {
       AnsiConsole.MarkupLine($"[red]Error: {e.Message}[/]");
    }

}

void WriteLogMessage(string message)
{
    AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
}

