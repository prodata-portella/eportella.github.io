using Microsoft.Extensions.DependencyInjection;
using MediatR;
var services = new ServiceCollection();
services
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>))
    .AddTransient(typeof(IStreamPipelineBehavior<,>), typeof(StreamLoggingPipelineBehavior<,>))
    .AddMediatR(mediatorServiceConfiguration => mediatorServiceConfiguration.RegisterServicesFromAssemblyContaining<Program>());

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

await foreach (var fileInfo in mediator.CreateStream(new CssFileGetStreamRequest { DirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest()) }))
    await mediator.Send(new CssMoveRequest { FileInfo = fileInfo });

await foreach (var fileInfo in mediator.CreateStream(new HtmlFileGetStreamRequest { DirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest()) }))
    await mediator.Send(new BuildRequest { FileInfo = fileInfo });
