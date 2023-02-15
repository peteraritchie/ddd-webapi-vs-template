namespace Tests.Common;

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

    public void AddProjectReference(CSharpProject project)
    {
        referencedProjects.Add(project);
    }

    public IEnumerable<CSharpProject> ReferencedProjects => referencedProjects.AsReadOnly();
}
