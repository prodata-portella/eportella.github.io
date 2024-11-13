using Microsoft.Extensions.DependencyInjection;
using MediatR;
var services = new ServiceCollection();
services.AddMediatR(mediatorServiceConfiguration => mediatorServiceConfiguration.RegisterServicesFromAssemblyContaining<Program>());

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

await foreach (var fileInfo in mediator.CreateStream(new CssFileGetStreamRequest { DirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest()) }))
{
    await mediator.Send(new CssMoveRequest { FileInfo = fileInfo });
    Console.WriteLine($"move -> '{fileInfo.FullName}' success!");
}

await foreach (var fileInfo in mediator.CreateStream(new HtmlFileGetStreamRequest { DirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest()) }))
{
    await mediator.Send(new BuildRequest { FileInfo = fileInfo });

    Console.WriteLine($"Portellas builder say->'{fileInfo.FullName}' success!");
}
