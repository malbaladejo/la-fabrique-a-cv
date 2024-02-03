using Microsoft.Extensions.Logging;

namespace la_fabrique_a_cv
{
	internal class OuptBuilder : IWorkflowStep
	{
		private readonly ILogger<OuptBuilder> logger;
		private bool isEnabled = true;

		public OuptBuilder(ILogger<OuptBuilder> logger)
		{
			this.logger = logger;
		}

		public bool Execute(Configuration configuration)
		{
			if (!this.isEnabled)
				return true;

			this.isEnabled = false;

			var outputDirectory = Path.GetDirectoryName(configuration.Output);

			logger.LogInformation($"Ensuring output directory {outputDirectory}");

			if (Directory.Exists(outputDirectory) && configuration.ClearOutput)
				Directory.Delete(outputDirectory, true);

			if (!Directory.Exists(outputDirectory))
				Directory.CreateDirectory(outputDirectory);

			return true;
		}
	}
}
