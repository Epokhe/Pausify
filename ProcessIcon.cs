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
            ni.Text = Configuration.appName;
            ni.Visible = true;
            if (Configuration.firsttime)
            {
                showNotification(5000, Configuration.appName, "You can double click on my icon to deactivate me, and right click to change my options or close me.", ToolTipIcon.None);
            }
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
                if (PauseControl.userDeactivated) 
                {
                    setPlayingIcon();
                    PauseControl.activate(); 
                }
                else 
                {
                    setInactiveIcon();
                    PauseControl.deactivate(); 
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