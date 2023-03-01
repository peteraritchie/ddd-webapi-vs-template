namespace Tests.Common
{
	public record CSharpProject
	{
		private readonly List<CSharpProject> referencedProjects = new();

		public CSharpProject(FileSystemInfo fileInfo)
		{
			Name = Path.GetFileNameWithoutExtension(fileInfo.Name);
			FilePath = fileInfo.FullName;
		}

		public string Name { get; set; }
		public string FilePath { get; set; }

		public IEnumerable<CSharpProject> ReferencedProjects => referencedProjects.AsReadOnly();

		public void AddProjectReference(CSharpProject project)
		{
			referencedProjects.Add(project);
		}
	}
}
