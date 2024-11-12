using MediatR;


internal sealed class JekyllDirectoryInfoGetRequest : IRequest<DirectoryInfo>
{

}
internal sealed class JekyllDirectoryInfoGetHendler : IRequestHandler<JekyllDirectoryInfoGetRequest, DirectoryInfo>
{
    public async Task<DirectoryInfo> Handle(JekyllDirectoryInfoGetRequest request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return new DirectoryInfo(Directory.GetCurrentDirectory()).Parent!.GetDirectories("_jekyll")[0];
    }
}