using Microsoft.Extensions.DependencyInjection;
using MediatR;
var services = new ServiceCollection();
services
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>))
    .AddTransient(typeof(IStreamPipelineBehavior<,>), typeof(StreamLoggingPipelineBehavior<,>))
    .AddMediatR(mediatorServiceConfiguration => mediatorServiceConfiguration.RegisterServicesFromAssemblyContaining<Program>());

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();
var jekyll = await mediator.Send(new JekyllDirectoryInfoGetRequest());
await foreach (var fileInfo in mediator.CreateStream(new CssFileGetStreamRequest { DirectoryInfo = jekyll }))
    await mediator.Send(new CssMoveRequest { FileInfo = fileInfo });

await foreach (var fileInfo in mediator.CreateStream(new HtmlFileGetStreamRequest { DirectoryInfo = jekyll }))
    await mediator.Send(new BuildRequest { FileInfo = fileInfo });
