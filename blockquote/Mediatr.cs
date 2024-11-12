using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using MediatR;


internal sealed class JekyllDirectoryInfoGetRequest : IRequest<DirectoryInfo>
{

}
internal sealed class JekyllDirectoryInfoGetRequestHandler : IRequestHandler<JekyllDirectoryInfoGetRequest, DirectoryInfo>
{
    public async Task<DirectoryInfo> Handle(JekyllDirectoryInfoGetRequest request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];
    }
}

internal sealed class HtmlFileGetStreamRequest : IStreamRequest<FileInfo>
{
    public DirectoryInfo? DirectoryInfo { get; init; }
}
internal sealed class HtmlFileGetStreamHandler : IStreamRequestHandler<HtmlFileGetStreamRequest, FileInfo>
{
    public async IAsyncEnumerable<FileInfo> Handle(HtmlFileGetStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var item in request.DirectoryInfo!.EnumerateFiles("*.html", new EnumerationOptions() { RecurseSubdirectories = true }))
            yield return item;

        await Task.Yield();
    }
}

internal sealed class BlockquoteFormatterRequest : IRequest<string?>
{
    public FileInfo? FileInfo { get; init; }
}
internal sealed class BlockquoteFormatterRequestHandler : IRequestHandler<BlockquoteFormatterRequest, string?>
{
    public async Task<string?> Handle(BlockquoteFormatterRequest request, CancellationToken cancellationToken)
    {
        using var stream = request.FileInfo!.OpenRead();
        await Task.Yield();
        return BlockquoteFormatter.Format(stream);
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
            p.SetAttributeValue("style", "display:flex; align-items:center; column-gap:0.4em; font-weight:500;");

            if (highlight?.Key != default)
                p.InnerHtml = p.InnerHtml.Replace(highlight.Key, string.Empty);

            if (highlight?.Color != default)
            {
                var span = HtmlNode.CreateNode($"<span style='color:{highlight.Color};'>{highlight.Name}</span>");
                p.PrependChild(span);
                p.PrependChild(HtmlNode.CreateNode(highlight.Key));
            }
        }

        public struct Highlight
        {
            public string? Key { get; set; }
            public string? Color { get; set; }
            public string? Name { get; set; }
        }
    }
}