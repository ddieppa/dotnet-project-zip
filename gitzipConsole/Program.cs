using CliWrap;
using Spectre.Console;



var currDir = AnsiConsole.Ask<string>("Enter [green]directory[/] to list files in:");

IEnumerable<string> directoriesToIgnore = new List<string> { "node_modules", "bin", "obj", ".idea" };
var childDirectories = Directory.GetDirectories(currDir);
var directories = childDirectories.Where(x => !directoriesToIgnore.Any(x.Contains));


foreach (var directory in directories)
{
    AnsiConsole.MarkupLine($"[green]{directory}[/]");
    await GitArchiveDirectory(directory);
}

async Task GitArchiveDirectory(string directory)
{
    var gitArchiveCmd = Cli.Wrap("git")
        .WithWorkingDirectory(directory)
        .WithArguments(args => args
            .Add("archive")
            .Add("HEAD")
            .Add("--format=zip")
            .Add("-o")
            .Add($"{directory}.zip")
        );
    AnsiConsole.MarkupLine($"[yellow]{gitArchiveCmd}[/]");
    
    await gitArchiveCmd.ExecuteAsync();

}

