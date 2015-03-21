using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SpotifyLib
{
    public class SpotifyController
    {
        // Credit goes to http://code.google.com/p/spotifycontrol/source/browse/trunk/SpotifyControl/Controllers/ControllerSpotify.vb 
        // for some of the key message commands, etc

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
           int dwExtraInfo);

        const int KEY_MESSAGE = 0x319;
        const int CONTROL_KEY = 0x11;

        const long PLAYPAUSE_KEY = 0xE0000L;
        const long NEXTTRACK_KEY = 0xB0000L;
        const long PREVIOUS_KEY = 0xC0000L;

        public static String getSpotifyWindowTitle()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            if (spotifyWindow == new IntPtr(0))
            {
                return "";
            }
            // Allocate correct string length first
            int length = GetWindowTextLength(spotifyWindow);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(spotifyWindow, sb, sb.Capacity);
            return sb.ToString();
        }

        static public bool isSpotifyOpen(bool open)
        {
            if (getSpotifyWindowTitle() == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static public String getSongName()
        {
            // Use character '–' to split song name/artist name
            String[] title = getSpotifyWindowTitle().Split('–');
            if (title.Count() > 1)
            {
                return title[1].Trim();
            }
            else
            {
                return "";
            }
        }

        static public String getArtistName()
        {
            // Use character '–' to split song name/artist name
            String[] title = getSpotifyWindowTitle().Split('–');
            if (title.Count() > 1)
            {
                return title[0].Split('-')[1].Trim();
            }
            else
            {
                return "";
            }
        }

        static public void pausePlay()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            PostMessage(spotifyWindow, KEY_MESSAGE, IntPtr.Zero, new IntPtr(PLAYPAUSE_KEY));
        }


        static public void nextTrack()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            PostMessage(spotifyWindow, KEY_MESSAGE, IntPtr.Zero, new IntPtr(NEXTTRACK_KEY));
        }
        static public void previousTrack()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            PostMessage(spotifyWindow, KEY_MESSAGE, IntPtr.Zero, new IntPtr(PREVIOUS_KEY));
        }
        static public void volumeUp()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            keybd_event(CONTROL_KEY, 0x1D, 0, 0);
            PostMessage(spotifyWindow, 0x100, new IntPtr(0x26), IntPtr.Zero);
            System.Threading.Thread.Sleep(100);
            keybd_event(CONTROL_KEY, 0x1D, 0x2, 0);
        }
        static public void volumeDown()
        {
            IntPtr spotifyWindow = FindWindow("SpotifyMainWindow", null);
            keybd_event(CONTROL_KEY, 0x1D, 0, 0);
            PostMessage(spotifyWindow, 0x100, new IntPtr(0x28), IntPtr.Zero);
            System.Threading.Thread.Sleep(100);
            keybd_event(CONTROL_KEY, 0x1D, 0x2, 0);
        }
    }
}
