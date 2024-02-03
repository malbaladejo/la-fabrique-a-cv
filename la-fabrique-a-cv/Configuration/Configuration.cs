// See https://aka.ms/new-console-template for more information

internal class Configuration
{
	public string? WorkingDirectory { get; set; }
	public string? Template { get; set; }
	public string? Data { get; set; }
	public string? Output { get; set; }
	public List<string> CssFiles { get; } = new List<string>();
	public List<string> Assets { get; } = new List<string>();
	public bool WatchFiles { get; set; }
	public bool ClearOutput { get; set; }

	public void Initialize()
	{
		if (string.IsNullOrWhiteSpace(WorkingDirectory))
			return;

		if (!Path.IsPathRooted(this.Template))
			this.Template = Path.Combine(this.WorkingDirectory, this.Template);

		if (!Path.IsPathRooted(this.Data))
			this.Data = Path.Combine(this.WorkingDirectory, this.Data);

		if (!Path.IsPathRooted(this.Output))
			this.Output = Path.Combine(this.WorkingDirectory, this.Output);

		for (int i = 0; i < this.CssFiles.Count; i++)
		{
			if (!Path.IsPathRooted(this.CssFiles[i]))
				this.CssFiles[i] = Path.Combine(this.WorkingDirectory, this.CssFiles[i]);
		}

		for (int i = 0; i < this.Assets.Count; i++)
		{
			if (!Path.IsPathRooted(this.Assets[i]))
				this.Assets[i] = Path.Combine(this.WorkingDirectory, this.Assets[i]);
		}
	}
}
