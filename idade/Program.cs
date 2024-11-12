using System.Globalization;
using System.Text.RegularExpressions;

var Jekyll = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];

var regex = new Regex(@"`\[IDADE\]:([\d]{4}\-[\d]{2}\-[\d]{2})`", RegexOptions.Multiline);
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

        Console.WriteLine("find:" + regex.ToString() + "->" + match.Success);
        if (!match.Success)
            break;
        Console.WriteLine("TODAY:" + today);
        Console.WriteLine("MATCH:" + match.Groups[1].Value);

        var age = AgeCalculator.Calculate(DateTime.ParseExact(match.Groups[1].Value, "yyyy-mm-dd", CultureInfo.InvariantCulture));

        Console.WriteLine("AGE:" + age);

        regex.Replace(match.Groups[0].Value, age.ToString());

    } while (true);

    using var writer = @new.OpenWrite();
    fileStrem.BaseStream.CopyTo(writer);

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
