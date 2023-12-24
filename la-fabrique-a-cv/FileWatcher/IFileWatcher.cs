namespace la_fabrique_a_cv
{
	internal interface IFileWatcher
	{
		void StartWatch(Configuration configuration, IEnumerable<IWorkflowStep> steps);
	}
}