#addin nuget:?package=Cake.Docker&version=0.11.1

var target = Argument("target", "Default");
var DebugConfiguration = Argument("configuration", "Debug");
var ReleaseConfiguration = Argument("configuration", "Release");
var solutionFolder = "./src";
var projFile = "./src/BSmith.PWSH";
var benchmark = "./src/BSmith.PWSH.Benchmark";
var outputFolder = "./output";

Task("Clean")
    .Does(() => {
        CleanDirectory(outputFolder);
    });

Task("Version")
    .IsDependentOn("Clean")
    .Does(() => {
        var propsFile = "./src/Directory.Build.props";
        var readedVersion = XmlPeek(propsFile, "//Version");
        var currentVersion = new Version(readedVersion);
        var newMinor = currentVersion.Minor;

        if (target == "publish")
        {
            newMinor++;
        }

        var semVersion = new Version(currentVersion.Major, newMinor, currentVersion.Build + 1);
        var version = semVersion.ToString();

        XmlPoke(propsFile, "//Version", version);

        Information(version);
    });

Task("Major-Release")
    .Does(() => {
        var propsFile = "./src/Directory.Build.props";
        var readedVersion = XmlPeek(propsFile, "//Version");
        var currentVersion = new Version(readedVersion);

        var semVersion = new Version(currentVersion.Major + 1, 0, 0);
        var version = semVersion.ToString();

        XmlPoke(propsFile, "//Version", version);
    });

Task("Restore")
    .IsDependentOn("Version")
    .Does(() => {
        DotNetCoreRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings{
            Configuration = DebugConfiguration,
            NoRestore = true
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest(solutionFolder, new DotNetCoreTestSettings{
            Configuration = DebugConfiguration,
            NoRestore = true,
            NoBuild = true
        });
    });

Task("Run")
    .IsDependentOn("Test")
    .Does(() => {
        var callerInfo = Context.GetCallerInfo();
        var pwd = callerInfo.SourceFilePath.ToString();
        pwd = pwd.Remove(pwd.Length - 11);

        DockerRun(new DockerContainerRunSettings{
            Volume = new string[] { $"{ pwd }/src/BSmith.PWSH/bin/debug/netstandard2.0:/modules", $"{ pwd }/test:/scripts" }
        }, "mcr.microsoft.com/powershell", "pwsh", "-F /scripts/test.ps1");
    });

Task("Run-Release")
    .IsDependentOn("Publish")
    .IsDependentOn("Run")
    .Does(() => {
        var callerInfo = Context.GetCallerInfo();
        var pwd = callerInfo.SourceFilePath.ToString();
        pwd = pwd.Remove(pwd.Length - 11);

        DockerRun(new DockerContainerRunSettings{
            Volume = new string[] { $"{ pwd }/output:/modules", $"{ pwd }/test:/scripts" }
        }, "mcr.microsoft.com/powershell", "pwsh", "-F /scripts/test.ps1");
    });

Task("Benchmark")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCoreRun(benchmark, new DotNetCoreRunSettings{
            Configuration = ReleaseConfiguration,
            NoRestore = false,
            NoBuild = false
        });
    });

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = outputFolder
        });
    });

Task("Default")
    .IsDependentOn("Run");

RunTarget(target);