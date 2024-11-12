using System.Text.RegularExpressions;

var Jekyll = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];

var regex = new Regex(@"`\[IDADE\]:(\w+)`", RegexOptions.Multiline);
var today = DateTime.Today;
foreach (var file in Jekyll!.GetFiles("*.html", new EnumerationOptions() { RecurseSubdirectories = true }))
{
    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();

    using var fileStrem = file.OpenText();
    var content = fileStrem.ReadToEnd();
    do
    {
        var match = regex.Match(content);

        if (!match.Success)
            break;
        Console.WriteLine("TODAY:" + today);
        Console.WriteLine("MATCH:" + match.Result(match.Groups["1"].Value));


    } while (true);
    
    using var writer = @new.OpenWrite();
    fileStrem.BaseStream.CopyTo(writer);

    Console.WriteLine($"move -> '{@new.FullName}' success!");
}
