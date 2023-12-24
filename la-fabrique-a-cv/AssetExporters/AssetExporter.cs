using la_fabrique_a_cv;
using Microsoft.Extensions.Logging;

internal class AssetExporter : IWorkflowStep
{
	private readonly ILogger<AssetExporter> logger;

	public AssetExporter(ILogger<AssetExporter> logger)
	{
		this.logger = logger;
	}

	public bool Execute(Configuration configuration)
	{
		this.logger.LogInformation("Exporting Css files");
		foreach (var item in configuration.CssFiles)
			this.ExportFile(configuration, item);

		this.logger.LogInformation("Exporting Asset files");
		foreach (var item in configuration.Assets)
			this.ExportFile(configuration, item);

		return true;
	}

	private void ExportFile(Configuration configuration, string filePath)
	{
		this.logger.LogInformation($"Copying {filePath}");
		File.Copy(filePath, Path.Combine(Path.GetDirectoryName(configuration.Output), Path.GetFileName(filePath)), true);
	}
}
