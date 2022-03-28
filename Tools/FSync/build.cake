var target = Argument("target", "Default");
var DebugConfiguration = Argument("configuration", "Debug");
var ReleaseConfiguration = Argument("configuration", "Release");
var solutionFolder = "./src";
var projFile = "./src/FSync";
var outputFolder = "./output";
var selfcontainedOutputFolder = $"{ outputFolder }/self-contained";
var dependentOutputFolder = $"{ outputFolder }/framework-dependent";

Task("Clean")
    .Does(() => {
        CleanDirectory(outputFolder);
		CleanDirectory("./test/destination/1");
		CleanDirectory("./test/destination/2");
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
		var arguments = new ProcessArgumentBuilder();
		arguments.Append("/mnt/c/Users/besmi/Development/BSStandard/Tools/FSync/test/source/");
		arguments.Append("/mnt/c/Users/besmi/Development/BSStandard/Tools/FSync/test/destination/");
        DotNetCoreRun(projFile, arguments, new DotNetCoreRunSettings{
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
            OutputDirectory = $"{ selfcontainedOutputFolder }/win-x64",
            PublishSingleFile = true,
            SelfContained = true,
            Runtime = "win-x64"
        });
    });

Task("Publish-Linux-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ selfcontainedOutputFolder }/linux-x64",
            PublishSingleFile = true,
            SelfContained = true,
            Runtime = "linux-x64"
        });
    });

Task("Publish-Osx-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ selfcontainedOutputFolder }/osx-x64",
            PublishSingleFile = true,
            SelfContained = true,
            Runtime = "osx-x64"
        });
    });

Task("Publish-Win-arm64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ selfcontainedOutputFolder }/win-arm64",
            PublishSingleFile = true,
            SelfContained = true,
            Runtime = "win-arm64"
        });
    });

Task("Publish-Win-x86")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ selfcontainedOutputFolder }/win-x86",
            PublishSingleFile = true,
            SelfContained = true,
            Runtime = "win-x86"
        });
    });

Task("Publish-Dependent-Win-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ dependentOutputFolder }/win-x64",
            PublishSingleFile = true,
            SelfContained = false,
            Runtime = "win-x64"
        });
    });

Task("Publish-Dependent-Linux-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ dependentOutputFolder }/linux-x64",
            PublishSingleFile = true,
            SelfContained = false,
            Runtime = "linux-x64"
        });
    });

Task("Publish-Dependent-Osx-x64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ dependentOutputFolder }/osx-x64",
            PublishSingleFile = true,
            SelfContained = false,
            Runtime = "osx-x64"
        });
    });

Task("Publish-Dependent-Win-arm64")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ dependentOutputFolder }/win-arm64",
            PublishSingleFile = true,
            SelfContained = false,
            Runtime = "win-arm64"
        });
    });

Task("Publish-Dependent-Win-x86")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ dependentOutputFolder }/win-x86",
            PublishSingleFile = true,
            SelfContained = false,
            Runtime = "win-x86"
        });
    });

Task("Publish")
    .IsDependentOn("Publish-Win-x64")
    .IsDependentOn("Publish-Linux-x64")
    .IsDependentOn("Publish-Osx-x64")
    .IsDependentOn("Publish-Win-arm64")
    .IsDependentOn("Publish-Win-x86")
    .IsDependentOn("Publish-Dependent-Win-x64")
    .IsDependentOn("Publish-Dependent-Linux-x64")
    .IsDependentOn("Publish-Dependent-Osx-x64")
    .IsDependentOn("Publish-Dependent-Win-arm64")
    .IsDependentOn("Publish-Dependent-Win-x86");

Task("Default")
    .IsDependentOn("Run");

RunTarget(target);
