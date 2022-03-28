var target = Argument("target", "Default");
var DebugConfiguration = Argument("configuration", "Debug");
var ReleaseConfiguration = Argument("configuration", "Release");
var solutionFolder = "./src";
var projFile = "./src/Sudo";
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

Task("Run")
    .IsDependentOn("Version")
    .Does(() => {
		var arguments = new ProcessArgumentBuilder();
        arguments.Append("notepad.exe");
        DotNetCoreRun(projFile, arguments, new DotNetCoreRunSettings{
            Configuration = DebugConfiguration
        });
    });

Task("Publish-Win-x64")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ outputFolder }/win-x64",
            Runtime = "win-x64"
        });
    });

Task("Publish-Win-arm64")
    .Does(() => {
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = ReleaseConfiguration,
            OutputDirectory = $"{ outputFolder }/win-arm64",
            Runtime = "win-arm64"
        });
    });

Task("Publish-Win-x86")
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