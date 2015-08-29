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
        ToolStripMenuItem exitItem, startupItem, settingsItem, adItem;
        bool startupEnabled;

        public ContextMenuStrip Create()
        {
            menu = new ContextMenuStrip();


            adItem = new ToolStripMenuItem();
            adItem.Text = "Mark as ad";
            adItem.Click += new EventHandler(Ad_Click);
            menu.Items.Add(adItem);


            checkStartup();

            startupItem = new ToolStripMenuItem();
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



            settingsItem = new ToolStripMenuItem();
            settingsItem.Text = "Settings";
            settingsItem.Click += new EventHandler(Settings_Click);
            menu.Items.Add(settingsItem);


            exitItem = new ToolStripMenuItem();
            exitItem.Text = "Exit";
            exitItem.Click += new System.EventHandler(Exit_Click);
            //item.Image = Resources.Exit;
            menu.Items.Add(exitItem);




            

            return menu;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Program.processIcon.Dispose();
            Application.Exit();
        }


        void Settings_Click(object sender, EventArgs e){

            if (!Program.settingsOpen)
            {
                Program.settingsForm = new SettingsForm();
                Program.settingsForm.Show();
                Program.settingsOpen = true;
            }
            else
            {
                Program.settingsForm.BringToFront();
            }
            
        }


        void Ad_Click(object sender, EventArgs e)
        {
            FileManager.addAd(PauseControl.spotifyWindowName);
            if (Program.settingsOpen)
            {
                Program.settingsForm.refreshForm();
            }
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
            
            if(rk.GetValue(Configuration.appName) == null)
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
            rk.SetValue(Configuration.appName, Application.ExecutablePath.ToString());
        }

        private void deleteStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.DeleteValue(Configuration.appName, false);
        }

        

        
    }
}