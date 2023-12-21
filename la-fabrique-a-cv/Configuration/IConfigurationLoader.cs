// See https://aka.ms/new-console-template for more information
internal interface IConfigurationLoader
{
	Configuration? Load(string[] args);
}