using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Windows.Forms;
using Pausify.Properties;
using System.Drawing;
using Microsoft.Win32;

namespace Pausify
{
    class ContextMenus
    {
        ContextMenuStrip menu;
        ToolStripMenuItem closeItem;
        ToolStripMenuItem startupItem;
        bool startupEnabled;

        public ContextMenuStrip Create()
        {
            menu = new ContextMenuStrip();
            checkStartup();

            startupItem = new ToolStripMenuItem();
            startupItem.Name = "Startup";
            startupItem.Text = "Open at startup";
            startupItem.Click += new EventHandler(Startup_Click);
            if (startupEnabled)
            {
                startupItem.Checked = true;
            }
            else
            {
                startupItem.Checked = false;
            }
            menu.Items.Add(startupItem);


            closeItem = new ToolStripMenuItem();
            closeItem.Text = "Close";
            closeItem.Click += new System.EventHandler(Exit_Click);
            //item.Image = Resources.Exit;
            menu.Items.Add(closeItem);

            

            return menu;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Program.processIcon.Dispose();
            Application.Exit();
        }

        void Startup_Click(object sender, EventArgs e)
        {
            if (startupEnabled)
            {
                deleteStartup();
                startupEnabled = false;
                startupItem.Checked = false;
            }
            else
            {
                setStartup();
                startupEnabled = true;
                startupItem.Checked = true;
            }
        }

        private void checkStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            
            if(rk.GetValue(Constants.appName) == null)
            {
                startupEnabled = false;
            }
            else
            {
                startupEnabled = true;
            }
        }

        private void setStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue(Constants.appName, Application.ExecutablePath.ToString());
        }

        private void deleteStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue(Constants.appName, false);
        }

        

        
    }
}