using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;
using MediatR;

internal sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    Stopwatch Stopwatch { get; }
    public LoggingPipelineBehavior(
    )
    {
        Stopwatch = new Stopwatch();
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Stopwatch.Start();
        
        var response = await next();
        Stopwatch.Stop();
        Console.WriteLine($"{typeof(TRequest).FullName} Time Elapsed {Stopwatch.ElapsedMilliseconds}ms");
        return response;
    }
}

internal sealed class StreamLoggingPipelineBehavior<TRequest, TResponse> : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
{
    Stopwatch Stopwatch { get; }
    public StreamLoggingPipelineBehavior(
    )
    {
        Stopwatch = new Stopwatch();
    }
    public async IAsyncEnumerable<TResponse> Handle(
        TRequest request,
        StreamHandlerDelegate<TResponse> next,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        Stopwatch.Start();
        await foreach (var item in next())
            yield return item;
        Stopwatch.Stop();
        Console.WriteLine($"{typeof(TRequest).FullName} Time Elapsed {Stopwatch.ElapsedMilliseconds}ms");
    }
}

internal sealed class RootDirectoryInfoGetRequest : IRequest<DirectoryInfo>
{

}
internal sealed class RootDirectoryInfoGetRequestHandler : IRequestHandler<RootDirectoryInfoGetRequest, DirectoryInfo>
{
    public async Task<DirectoryInfo> Handle(RootDirectoryInfoGetRequest request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;
    }
}

internal sealed class JekyllDirectoryInfoGetRequest : IRequest<DirectoryInfo>
{

}
internal sealed class JekyllDirectoryInfoGetRequestHandler(IMediator mediator) : IRequestHandler<JekyllDirectoryInfoGetRequest, DirectoryInfo>
{
    
    public async Task<DirectoryInfo> Handle(JekyllDirectoryInfoGetRequest request, CancellationToken cancellationToken)
    {
        return (await mediator.Send(new RootDirectoryInfoGetRequest(), cancellationToken))!.GetDirectories("_jekyll")[0];
    }
}

internal sealed class CssFileGetStreamRequest : IStreamRequest<FileInfo>
{
    public DirectoryInfo? DirectoryInfo { get; init; }
}
internal sealed class CssFileGetStreamHandler : IStreamRequestHandler<CssFileGetStreamRequest, FileInfo>
{
    public async IAsyncEnumerable<FileInfo> Handle(CssFileGetStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var item in request.DirectoryInfo!.EnumerateFiles("*.css", new EnumerationOptions() { RecurseSubdirectories = true }))
            yield return item;

        await Task.Yield();
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

internal sealed class MarkdownFileInfoGetStreamRequest : IStreamRequest<FileInfo>
{
    public DirectoryInfo? DirectoryInfo { get; init; }
}
internal sealed class MarkdownFileInfoGetStreamHandler : IStreamRequestHandler<MarkdownFileInfoGetStreamRequest, FileInfo>
{
    public async IAsyncEnumerable<FileInfo> Handle(MarkdownFileInfoGetStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var item in request.DirectoryInfo!.EnumerateFiles("*.md", new EnumerationOptions() { RecurseSubdirectories = true }))
            yield return item;

        await Task.Yield();
    }
}

