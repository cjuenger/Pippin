using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    const string NuGetSource = "https://api.nuget.org/v3/index.json";

    public static int Main () => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Parameter("NuGet API Key", Name = "NUGET_API_KEY_PIPPIN")] 
    [CanBeNull] readonly string NuGetApiKey;
    
    [GitVersion] readonly GitVersion GitVersion;
    [Solution] readonly Solution Solution;

    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath OutputDirectory => RootDirectory / "build";
    static AbsolutePath PackageOutputDirectory => OutputDirectory / "packages";
    
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/{obj,bin}").DeleteDirectories();
            OutputDirectory.GlobDirectories("**/.output").DeleteDirectories();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            if (GitVersion == null)
            {
                Log.Warning("GitVersion appears to be null. Have a look at it! Versions are defaulting to 0.1.0 for now...");
            }
            
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion?.AssemblySemVer ?? "0.1.0")
                .SetFileVersion(GitVersion?.AssemblySemFileVer ?? "0.1.0")
                .SetInformationalVersion(GitVersion?.InformationalVersion ?? "0.1.0")
                .EnableNoRestore());
        });
    
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetConfiguration(Configuration)
                .SetProjectFile(Solution)
                .EnableNoRestore()
                .EnableNoBuild());
        });
    
    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            var packableProjects = Solution?
                .AllProjects
                .Where(project => project.GetProperty<bool>("IsPackable")) ?? Enumerable.Empty<Project>();

            foreach (var project in packableProjects)
            {
                Log.Information("Packaging project '{ProjectName}'...", project.Name);
                
                DotNetPack(settings => settings
                    .SetProject(project)
                    .SetOutputDirectory(PackageOutputDirectory)
                    .SetConfiguration(Configuration)
                    .EnableNoBuild()
                    .EnableNoRestore()
                    .SetVersion(GitVersion?.NuGetVersionV2)
                    .SetAssemblyVersion(GitVersion?.AssemblySemVer)
                    .SetFileVersion(GitVersion?.AssemblySemFileVer)
                    .SetInformationalVersion(GitVersion?.InformationalVersion));
            }
        });
    
    // ReSharper disable once UnusedMember.Local
    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => !string.IsNullOrWhiteSpace(NuGetApiKey))
        .Executes(() =>
        {
            var packageFiles = PackageOutputDirectory.GlobFiles("*.nupkg");

            foreach (var packageFile in packageFiles)
            {
                Log.Information("Pushing '{PackageName}'...", packageFile.Name);
                
                DotNetNuGetPush(settings => settings
                    .SetApiKey(NuGetApiKey)
                    .SetSymbolApiKey(NuGetApiKey)
                    .SetTargetPath(packageFile)
                    .SetSource(NuGetSource)
                    .SetSymbolSource(NuGetSource));
            }
        });

}
