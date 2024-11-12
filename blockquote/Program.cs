using HtmlAgilityPack;
using System.Xml.Linq;

var @base = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;

var Jekyll = @base!.GetDirectories("_jekyll")[0];
_ = @base.CreateSubdirectory("_site");

foreach (var file in Jekyll!.GetFiles("*.css", new EnumerationOptions() { RecurseSubdirectories = true }))
{

    using var fileStrem = file.OpenText();

    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();
    using var writer = @new.OpenWrite();
    fileStrem.BaseStream.CopyTo(writer);

    Console.WriteLine($"Portellas builder say->'{file.FullName}' success!");
}


foreach (var file in Jekyll!.GetFiles("*.html", new EnumerationOptions() { RecurseSubdirectories = true }))
{
    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();

    using var fileStrem = file.OpenRead();
    using var writer = @new.CreateText();
    writer.Write(BlockquoteFormatter.Format(fileStrem));

    Console.WriteLine($"Portellas builder say->'{file.FullName}' success!");
}



static class BlockquoteFormatter
{
    public static string? Format(FileStream stream)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.Load(stream);

        var blockquotes = htmlDocument.DocumentNode.SelectNodes("//blockquote");

        if (blockquotes == null)
            return htmlDocument.DocumentNode.OuterHtml;

        foreach (var blockquote in blockquotes)
        {
            var p = blockquote.SelectSingleNode("p");
            if (p == null)
                continue;

            if (p.InnerText.Trim().StartsWith("[!NOTE]"))
            {
                var highlight = new Highlight
                {
                    Key = "[!NOTE]",
                    Color = "#1f6feb",
                    Name = "Note"
                };
                blockquote.SetAttributeValue("style", $"border-color: {highlight.Color};");
                Format(p, highlight);
                continue;
            }
            if (p.InnerText.Trim().StartsWith("[!TIP]"))
            {
                var highlight = new Highlight
                {
                    Key = "[!TIP]",
                    Color = "#3fb950",
                    Name = "Tip"
                };
                blockquote.SetAttributeValue("style", $"border-color: {highlight.Color};");
                Format(p, highlight);
                continue;
            }
            if (p.InnerText.Trim().StartsWith("[!IMPORTANT]"))
            {
                var highlight = new Highlight
                {
                    Key = "[!IMPORTANT]",
                    Color = "#ab7df8",
                    Name = "Important"
                };
                blockquote.SetAttributeValue("style", $"border-color: {highlight.Color};");
                Format(p, highlight);
                continue;
            }
            if (p.InnerText.Trim().StartsWith("[!WARNING]"))
            {
                var highlight = new Highlight
                {
                    Key = "[!WARNING]",
                    Color = "#d29922",
                    Name = "Warning"
                };
                blockquote.SetAttributeValue("style", $"border-color: {highlight.Color};");
                Format(p, highlight);
                continue;
            }
            if (p.InnerText.Trim().StartsWith("[!CAUTION]"))
            {
                var highlight = new Highlight
                {
                    Key = "[!CAUTION]",
                    Color = "#f85149",
                    Name = "Caution"
                };
                blockquote.SetAttributeValue("style", $"border-color: {highlight.Color};");
                Format(p, highlight);
                continue;
            }
        }

        return htmlDocument.DocumentNode.OuterHtml;
    }

    private static void Format(HtmlNode p, Highlight highlight)
    {
        p.SetAttributeValue("style","display:flex; align-items:center; column-gap:0.4em; font-weight:500;");
        
        if (highlight?.Key != default)
            p.InnerHtml = p.InnerHtml.Replace(highlight.Key, string.Empty);

        if (highlight?.Color != default)
        {
            var span = HtmlNode.CreateNode($"<span style='color:{highlight.Color};'>{highlight.Name}</span>");
            p.PrependChild(span);
            p.PrependChild(HtmlNode.CreateNode(highlight.Key));
        }
    }

    public class Highlight
    {
        public string? Key { get; set; }
        public string? Color { get; set; }
        public string? Name { get; set; }
    }
}
