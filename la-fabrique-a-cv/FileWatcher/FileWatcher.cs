using Microsoft.Extensions.Logging;

namespace la_fabrique_a_cv
{
	internal class FileWatcher : IFileWatcher, IDisposable
	{
		private readonly ILogger<FileWatcher> logger;
		private bool isBusy;
		private readonly List<FileSystemWatcher> watches = new List<FileSystemWatcher>();

		public FileWatcher(ILogger<FileWatcher> logger)
		{
			this.logger = logger;
		}

		public void StartWatch(Configuration configuration, IEnumerable<IWorkflowStep> steps)
		{
			this.logger.LogInformation("Start watching");
			var files = GetFilesToWatch(configuration);
			var directories = files.Select(f => Path.GetDirectoryName(f)).Distinct();

			foreach (var directory in directories)
				RegisterFile(directory, configuration, steps);
		}

		private IEnumerable<string> GetFilesToWatch(Configuration configuration)
		{
			yield return configuration.Template;
			yield return configuration.Data;

			foreach (var item in configuration.CssFiles)
				yield return item;

			foreach (var item in configuration.Assets)
				yield return item;
		}

		private void RegisterFile(string directory, Configuration configuration, IEnumerable<IWorkflowStep> steps)
		{
			this.logger.LogInformation($"Watching {directory}");

			var watcher = new FileSystemWatcher(directory);

			this.watches.Add(watcher);
			watcher.NotifyFilter = NotifyFilters.Attributes
								 | NotifyFilters.CreationTime
								 | NotifyFilters.DirectoryName
								 | NotifyFilters.FileName
								 | NotifyFilters.LastAccess
								 | NotifyFilters.LastWrite
								 | NotifyFilters.Security
								 | NotifyFilters.Size;

			watcher.Changed += (s, e) => OnChanged(e, configuration, steps);
			watcher.EnableRaisingEvents = true;
		}

		private async Task OnChanged(FileSystemEventArgs e, Configuration configuration, IEnumerable<IWorkflowStep> steps)
		{
			if (this.isBusy)
				return;

			this.isBusy = true;
			await Task.Delay(200);
			try
			{
				this.logger.LogInformation($"{e.FullPath} has changed.");
				this.logger.LogInformation($"Render begin.");
				foreach (var step in steps)
				{
					if (!step.Execute(configuration))
						break;
				}
				this.logger.LogInformation($"Render end.");
			}
			finally
			{
				this.isBusy = false;
			}
		}

		private bool _disposedValue;

		// Public implementation of Dispose pattern callable by consumers.
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					foreach (var item in watches)
						item.Dispose();
				}

				_disposedValue = true;
			}
		}
	}
}
