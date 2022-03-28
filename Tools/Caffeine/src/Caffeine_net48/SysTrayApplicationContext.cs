using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Caffeine.Model;

namespace Caffeine
{
	public class SysTrayApplicationContext : ApplicationContext
	{
		private NotifyIcon trayIcon;
		private ContextMenuStrip contextMenu;
		private ToolStripMenuItem enableMenuItem;
		private ToolStripMenuItem disableMenuItem;
		private ToolStripMenuItem exitMenuItem;
		private IContainer components;
		public static CaffeineInterface runtime;
		public static System.Timers.Timer backgroundTimer;
		//public Thread backgroundThread;


		public SysTrayApplicationContext()
		{
			components = new Container();
			contextMenu = new ContextMenuStrip();
			enableMenuItem = new ToolStripMenuItem();
			disableMenuItem = new ToolStripMenuItem();
			exitMenuItem = new ToolStripMenuItem();

			contextMenu.Items.AddRange(new ToolStripMenuItem[] { enableMenuItem, disableMenuItem,exitMenuItem });

			enableMenuItem.Text = "Enable";
			enableMenuItem.Click += new EventHandler(Enable);

			disableMenuItem.Text = "Disable";
			disableMenuItem.Click += new EventHandler(Disable);

			exitMenuItem.Text = "E&xit";
			exitMenuItem.Click += new EventHandler(Exit);

			trayIcon = new NotifyIcon(components);
			trayIcon.Icon = new Icon($"{ Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) }/images/caffeine.ico");
			trayIcon.ContextMenuStrip = contextMenu;
			trayIcon.Visible = true;

			
			backgroundTimer = new System.Timers.Timer();
			backgroundTimer.Interval = 1000 * 60 * 5; //5 minutes = 1000ms * 60s * 5m
			backgroundTimer.Elapsed += TriggerTimeOut;

			if (Program.cmdFlags.disableFlag)
			{
				runtime = new DisableInterface();
				disableMenuItem.Checked = true;
				backgroundTimer.AutoReset = false;
				backgroundTimer.Enabled = false;
			}
			else
			{
				runtime = new EnableInterface();
				enableMenuItem.Checked = true;
				backgroundTimer.AutoReset = true;
				backgroundTimer.Enabled = true;
			}
		}

		public void Enable(object sender, EventArgs e)
		{
			enableMenuItem.Checked = true;
			disableMenuItem.Checked = false;

			runtime = new EnableInterface();

			backgroundTimer.AutoReset = true;
			backgroundTimer.Enabled = true;
		}

		public void Disable(object sender, EventArgs e)
		{
			enableMenuItem.Checked = false;
			disableMenuItem.Checked = true;

			runtime = new DisableInterface();

			backgroundTimer.AutoReset = false;
			backgroundTimer.Enabled = false;
		}

		public void Exit(object sender, EventArgs e)
    	{
    	    // Hide tray icon, otherwise it will remain shown until user mouses over it
    	    trayIcon.Visible = false;

    	    Application.Exit();
    	}

		public static void TriggerTimeOut(Object source, System.Timers.ElapsedEventArgs e)
		{
			runtime.ExecuteAsync();
		}
	}
}