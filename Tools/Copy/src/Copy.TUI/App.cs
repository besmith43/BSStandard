using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Terminal.Gui;
using Copy.Class;

namespace Copy.TUI
{
    public static class App
    {
        public static FileSystemInfo sourceInfo { get; set; }
        public static FileSystemInfo destinationInfo { get; set; }

        public static void Start()
        {
            Application.Init();

            var top = Application.Top;

            var win = new Window("Copy")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            var menu = new MenuBar( new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Quit", "", () => { if (Quit()) top.Running = false;})
                }),
                new MenuBarItem("_Help", new MenuItem[] {
                    new MenuItem("_About", "", () => { 
                        MessageBox.Query(50, 5, "About Copy", $"Written by Blake Smith\nVersion: { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }", "Ok");
                    })
                })
            });

            top.Add(menu);

            var statusBar = new StatusBar (new StatusItem[] {
                new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => { if (Quit()) top.Running = false; })
            });

            top.Add(statusBar);

            var sourceLabel = new Label("Source") {
                X = 3,
                Y = 2
            };

            var sourceText = new TextField("")
            {
                X = Pos.Left(sourceLabel),
                Y = Pos.Top(sourceLabel) + 1,
                Width = Dim.Percent(80, true),
            };

            var destinationLabel = new Label("Destination")
            {
                X = Pos.Left(sourceText),
                Y = Pos.Top(sourceText) + 1
            };

            var destinationText = new TextField("")
            {
                X = Pos.Left(destinationLabel),
                Y = Pos.Top(destinationLabel) + 1,
                Width = Dim.Percent(80, true),
            };

            var addSourceButton = new Button("Add _Source")
            {
                X = Pos.Left(destinationText),
                Y = Pos.Top(destinationText) + 1
            };

            var addDestButton = new Button("Add _Destination")
            {
                X = Pos.Left(addSourceButton) + 20,
                Y = Pos.Top(addSourceButton)
            };

            var startCopyButton = new Button("Start _Copy")
            {
                X = Pos.Left(addSourceButton),
                Y = Pos.Top(addSourceButton) + 1
            };

            var copyProgressLabel = new Label("Copying in Progress")
            {
                X = Pos.Left(startCopyButton),
                Y = Pos.Top(startCopyButton) + 2
            };

            addSourceButton.Clicked += () =>
            {
                var sourceTreeExplorer = new FileSystemTree(null)
                {
                    source = true,

                    PathAction = (PathData) =>
                    {
                        sourceText.Text = PathData.path;
                        sourceInfo = PathData.info;
                    }
                };

                Application.Run(sourceTreeExplorer);
            };

            addDestButton.Clicked += () =>
            {
                var destinationTreeExplorer = new FileSystemTree(null)
                {
                    destination = true,

                    PathAction = (PathData) =>
                    {
                        destinationText.Text = PathData.path;
                        destinationInfo = PathData.info;
                    }
                };

                Application.Run(destinationTreeExplorer);
            };

            startCopyButton.Clicked += () =>
            {
                if (String.IsNullOrEmpty(sourceText.Text.ToString()))
                {
                    MessageBox.Query("Source Error", "Please select a target source to be copied", "Ok");
                }
                else if (String.IsNullOrEmpty(destinationText.Text.ToString()))
                {
                    MessageBox.Query("Destination Error", "Please select a target destination to place the source", "Ok");
                }
                else
                {
                   var copyProgress = new Progress(null)
                    {
                        sourPath = sourceText.Text.ToString(),
                        sourInfo = sourceInfo,
                        destPath = destinationText.Text.ToString(),
                        destInfo = destinationInfo
                    };

                    Application.Run(copyProgress);
               }
           };

            win.Add(
                sourceLabel,
                sourceText,
                destinationLabel,
                destinationText,
                addSourceButton,
                addDestButton,
                startCopyButton
            );

            Application.Run();
        }

        public static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Copy", "Are you sure that you want to quit this application?", "Yes", "No");
            return n == 0;
        }
    }
}
