using CliWrap;
using Spectre.Console;



var currDir = AnsiConsole.Ask<string>("Enter [green]directory[/] to list files in:");

IEnumerable<string> directoriesToIgnore = new List<string> { "node_modules", "bin", "obj", ".idea" };
var childDirectories = Directory.GetDirectories(currDir);
var directories = childDirectories.Where(x => !directoriesToIgnore.Any(x.Contains));


var table = new Table().Expand().BorderColor(Color.Grey);
table.AddColumn("[yellow]Directory[/]");
table.AddColumn("[green]Success[/]");
table.AddColumn("[red]Fail[/]");

AnsiConsole.MarkupLine("Press [yellow]CTRL+C[/] to exit");

await AnsiConsole.Live(table)
    .AutoClear(false)
    .Overflow(VerticalOverflow.Ellipsis)
    .Cropping(VerticalOverflowCropping.Bottom)
    .StartAsync(async ctx =>
    {
        
        foreach (var directory in directories)
        {

            await GitArchiveDirectory(directory, table, ctx);
            
        }
    });



async Task GitArchiveDirectory(string directory, Table table, LiveDisplayContext ctx)
{
    try
    {
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
        var result = await gitArchiveCmd.ExecuteAsync();
        if (result.ExitCode != 0)
        {
            table.AddRow(directory, "", "X");
        }
        else
        {
            table.AddRow(directory, "X", "");
        }
        ctx.Refresh();
        
        
    }
    catch (Exception e)
    {
        table.AddRow(directory, "", "X");
    }

}