internal sealed class BlockquoteFormatRequest : IRequest<string?>
{
    public FileInfo? FileInfo { get; init; }
}
internal sealed class BlockquoteFormatRequestHandler : IRequestHandler<BlockquoteFormatRequest, string?>
{
    public async Task<string?> Handle(BlockquoteFormatRequest request, CancellationToken cancellationToken)
    {
        using var stream = request.FileInfo!.OpenRead();
        await Task.Yield();
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

        if (highlight.Key != default)
            p.InnerHtml = p.InnerHtml.Replace(highlight.Key, string.Empty);

        if (highlight.Color != default)
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

internal sealed class SvgFormatRequest : IRequest<string?>
{
    public string? Content { get; init; }
}
internal sealed class SvgFormatRequestHandler : IRequestHandler<SvgFormatRequest, string?>
{
    public async Task<string?> Handle(SvgFormatRequest request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        if (request.Content == default)
            return default;

        return request
            .Content
            .Replace(
                "[!NOTE]",
                SvgCreate("#1f6feb", "M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8Zm8-6.5a6.5 6.5 0 1 0 0 13 6.5 6.5 0 0 0 0-13ZM6.5 7.75A.75.75 0 0 1 7.25 7h1a.75.75 0 0 1 .75.75v2.75h.25a.75.75 0 0 1 0 1.5h-2a.75.75 0 0 1 0-1.5h.25v-2h-.25a.75.75 0 0 1-.75-.75ZM8 6a1 1 0 1 1 0-2 1 1 0 0 1 0 2Z"))
            .Replace(
                "[!TIP]",
                SvgCreate("#3fb950", "M8 1.5c-2.363 0-4 1.69-4 3.75 0 .984.424 1.625.984 2.304l.214.253c.223.264.47.556.673.848.284.411.537.896.621 1.49a.75.75 0 0 1-1.484.211c-.04-.282-.163-.547-.37-.847a8.456 8.456 0 0 0-.542-.68c-.084-.1-.173-.205-.268-.32C3.201 7.75 2.5 6.766 2.5 5.25 2.5 2.31 4.863 0 8 0s5.5 2.31 5.5 5.25c0 1.516-.701 2.5-1.328 3.259-.095.115-.184.22-.268.319-.207.245-.383.453-.541.681-.208.3-.33.565-.37.847a.751.75 0 0 1-1.485-.212c.084-.593.337-1.078.621-1.489.203-.292.45-.584.673-.848.075-.088.147-.173.213-.253.561-.679.985-1.32.985-2.304 0-2.06-1.637-3.75-4-3.75ZM5.75 12h4.5a.75.75 0 0 1 0 1.5h-4.5a.75.75 0 0 1 0-1.5ZM6 15.25a.75.75 0 0 1 .75-.75h2.5a.75.75 0 0 1 0 1.5h-2.5a.75.75 0 0 1-.75-.75Z"))
            .Replace(
                "[!IMPORTANT]",
                SvgCreate("#ab7df8", "M0 1.75C0 .784.784 0 1.75 0h12.5C15.216 0 16 .784 16 1.75v9.5A1.75 1.75 0 0 1 14.25 13H8.06l-2.573 2.573A1.458 1.458 0 0 1 3 14.543V13H1.75A1.75 1.75 0 0 1 0 11.25Zm1.75-.25a.25.25 0 0 0-.25.25v9.5c0 .138.112.25.25.25h2a.75.75 0 0 1 .75.75v2.19l2.72-2.72a.749.749 0 0 1 .53-.22h6.5a.25.25 0 0 0 .25-.25v-9.5a.25.25 0 0 0-.25-.25Zm7 2.25v2.5a.75.75 0 0 1-1.5 0v-2.5a.75.75 0 0 1 1.5 0ZM9 9a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"))
            .Replace(
                "[!WARNING]",
                SvgCreate("#d29922", "M6.457 1.047c.659-1.234 2.427-1.234 3.086 0l6.082 11.378A1.75 1.75 0 0 1 14.082 15H1.918a1.75 1.75 0 0 1-1.543-2.575Zm1.763.707a.25.25 0 0 0-.44 0L1.698 13.132a.25.25 0 0 0 .22.368h12.164a.25.25 0 0 0 .22-.368Zm.53 3.996v2.5a.75.75 0 0 1-1.5 0v-2.5a.75.75 0 0 1 1.5 0ZM9 11a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"))
            .Replace(
                "[!CAUTION]",
                SvgCreate("#f85149", "M4.47.22A.749.749 0 0 1 5 0h6c.199 0 .389.079.53.22l4.25 4.25c.141.14.22.331.22.53v6a.749.749 0 0 1-.22.53l-4.25 4.25A.749.749 0 0 1 11 16H5a.749.749 0 0 1-.53-.22L.22 11.53A.749.749 0 0 1 0 11V5c0-.199.079-.389.22-.53Zm.84 1.28L1.5 5.31v5.38l3.81 3.81h5.38l3.81-3.81V5.31L10.69 1.5ZM8 4a.75.75 0 0 1 .75.75v3.5a.75.75 0 0 1-1.5 0v-3.5A.75.75 0 0 1 8 4Zm0 8a1 1 0 1 1 0-2 1 1 0 0 1 0 2Z")
            );
    }

    static string SvgCreate(string color, string shape)
    {
        var namespaceUri = "http://www.w3.org/2000/svg";
        var svgElement = new XElement(XName.Get("svg", namespaceUri),
            new XAttribute("viewBox", "0 0 16 16"),
            new XAttribute("version", "1.1"),
            new XAttribute("width", "16"),
            new XAttribute("height", "16"),
            new XAttribute("aria-hidden", "true"),
            new XElement(XName.Get("path", namespaceUri),
                new XAttribute("d", shape),
                new XAttribute("style", $"fill:{color}")
            )
        );

        return svgElement.ToString();
    }
}


internal sealed class IdadeBuildRequest : IRequest<string?>
{
    public string? Content { get; init; }
}
internal sealed class IdadeBuildRequestHandler : IRequestHandler<IdadeBuildRequest, string?>
{
    Regex Regex { get; }
    public IdadeBuildRequestHandler()
    {
        Regex = new Regex(@"<code.*?>\[IDADE\]:([\d]{4}\-[\d]{2}\-[\d]{2})\<\/code>", RegexOptions.Multiline);
    }
    public async Task<string?> Handle(IdadeBuildRequest request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        if (request.Content == default)
            return default;

        var content = request.Content;
        do
        {
            var match = Regex.Match(content);
            if (!match.Success)
                break;
            content = content.Replace(
                match.Groups[0].Value,
                Calculate(DateTime.ParseExact(match.Groups[1].Value, "yyyy-mm-dd", CultureInfo.InvariantCulture)).ToString()
            );

        } while (true);

        return content;
    }

    internal static int Calculate(DateTime birthDate)
    {
        DateTime today = DateTime.Today;

        int age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age).Date)
            return age - 1;

        return age;
    }
}

