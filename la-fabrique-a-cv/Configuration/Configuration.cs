// See https://aka.ms/new-console-template for more information

internal class Configuration
{
	public string? Template { get; set; }
	public string? Data { get; set; }
	public string? Output { get; set; }
	public List<string> CssFiles { get; } = new List<string>();
	public List<string> Assets { get; } = new List<string>();

}
