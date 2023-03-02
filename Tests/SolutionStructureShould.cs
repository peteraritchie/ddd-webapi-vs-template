using System.Diagnostics;
using Microsoft.Build.Construction;
using Tests.Common;

namespace Tests
{
	public class SolutionStructureShould
	{
		private const string infrastructureProjectName = "Infrastructure";
		private const string coreProjectName = "Domain";
		private readonly string[] applicationProjectNames = { "WebApi" };
		private readonly List<CSharpProject> projects = new();
		private readonly string[] testProjectNames = { "Tests" };

		public SolutionStructureShould()
		{
			var queue = new Queue<FileInfo>(new[] { new FileInfo(@"..\..\..\Tests.csproj") });

			do
			{
				var fileInfo = queue.Dequeue();
				Debug.Assert(fileInfo.Exists);
				var project = projects.Find(p => p.FilePath == fileInfo.FullName);
				if (project == null)
				{
					project = new CSharpProject(fileInfo);
					projects.Add(project);
				}

				var rootElement = ProjectRootElement.Open(fileInfo.FullName);
				Debug.Assert(rootElement is { });
				var projectReferenceElements = rootElement.ItemGroups
					.SelectMany(g => g.Items)
					.Where(i => i.ElementName == "ProjectReference");

				foreach (var e in projectReferenceElements)
				{
					var referencedFileInfo = new FileInfo(
						Path.Combine(
							fileInfo.DirectoryName!,
							e.Include));

					var referencedProject = projects.Find(p => p.FilePath == referencedFileInfo.FullName);
					if (referencedProject == null)
					{
						referencedProject = new CSharpProject(referencedFileInfo);
						projects.Add(referencedProject);
					}

					project.AddProjectReference(referencedProject);
					queue.Enqueue(referencedFileInfo);
				}
			} while (queue.Count > 0);
		}

		[Fact]
		public void HaveInfrastructureNotReferenceApplicationProjects()
		{
			Assert.Empty(
				projects
					.Single(e => e.Name == infrastructureProjectName)
					.ReferencedProjects
					.Where(e => applicationProjectNames.Contains(e.Name)));
		}

		[Fact]
		public void HaveInfrastructureReferenceCoreProject()
		{
			Assert.NotEmpty(
				projects
					.Single(e => e.Name == infrastructureProjectName)
					.ReferencedProjects
					.Where(e => e.Name == coreProjectName));
		}

		[Fact]
		public void HaveNoProjectsThatReferenceTests()
		{
			var nonTestProjects = projects.Where(e => !testProjectNames.Contains(e.Name));
			var projectsThatReferenceTests = nonTestProjects
				.Where(rp => rp.ReferencedProjects.Any(p => testProjectNames.Contains(p.Name)));
			Assert.Empty(projectsThatReferenceTests);
		}

		[Fact]
		public void HaveCoreProjectNotReferenceOtherProjects()
		{
			Assert.Empty(projects.Single(e => e.Name == coreProjectName).ReferencedProjects);
		}
	}
}