internal sealed class CssMoveRequest : IRequest
{
    public FileInfo? FileInfoSource { get; init; }
    public FileInfo? FileInfoTarget { get; init; }
}
internal sealed class CssMoveRequestHandler : IRequestHandler<CssMoveRequest>
{
    public async Task Handle(CssMoveRequest request, CancellationToken cancellationToken)
    {
        if (!request.FileInfoTarget!.Directory!.Exists)
            request.FileInfoTarget.Directory.Create();
        using var writer = request.FileInfoTarget.OpenWrite();
        using var fileStrem = request.FileInfoSource!.OpenText();
        await fileStrem.BaseStream.CopyToAsync(writer, cancellationToken);
    }
}

internal sealed class BuildRequest : IRequest
{
    public FileInfo? FileInfoSource { get; init; }
    public FileInfo? FileInfoTarget { get; init; }
}
internal sealed class BuildRequestHandler(IMediator mediator) : IRequestHandler<BuildRequest>
{
    public async Task Handle(BuildRequest request, CancellationToken cancellationToken)
    {
        if (!request.FileInfoTarget!.Directory!.Exists)
            request.FileInfoTarget.Directory.Create();
        var content = await mediator.Send(new BlockquoteFormatRequest { FileInfo = request.FileInfoSource }, cancellationToken);
        content = await mediator.Send(new SvgFormatRequest { Content = content }, cancellationToken);
        content = await mediator.Send(new IdadeBuildRequest { Content = content }, cancellationToken);
        using var writer = request.FileInfoTarget.CreateText();
        await writer.WriteAsync(content);
    }
}

internal sealed class LogRequest : IRequest
{
    public FileInfo? FileInfo { get; init; }
}
internal sealed class LogRequestHandler(IMediator mediator) : IRequestHandler<LogRequest>
{
    public async Task Handle(LogRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine($"LOG->'{request.FileInfo.FullName}'");
        using var reader = request.FileInfo.OpenText();
        var content = await reader.ReadToEndAsync();
        Console.WriteLine(content);
        Console.WriteLine();
        var regex = new Regex("^# (.+)$");
        var match = regex.Match(content);

        do
        {
            if(!match.Sucess)
                break;

            content = content.Replace(match.Groups[0].value, $"<h1>{match.Groups[1].Value}</h1>")
        } while(true);

        Console.WriteLine(content);
        Console.WriteLine();
    }
}