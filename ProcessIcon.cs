using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;
using SpotifyLib;
using System.Collections;
using System.Diagnostics;
using WindowsAudio;
using System.Windows.Forms;
using Pausify.Properties;

namespace Pausify
{
 
    class ProcessIcon 
    {
        NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        /// Displays the icon in the system tray.
        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.
            ni.MouseDoubleClick += new MouseEventHandler(ni_MouseDoubleClick);
            setPlayingIcon();
            ni.Text = Constants.appName;
            ni.Visible = true;
            showNotification(5000, Constants.appName, "You can double click on my icon to deactivate me, and right click to close.", ToolTipIcon.None);
            
            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }

        /// Handles the MouseDoubleClick event of the ni control. 
        void ni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MainControl.userDeactivated) 
                {
                    setPlayingIcon();
                    MainControl.activate(); 
                }
                else 
                {
                    setInactiveIcon();
                    MainControl.deactivate(); 
                }
                
            }
        }

        public void setPlayingIcon()
        {
            ni.Icon = Resources.Spotify_green;
        }
        public void setPausedIcon()
        {
            ni.Icon = Resources.Spotify_red;
        }
        public void setInactiveIcon()
        {
            ni.Icon = Resources.Spotify_gray;
        }

        public void showNotification(int delay, string title, string text, ToolTipIcon type)
        {
            ni.ShowBalloonTip(delay, title, text, type);
        }
    }
}