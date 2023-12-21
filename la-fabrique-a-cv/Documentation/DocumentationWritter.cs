// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;

internal class DocumentationWritter : IDocumentationWritter
{
	private readonly ILogger<DocumentationWritter> logger;

	public DocumentationWritter(ILogger<DocumentationWritter> logger)
	{
		this.logger = logger;
	}

	public void Write()
	{
		Console.WriteLine($"{ConfigurationFields.Template}:\t [required] Path of the template file.");
		Console.WriteLine($"{ConfigurationFields.Data}:\t\t [required] Path of the data file.");
		Console.WriteLine($"{ConfigurationFields.Output}:\t [required] Path of the output file.");
		Console.WriteLine($"{ConfigurationFields.Css}:\t\t [optional] [multiple] Path of css file to include in generated file.");
		Console.WriteLine($"{ConfigurationFields.Asset}:\t\t [optional] [multiple] Pathof other file to copy to output.");
	}
}
