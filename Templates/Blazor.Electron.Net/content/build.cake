#addin nuget:?package=Cake.Electron.Net&version=1.1.0
#tool "nuget:?package=ElectronNet.CLI&version=9.31.2"

using Cake.Electron.Net;
using Cake.Electron.Net.Commands.Settings;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./src";
var projFile = "./src/BSmith.Blazor.Electron.Net";
var outputFolder = "./output";

Task("Init")
    .Does(() => {
        ElectronNetInit(projFile);
    });

Task("Version")
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

Task("Build")
    .IsDependentOn("Version")
    .Does(() => {
       if (DirectoryExists("./src/BSmith.Blazor.Electron.Net/wwwroot/less"))
        {
            Information("Compiling Less");
            StartProcess("powershell.exe", new ProcessSettings{ Arguments = "-NoProfile -Command lessc ./src/BSmith.Blazor.Electron.Net/wwwroot/less/site.less ./src/BSmith.Blazor.Electron.Net/wwwroot/css/site.css"});
        }

        if (DirectoryExists("./src/BSmith.Blazor.Electron.Net/wwwroot/ts"))
        {
            Information("Compiling TypeScript");
            StartProcess("powershell.exe", new ProcessSettings{ Arguments = "-NoProfile -Command tsc --outFile ./src/BSmith.Blazor.Electron.Net/wwwroot/js/site.js -t ES5 ./src/BSmith.Blazor.Electron.Net/wwwroot/ts/site.ts"});
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest(solutionFolder, new DotNetCoreTestSettings{
            Configuration = configuration
        });
    });

Task("Run")
    .IsDependentOn("Test")
    .Does(() => {
        ElectronNetStart(projFile);
    });

Task("Publish-Win-x64")
    .Does(() => {
        ElectronNetBuild(new ElectronNetBuildSettings{
            WorkingDirectory = projFile,
            ElectronTarget = ElectronTarget.Win,
            DotNetConfig = DotNetConfig.Release
        });
    });

Task("Publish-Linux-x64")
    .Does(() => {
        // looks like this is still failing too
        ElectronNetBuild(new ElectronNetBuildSettings{
            WorkingDirectory = projFile,
            ElectronTarget = ElectronTarget.Linux,
            DotNetConfig = DotNetConfig.Release
        });
    });

Task("Publish-Osx-x64")
    .Does(() => {
        // cmd: electronize build /target osx /PublishReadyToRun false
        // but can still only be built on macos
        ElectronNetBuild(new ElectronNetBuildSettings{
            WorkingDirectory = projFile,
            ElectronTarget = ElectronTarget.MacOs,
            DotNetConfig = DotNetConfig.Release
        });
    });

Task("Publish-Win-arm64")
<<<<<<< HEAD
    .Does(() => {
        // cmd: electronize build /target custom "win-arm64;win" /electron=arch arm64 /PublishReadyToRun false
        ElectronNetBuildCustom(new ElectronNetCustomBuildSettings{
			WorkingDirectory = projFile,
			ElectronTargetCustom = ElectronTargetCustom.PortableWinX86,
			DotNetConfig = DotNetConfig.Release,
			ElectronArch = "arm64"
		}); 
=======
    //.IsDependentOn("Test")
    .Does(() => {
        // cmd: electronize build /target custom "win-arm64;win" /electron-arch arm64 /PublishReadyToRun false
        DotNetCorePublish(projFile, new DotNetCorePublishSettings{
            Configuration = configuration,
            OutputDirectory = $"{ outputFolder }/win-arm64",
            PublishSingleFile = true,
            SelfContained = true,
            PublishReadyToRun = false,
            Runtime = "win-arm64"
        });
>>>>>>> 45e81b371388ee9f2012aaf7de460216580e15b0
    });

Task("Publish-Win-x86")
    .Does(() => {
        // needs to be modified to run this
        // electronize build /target custom win7-x86;win32 /electron-arch ia32 
        ElectronNetBuildCustom(new ElectronNetCustomBuildSettings{
            WorkingDirectory = projFile,
            ElectronTargetCustom = ElectronTargetCustom.PortableWinX86,
            DotNetConfig = DotNetConfig.Release,
            ElectronArch = "ia32"
        });
    });

Task("Publish-Win")
    .IsDependentOn("Test")
    .IsDependentOn("Publish-Win-x64")
    //.IsDependentOn("Publish-Win-arm64")
    .IsDependentOn("Publish-Win-x86");

Task("Publish")
    .IsDependentOn("Test");
    //.IsDependentOn("Publish-Linux-x64")
    //.IsDependentOn("Publish-Osx-x64")
    //.IsDependentOn("Publish-Win-arm64")
    //.IsDependentOn("Publish-Win-x86");

Task("Default")
    .IsDependentOn("Run");

RunTarget(target);
