using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;
using Terminal.Gui.Trees;

namespace Copy.TUI
{
    public class FileSystemTree : Window
    {
        public Action OnExit { get; set; }
        public Action<(string path, FileSystemInfo info)> PathAction { get; set; }
        public bool source = false;
        public bool destination = false;
        private readonly View _parent;
        private TreeView<FileSystemInfo> treeViewFiles;

        public FileSystemTree(View parent)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }

        private void InitStyle()
        {
            X = 0;
            Y = 1;
            Width = Dim.Fill();
            Height = Dim.Fill(1);

            if (source)
            {
                Title = "Select The Source File or Directory";
            }
            else if (destination)
            {
                Title = "Select the Destination Directory";
            }
        }

        private void InitControls()
        {
            KeyPress += (a) =>
            {
                if (a.KeyEvent.Key == Key.Esc)
                {
                    Close();
                }
            };

            var filesLabel = new Label("File Tree:")
            {
                X = 0,
                Y = 1
            };

            Add(filesLabel);

            treeViewFiles = new TreeView<FileSystemInfo>()
            {
                X = 0,
                Y = Pos.Bottom(filesLabel),
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            treeViewFiles.ObjectActivated += TreeViewFiles_ObjectActivated;

            SetupFileTree();

            Add(treeViewFiles);

            SetupScrollBar();

            var statusBar = new StatusBar (new StatusItem [] {
				new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => Close()),
				new StatusItem(Key.CtrlMask | Key.D, "~^D~ Add Directory", () => AddChildDirectoryNode()),
                new StatusItem(Key.CtrlMask | Key.F, "~^F~ Add File", () => AddChildFileNode()),
				new StatusItem(Key.CtrlMask | Key.T, "~^T~ Add Root", () => AddRootNode()),
				new StatusItem(Key.CtrlMask | Key.R, "~^R~ Rename Node", () => RenameNode()),
			});

            Add(statusBar);
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

				var n = MessageBox.Query ($"Select { f.Name }?", sb.ToString (), "Yes", "No");

                if (n == 0)
                {
                    NodeSelected(f);
                }
			}

			if (obj.ActivatedObject is DirectoryInfo dir)
            {
				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				sb.AppendLine ($"Path:{dir.Parent?.FullName}");
				sb.AppendLine ($"Modified:{ dir.LastWriteTime}");
				sb.AppendLine ($"Created:{ dir.CreationTime}");

				var n = MessageBox.Query($"Select { dir.Name }?", sb.ToString (), "Yes", "No");

                if (n == 0)
                {
                    NodeSelected(dir);
                }
			}
        }

        private void NodeSelected(FileSystemInfo obj)
        {
            if (obj is FileInfo f && destination == false)
            {
                PathAction?.Invoke((path: GeneratePathName(f.DirectoryName, f.Name), info: obj));
                Close();
            }
            else if (obj is FileInfo fi && destination == true)
            {
                MessageBox.Query("Destination Selection Error", "The destination must be a directory", "Ok");
            }
            else if (obj is DirectoryInfo dir)
            {
                PathAction?.Invoke((path: GeneratePathName(dir.Parent?.FullName, dir.Name), info: obj));
                Close();
            }
        }

        private string GeneratePathName(string root, string objName)
        {
            if (IsWindows())
            {
                if (String.IsNullOrWhiteSpace(root))
                {
                    return objName;
                }
                else
                {
                    return $"{ root }\\{ objName }";
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(root))
                {
                    return objName;
                }
                else
                {
                    return $"{ root }/{ objName }";
                }
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

		private void AddRootNode ()
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

		private void AddChildDirectoryNode ()
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
                    };
				}
			}
		}

        private void AddChildFileNode ()
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


        public void Close()
        {
            Application.RequestStop();
            _parent?.Remove(this);
        }

        public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
    }
}