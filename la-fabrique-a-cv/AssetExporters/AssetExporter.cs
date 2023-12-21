using Microsoft.Extensions.Logging;

internal class AssetExporter
{
	private readonly ILogger<AssetExporter> logger;

	public AssetExporter(ILogger<AssetExporter> logger)
	{
		this.logger = logger;
	}

	public void Export(Configuration configuration)
	{
		this.logger.LogInformation("Exporting Css files");
		foreach (var item in configuration.CssFiles)
			this.ExportFile(configuration, item);

		this.logger.LogInformation("Exporting Asset files");
		foreach (var item in configuration.Assets)
			this.ExportFile(configuration, item);
	}

	private void ExportFile(Configuration configuration, string filePath)
	{
		this.logger.LogInformation($"Copying {filePath}");
		File.Copy(filePath, Path.Combine(Path.GetDirectoryName(configuration.Output), Path.GetFileName(filePath)), true);
	}
}
