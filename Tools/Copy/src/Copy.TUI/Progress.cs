using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Terminal.Gui;
using Terminal.Gui.Trees;
using Copy.Class;

namespace Copy.TUI
{
    public class Progress : Window
    {
        public Action OnExit { get; set; }
        public FileSystemInfo sourInfo { get; set; }
        public string sourPath { get; set; }
        public FileSystemInfo destInfo { get; set; }
        public string destPath { get; set; }
        private readonly View _parent;
        private ProgressBar progressBar { get; set; }
        private ListView listView;
        private List<string> progressList = new();

        public Progress(View parent)
        {
            _parent = parent;
            InitStyle();
            InitControls();
        }

        private void InitStyle()
        {
            X = 0;
            Y = 1;
            Width = Dim.Fill();
            Height = Dim.Fill();

            Title = "Copying in Progress";
        }

        private void InitControls()
        {
            progressBar = new ProgressBar()
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = 1,
                Fraction = 0F
            };

            Add(progressBar);

            listView = new ListView(progressList)
            {
                X = 0,
                Y = Pos.Top(progressBar) + 2,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Add(listView);

            var scrollBar = new ScrollBarView (listView, true);

			scrollBar.ChangedPosition += () => {
				listView.TopItem = scrollBar.Position;
				if (listView.TopItem != scrollBar.Position) {
					scrollBar.Position = listView.TopItem;
				}
				listView.SetNeedsDisplay ();
			};

			scrollBar.OtherScrollBarView.ChangedPosition += () => {
				listView.LeftItem = scrollBar.OtherScrollBarView.Position;
				if (listView.LeftItem != scrollBar.OtherScrollBarView.Position) {
					scrollBar.OtherScrollBarView.Position = listView.LeftItem;
				}
				listView.SetNeedsDisplay ();
			};

			listView.DrawContent += (e) => {
				scrollBar.Size = listView.Source.Count - 1;
				scrollBar.Position = listView.TopItem;
				scrollBar.OtherScrollBarView.Size = listView.Maxlength - 1;
				scrollBar.OtherScrollBarView.Position = listView.LeftItem;
				scrollBar.Refresh ();
			};

           Application.MainLoop?.Invoke(() => 
            {
                StartCopy();
                progressBar.Fraction = 1F;
                MessageBox.Query("Copy Complete", "The copy is complete", "Ok");
                Close();
            });
        }

        public void StartCopy()
        {
            if (File.Exists(sourPath))
            {
                try
                {
                    Class1.CopyFile(sourPath, destPath);
                }
                catch (Exception e)
                {
                    MessageBox.Query("Error Encountered", e.Message, "Ok");
                }
            }
            else if (Directory.Exists(sourPath))
            {
                try
                {
                    List<string> errors = Class1.CopyDirectory(sourPath, destPath, LogProgress);

                    StringBuilder errorMessage = new StringBuilder();

                    foreach(string error in errors)
                    {
                        errorMessage.AppendLine(error);
                    }

                    if (errors.Count > 0)
                    {
                        MessageBox.Query("Errors Encountered", $"These weren't copied due to errors\n{ errorMessage.ToString() }", "Ok");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Query("Error Encountered", e.Message, "Ok");
                }
            }
       }

        public void LogProgress(string msg, int total, int num_completed)
	    {
	    	progressList.Add(msg);
	    	listView.MoveDown();
            progressBar.Fraction = (float) num_completed / total;
	    }

        public void Close()
        {
            Application.RequestStop();
            _parent?.Remove(this);
        }
    }
}