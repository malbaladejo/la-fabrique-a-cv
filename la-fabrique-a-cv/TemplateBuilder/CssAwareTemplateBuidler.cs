// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

internal class CssAwareTemplateBuidler : ITemplateBuidler
{
	private readonly ILogger<CssAwareTemplateBuidler> logger;

	public CssAwareTemplateBuidler(ILogger<CssAwareTemplateBuidler> logger)
	{
		this.logger = logger;
	}

	public string Build(Configuration configuration)
	{
		logger.LogInformation("Reading template");
		const string cssTag = "<!-- css -->";
		var template = File.ReadAllText(configuration.Template);

		if (configuration.CssFiles.Count > 0 && !template.Contains(cssTag))
		{
			this.logger.LogWarning($"Template does not contains {cssTag}");
			return template;
		}

		var cssTags = configuration.CssFiles.Select(this.GetFileName)
					.Select(f => $"<link href=\"{f}\" rel=\"stylesheet\">");

		logger.LogInformation("Adding css tags");
		template = template.Replace(cssTag, string.Join(Environment.NewLine, cssTags));

		return template;
	}

	private string GetFileName(string filePath)
	{
		var hash = this.CalculateMD5(filePath);

		return $"{Path.GetFileName(filePath)}?v={hash}";
	}

	private string CalculateMD5(string filename)
	{
		using (var md5 = MD5.Create())
		{
			using (var stream = File.OpenRead(filename))
			{
				var hash = md5.ComputeHash(stream);
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}
	}
}
