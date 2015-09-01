using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Pausify
{
    public partial class SettingsForm : Form
    {

        //KeyboardHook hook = new KeyboardHook();


        public SettingsForm()
        {
            InitializeComponent();


            putDataIntoList();

            adCheckBox.Checked = Configuration.option_adblock;
            pauseCheckBox.Checked = Configuration.option_autopause;
            rememberCheckBox.Checked = Configuration.option_remember;
            trackBar.Value = (int)Configuration.spotify_volume;

            if (Configuration.option_adblock)
            {
                toggleAdOptions(true);
            }
            else
            {
                toggleAdOptions(false);
            }

            // register the event that is fired after the key press.
            //hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            //var ModifierKeys = new ModifierKeys();
            //hook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F12);
            //this.adListBox.Items.AddRange(tmp);


            this.FormClosed += SettingsForm_FormClosed;
        }

        public void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            adLabel.Text = e.Modifier.ToString() + " + " + e.Key.ToString();
        }

        //might add more here idk
        public void refreshForm()
        {
            putDataIntoList();
            adCheckBox.Checked = Configuration.option_adblock;
            pauseCheckBox.Checked = Configuration.option_autopause;
        }

        private void putDataIntoList()
        {
            string[] tmp = new string[FileManager.adSet.Count];
            FileManager.adSet.CopyTo(tmp);
            adListBox.DataSource = null;
            adListBox.DataSource = tmp;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            adListBox.DataSource = null;
            Program.settingsOpen = false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void toggleAdOptions(bool isenabled)
        {
            volLabel.Enabled = isenabled;
            adLabel.Enabled = isenabled;
            rememberCheckBox.Enabled = isenabled;
            adListBox.Enabled = isenabled;
            deleteButton.Enabled = isenabled;
            trackBar.Enabled = isenabled;
        }

        private void adCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (adCheckBox.Checked)
            {
                //Start ad controller
                Configuration.option_adblock = true;
                FileManager.changeConfig("adblock", "1");
                toggleAdOptions(true);
            }
            else
            {
                //Stop ad controller
                AdControl.disable();

                //disable all related option controllers
                toggleAdOptions(false);
            }
        }

        private void pauseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pauseCheckBox.Checked)
            {
                //Start auto pauser
                //create a option->their string or otherwise map
                Configuration.option_autopause = true;
                FileManager.changeConfig("autopause", "1");
                PauseControl.activate();
            }
            else
            {
                //Stop auto pauser
                Configuration.option_autopause = false;
                FileManager.changeConfig("autopause", "0");
            }
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            int tmp = (int)Configuration.spotify_volume;
            FileManager.changeConfig("spotifyvalue", tmp.ToString());
            this.Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string[] chosenOnes = adListBox.SelectedItems.Cast<string>().ToArray();
            FileManager.removeAds(chosenOnes);
            putDataIntoList();
        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            FileManager.createDefaultFiles();
            FileManager.refresh();
            Program.settingsForm.refreshForm();
        }

        private void rememberCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            /*if (rememberCheckBox.Checked)
            {
                Configuration.option_remember = true;
                FileManager.changeConfig("remember", "1");
                trackBar.Enabled = false;
            }
            else
            {
                Configuration.option_remember = false;
                FileManager.changeConfig("remember", "0");
                trackBar.Enabled = true;
            }*/
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            Configuration.spotify_volume = trackBar.Value;
            toolTip1.SetToolTip(trackBar, trackBar.Value.ToString());
        }
    }
}
