var target = Argument("target", "Default");
var DebugConfiguration = Argument("configuration", "Debug");
var ReleaseConfiguration = Argument("configuration", "Release");
var solutionFolder = "./src";
var projFile = "./src/BSmith.Console.Net48";
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
        DotNetCoreRun(projFile, new DotNetCoreRunSettings{
            Configuration = DebugConfiguration,
            NoRestore = true,
            NoBuild = true
        });
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePack(solutionFolder, new DotNetCorePackSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = outputFolder,
            NoRestore = true,
            NoBuild = true
        });
    });

Task("Publish-Win-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ outputFolder }/win-x64",
            Runtime = "win-x64"
        });
    });

Task("Publish-Win-arm64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ outputFolder }/win-arm64",
            Runtime = "win-arm64"
        });
    });

Task("Publish-Win-x86")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ outputFolder }/win-x86",
            Runtime = "win-x86"
        });
    });

Task("Publish")
    .IsDependentOn("Publish-Win-x64")
    .IsDependentOn("Publish-Win-arm64")
    .IsDependentOn("Publish-Win-x86");

Task("Default")
    .IsDependentOn("Run");

RunTarget(target);