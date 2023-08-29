using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [GitVersion] readonly GitVersion GitVersion;
    [Solution] readonly Solution Solution;
    
    AbsolutePath SourceDirectory => RootDirectory / "src";

    AbsolutePath BuildDirectory => RootDirectory / "build";
    
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/{obj,bin}").DeleteDirectories();
            BuildDirectory.GlobDirectories("**/.output").DeleteDirectories();
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

}
