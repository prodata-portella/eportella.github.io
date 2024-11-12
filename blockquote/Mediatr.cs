using System.Runtime.CompilerServices;
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
    public DirectoryInfo? DirectoryInfo { get; set; }
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