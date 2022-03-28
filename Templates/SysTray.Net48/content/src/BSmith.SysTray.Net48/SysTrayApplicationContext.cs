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

namespace BSmith.SysTray.Net48
{
	public class SysTrayApplicationContext : ApplicationContext
	{
		private NotifyIcon trayIcon;
		private ContextMenuStrip contextMenu;
		private ToolStripMenuItem exitMenuItem;
		private IContainer components;


		public SysTrayApplicationContext()
		{
			components = new Container();
			contextMenu = new ContextMenuStrip();
			exitMenuItem = new ToolStripMenuItem();

			contextMenu.Items.AddRange(new ToolStripMenuItem[] { exitMenuItem });

			exitMenuItem.Text = "E&xit";
			exitMenuItem.Click += new EventHandler(Exit);

			trayIcon = new NotifyIcon(components);
			trayIcon.Icon = new Icon("images/test.ico");
			trayIcon.ContextMenuStrip = contextMenu;
			trayIcon.Visible = true;
		}

		public void Exit(object sender, EventArgs e)
    	{
    	    // Hide tray icon, otherwise it will remain shown until user mouses over it
    	    trayIcon.Visible = false;

    	    Application.Exit();
    	}
	}
}