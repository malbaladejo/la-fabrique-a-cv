// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using System.Text;

internal class DocumentationWritter : IDocumentationWritter
{
    private readonly ILogger<DocumentationWritter> logger;

    public DocumentationWritter(ILogger<DocumentationWritter> logger)
    {
        this.logger = logger;
    }

    public void Write()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"Inline configuration.");
        stringBuilder.AppendLine($"{ConfigurationFields.Template}:\t [required] Path of the template file.");
        stringBuilder.AppendLine($"{ConfigurationFields.Data}:\t\t [required] Path of the data file.");
        stringBuilder.AppendLine($"{ConfigurationFields.Output}:\t [required] Path of the output file.");
        stringBuilder.AppendLine($"{ConfigurationFields.WorkingDirectory}:\t [optional] If {ConfigurationFields.WorkingDirectory} is defined, other pathes can be relative.");
        stringBuilder.AppendLine($"{ConfigurationFields.Css}:\t\t [optional] [multiple] Path of css file to include in generated file.");
        stringBuilder.AppendLine($"{ConfigurationFields.Asset}:\t\t [optional] [multiple] Path of other file to copy to output.");
        stringBuilder.AppendLine($"{ConfigurationFields.Watch}:\t\t [optional] Watch for files update and auto render.");
        stringBuilder.AppendLine($"{ConfigurationFields.ClearOutput}:\t\t [optional] Clear the output folder before generate.");
        stringBuilder.AppendLine($"");
        stringBuilder.AppendLine($"Configuration in json file.");
        stringBuilder.AppendLine($"{ConfigurationFields.Config}:\t\t Path of the config file.");
        stringBuilder.AppendLine($"{ConfigurationFields.New}:\t\t Create an empty config file in the path.");

        this.logger.LogInformation(stringBuilder.ToString());
    }
}
