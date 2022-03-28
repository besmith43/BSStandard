using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terminal.Gui;
using Terminal.Gui.Trees;

namespace RangerSharp.Class
{
    public class Class1
    {
        private Toplevel _top;
        private TreeView<FileSystemInfo> treeViewFiles;
        private bool cutFlag = false;
        private FileSystemInfo pasteSource;

        public int Add(int a, int b)
        {
            return a + b;
        }

        public void Start()
        {
            Application.Init();

            _top = Application.Top;

            var win = new Window("RangerSharp")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            _top.Add(win);

            var menu = new MenuBar( new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("Add _File", "", () => AddChildFileNode(), null, null, Key.CtrlMask | Key.F),
                    new MenuItem("Add Directory", "", () => AddChildDirectoryNode(), null, null, Key.CtrlMask | Key.N),
                    new MenuItem("Add Root", "", () => AddRootNode(), null, null, Key.CtrlMask | Key.T),
                    new MenuItem("_Quit", "", () => { if (Quit()) { Application.RequestStop(); } }, null, null, Key.CtrlMask | Key.Q)
                }),
                new MenuBarItem("_Edit", new MenuItem[] {
                    new MenuItem("C_ut", "", () => CutNode()),
                    new MenuItem("_Copy", "", () => CopyNode()),
                    new MenuItem("_Paste", "", () => PasteNode()),
                    new MenuItem("_Edit File", "", () => Edit(), null, null, Key.CtrlMask | Key.E),
                    new MenuItem("Delete", "", () => DeleteNode(), null, null, Key.CtrlMask | Key.D),
                    new MenuItem("Rename", "", () => RenameNode(), null, null, Key.CtrlMask | Key.R)
                }),
                new MenuBarItem("_Help", new MenuItem[] {
                    new MenuItem("_About", "", () => { 
                        MessageBox.Query(50, 5, "About RangerSharp", $"Written by Blake Smith\nVersion: { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }", "Ok");
                    })
                })
            });

            _top.Add(menu);

            /*
            var statusBar = new StatusBar (new StatusItem [] {
				new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => Quit()),
				new StatusItem(Key.CtrlMask | Key.N, "~^N~ Add Folder", () => AddChildDirectoryNode()),
                new StatusItem(Key.CtrlMask | Key.F, "~^F~ Add File", () => AddChildFileNode()),
				new StatusItem(Key.CtrlMask | Key.T, "~^T~ Add Root", () => AddRootNode()),
				new StatusItem(Key.CtrlMask | Key.R, "~^R~ Rename", () => RenameNode()),
                new StatusItem(Key.CtrlMask | Key.E, "~^E~ Edit File", () => Edit()),
                new StatusItem(Key.CtrlMask | Key.D, "~^D~ Delete", () => DeleteNode())
			});

            _top.Add(statusBar);
            */

            var filesLabel = new Label("File Tree:")
            {
                X = 0,
                Y = 0
            };

            win.Add(filesLabel);

            treeViewFiles = new TreeView<FileSystemInfo>()
            {
                X = 0,
                Y = Pos.Bottom(filesLabel),
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                AllowLetterBasedNavigation = true
            };

            treeViewFiles.ObjectActivated += TreeViewFiles_ObjectActivated;

            SetupFileTree();

            win.Add(treeViewFiles);

            SetupScrollBar();

            Application.Run(_top);
        }

        private void TreeViewFiles_ObjectActivated(ObjectActivatedEventArgs<FileSystemInfo> obj)
        {
            if (obj.ActivatedObject is FileInfo f)
            {
				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				sb.AppendLine ($"Path:{f.DirectoryName}");
				sb.AppendLine ($"Size:{f.Length:N0} bytes");
				sb.AppendLine ($"Modified:{ f.LastWriteTime}");
				sb.AppendLine ($"Created:{ f.CreationTime}");

                MessageBox.Query (f.Name, sb.ToString (), "Close");
            }

			if (obj.ActivatedObject is DirectoryInfo dir)
            {
				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				sb.AppendLine ($"Path:{dir.Parent?.FullName}");
				sb.AppendLine ($"Modified:{ dir.LastWriteTime}");
				sb.AppendLine ($"Created:{ dir.CreationTime}");

                MessageBox.Query(dir.Name, sb.ToString(), "Close");
    		}
        }

        private void SetupFileTree()
        {
            treeViewFiles.TreeBuilder = new DelegateTreeBuilder<FileSystemInfo>(GetChildren, (o) => o is DirectoryInfo);

            treeViewFiles.AspectGetter = FileSystemAspectGetter;

            treeViewFiles.AddObjects(DriveInfo.GetDrives().Select(d => d.RootDirectory));
        }

        private IEnumerable<FileSystemInfo> GetChildren(FileSystemInfo model)
        {
            if (model is DirectoryInfo d)
            {
                try
                {
                    return d.GetFileSystemInfos()
                        .OrderBy(a => a is DirectoryInfo ? 0 : 1)
                        .ThenBy(b => b.Name);
                }
                catch (SystemException)
                {
                    return Enumerable.Empty<FileSystemInfo>();
                }
            }

            return Enumerable.Empty<FileSystemInfo>();
        }

        private string FileSystemAspectGetter(FileSystemInfo model)
        {
            if (model is DirectoryInfo d)
            {
                return d.Name;
            }
            else if (model is FileInfo f)
            {
                return f.Name;
            }

            return model.ToString();
        }

		private void SetupScrollBar ()
		{
			// When using scroll bar leave the last row of the control free (for over-rendering with scroll bar)
			treeViewFiles.Style.LeaveLastRow = true;

			var _scrollBar = new ScrollBarView (treeViewFiles, true);

			_scrollBar.ChangedPosition += () => {
				treeViewFiles.ScrollOffsetVertical = _scrollBar.Position;
				if (treeViewFiles.ScrollOffsetVertical != _scrollBar.Position) {
					_scrollBar.Position = treeViewFiles.ScrollOffsetVertical;
				}
				treeViewFiles.SetNeedsDisplay ();
			};

			_scrollBar.OtherScrollBarView.ChangedPosition += () => {
				treeViewFiles.ScrollOffsetHorizontal = _scrollBar.OtherScrollBarView.Position;
				if (treeViewFiles.ScrollOffsetHorizontal != _scrollBar.OtherScrollBarView.Position) {
					_scrollBar.OtherScrollBarView.Position = treeViewFiles.ScrollOffsetHorizontal;
				}
				treeViewFiles.SetNeedsDisplay ();
			};

			treeViewFiles.DrawContent += (e) => {
				_scrollBar.Size = treeViewFiles.ContentHeight;
				_scrollBar.Position = treeViewFiles.ScrollOffsetVertical;
				_scrollBar.OtherScrollBarView.Size = treeViewFiles.GetContentWidth (true);
				_scrollBar.OtherScrollBarView.Position = treeViewFiles.ScrollOffsetHorizontal;
				_scrollBar.Refresh ();
			};
		}

		private void RenameNode()
		{
			var node = treeViewFiles.SelectedObject;

			if (node != null) {
				if (GetText ("Text", "Enter text for node:", node.Name, out string entered))
                {
					if (node is FileInfo f)
                    {
                        File.Move(Path.Join(f.DirectoryName, f.Name), Path.Join(f.DirectoryName, entered));
                    }
                    else if (node is DirectoryInfo d)
                    {
                        var parentNode = treeViewFiles.GetParent(node);
                        Directory.Move(d.FullName, Path.Join(d.Parent.FullName, entered));
                    }
				}
			}

            treeViewFiles.RefreshObject(treeViewFiles.GetParent(node));
		}

		private void AddRootNode()
		{
			if (GetText ("Text", "Enter text for node:", "", out string entered))
            {
                try
                {
                    treeViewFiles.AddObject(new DirectoryInfo(entered));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    MessageBox.Query("Add Root Error", e.Message, "Ok");
                }
			}
		}

		private void AddChildDirectoryNode()
		{
			var node = treeViewFiles.SelectedObject;

			if (node != null)
            {
				if (GetText ("Text", "Enter text for directory node:", "", out string entered))
                {
                    try
                    {
					    Directory.CreateDirectory(Path.Join(node.FullName, entered));
					    treeViewFiles.RefreshObject (node);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                        MessageBox.Query("Create Directory Error", e.Message, "Ok");
                    }
			    }
			}
		}

        private void AddChildFileNode()
		{
			var node = treeViewFiles.SelectedObject;

			if (node != null)
            {
				if (GetText ("Text", "Enter text for file node:", "", out string entered))
                {
                    try
                    {
                        var sw = File.CreateText(Path.Join(node.FullName, entered));
                        sw.Close();
					    treeViewFiles.RefreshObject (node);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                        MessageBox.Query("Create File Error", e.Message, "Ok");
                    }
				}
			}
		}

        private void Edit()
        {
            var node = treeViewFiles.SelectedObject;
            if (node is FileInfo f)
            {
                var editorInstance = new Editor(Path.Join(f.DirectoryName, f.Name));
            }
       }

       private void DeleteNode()
       {
            var node = treeViewFiles.SelectedObject;
            var parent = treeViewFiles.GetParent(node);

            if (node is FileInfo f)
            {
                try
                {
                    File.Delete(Path.Join(f.DirectoryName, f.Name));
                    treeViewFiles.Remove(node);
                    treeViewFiles.RefreshObject(parent);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    MessageBox.Query("Delete File Error", e.Message, "Ok");
                }
            }
            else if (node is DirectoryInfo dir)
            {
                try
                {
                    Directory.Delete(dir.FullName, true);
                    treeViewFiles.Remove(node);
                    treeViewFiles.RefreshObject(parent);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    MessageBox.Query("Delete Folder Error", e.Message, "Ok");
                }
            }
        }

        private void CutNode()
        {
             pasteSource = treeViewFiles.SelectedObject;
             cutFlag = true;
        }

        private void CopyNode()
        {
            pasteSource = treeViewFiles.SelectedObject;
            cutFlag = false;
        }

        private void PasteNode()
        {
            if (pasteSource == null)
            {
                MessageBox.Query("Paste Error", "You haven't selected anything", "Ok");
                return;
            }

            var pasteDestination = treeViewFiles.SelectedObject;

            var pasteProgress = new Progress(null)
            {
                source = pasteSource,
                destination = pasteDestination,
                cutF = cutFlag
            };

            Application.Run(pasteProgress);

            if (cutFlag)
            {
                var parent = treeViewFiles.GetParent(pasteSource);
                if (pasteSource is FileInfo f)
                {
                    File.Delete(Path.Join(f.DirectoryName, f.Name));
                }
                else if (pasteSource is DirectoryInfo dir)
                {
                    Directory.Delete(dir.FullName, true);
                }

                treeViewFiles.RefreshObject(parent);
            }

            treeViewFiles.RefreshObject(pasteDestination);
        }

		private bool GetText (string title, string label, string initialText, out string enteredText)
		{
			bool okPressed = false;

			var ok = new Button ("Ok", is_default: true);
			ok.Clicked += () => { okPressed = true; Application.RequestStop (); };
			var cancel = new Button ("Cancel");
			cancel.Clicked += () => { Application.RequestStop (); };
			var d = new Dialog (title, 60, 20, ok, cancel);

			var lbl = new Label () {
				X = 0,
				Y = 1,
				Text = label
			};

			var tf = new TextField () {
				Text = initialText,
				X = 0,
				Y = 2,
				Width = Dim.Fill ()
			};

			d.Add (lbl, tf);
			tf.SetFocus ();

			Application.Run (d);

			enteredText = okPressed ? tf.Text.ToString () : null;
			return okPressed;
		}

        public static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit RangerSharp", "Are you sure that you want to quit this application?", "Yes", "No");
            return n == 0;
        }

        public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
    }
}
