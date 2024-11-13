using Microsoft.Extensions.DependencyInjection;
using MediatR;
var services = new ServiceCollection();
services
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>))
    .AddTransient(typeof(IStreamPipelineBehavior<,>), typeof(StreamLoggingPipelineBehavior<,>))
    .AddMediatR(mediatorServiceConfiguration => mediatorServiceConfiguration.RegisterServicesFromAssemblyContaining<Program>());

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();
var jekyllDirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest());
await foreach (var cssFileInfo in mediator.CreateStream(new CssFileGetStreamRequest { DirectoryInfo = jekyllDirectoryInfo }))
    await mediator.Send(new CssMoveRequest 
    { 
        FileInfoSource = cssFileInfo,
        FileInfoTarget = new FileInfo(cssFileInfo!.FullName.Replace("/_jekyll/", "/_site/"))
    });

await foreach (var htmlFileInfo in mediator.CreateStream(new HtmlFileGetStreamRequest { DirectoryInfo = jekyllDirectoryInfo }))
    await mediator.Send(new BuildRequest 
    { 
        FileInfoSource = htmlFileInfo,
        FileInfoTarget = new FileInfo(htmlFileInfo!.FullName.Replace("/_jekyll/", "/_site/"))
    });
