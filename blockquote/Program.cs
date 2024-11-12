using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
var services = new ServiceCollection();
services.AddMediatR(mediatorServiceConfiguration => mediatorServiceConfiguration.RegisterServicesFromAssemblyContaining<Program>());

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

await foreach (var file in mediator.CreateStream(new HtmlFileGetStreamRequest { DirectoryInfo = await mediator.Send(new JekyllDirectoryInfoGetRequest()) }))
{
    var @new = new FileInfo(file.FullName.Replace("/_jekyll/", "/_site/"));

    if (!@new.Directory!.Exists)
        @new.Directory.Create();

    
    using var writer = @new.CreateText();
    await writer.WriteAsync(await mediator.Send(new BlockquoteFormatterRequest { FileInfo = file }));

    Console.WriteLine($"Portellas builder say->'{file.FullName}' success!");
}
