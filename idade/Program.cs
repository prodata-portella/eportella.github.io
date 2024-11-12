using System.Globalization;
using System.Text.RegularExpressions;

var Jekyll = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];
var regex = new Regex(@"<code.*>\[IDADE\]:([\d]{4}\-[\d]{2}\-[\d]{2})\<\/code>", RegexOptions.Multiline);

foreach (var file in Jekyll!.GetFiles("*.html", new EnumerationOptions() { RecurseSubdirectories = true }))
{
    using var fileStrem = file.OpenText();
    var content = fileStrem.ReadToEnd();
    do
    {
        var match = regex.Match(content);
        if (!match.Success)
            break;
        content = content.Replace(
            match.Groups[0].Value, 
            AgeCalculator.Calculate(DateTime.ParseExact(match.Groups[1].Value, "yyyy-mm-dd", CultureInfo.InvariantCulture)).ToString()
        );

    } while (true);

    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();

    using var writer = @new.CreateText();
    writer.Write(content);

    Console.WriteLine($"move -> '{@new.FullName}' success!");
}

class AgeCalculator
{
    internal static int Calculate(DateTime birthDate)
    {
        DateTime today = DateTime.Today;

        // Calcula a idade em anos diretamente
        int age = today.Year - birthDate.Year;

        // Ajusta a idade se o aniversário ainda não aconteceu este ano
        if (birthDate.Date > today.AddYears(-age).Date)
        {
            age--;
        }

        return age;
    }
}
