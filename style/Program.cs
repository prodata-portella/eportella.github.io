var Jekyll = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];

foreach (var file in Jekyll!.GetFiles("*.css", new EnumerationOptions() { RecurseSubdirectories = true }))
{

    using var fileStrem = file.OpenText();

    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();
    using var writer = @new.OpenWrite();
    fileStrem.BaseStream.CopyTo(writer);

    Console.WriteLine($"move -> '{@new.FullName}' success!");
}
