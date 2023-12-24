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
		this.logger.LogInformation($"{ConfigurationFields.Template}:\t [required] Path of the template file.");
		this.logger.LogInformation($"{ConfigurationFields.Data}:\t\t [required] Path of the data file.");
		this.logger.LogInformation($"{ConfigurationFields.Output}:\t [required] Path of the output file.");
		this.logger.LogInformation($"{ConfigurationFields.Css}:\t\t [optional] [multiple] Path of css file to include in generated file.");
		this.logger.LogInformation($"{ConfigurationFields.Asset}:\t\t [optional] [multiple] Path of other file to copy to output.");
		this.logger.LogInformation($"{ConfigurationFields.Watch}:\t\t [optional] Watch for files update and auto render.");
	}
}
