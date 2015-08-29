using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pausify
{
    class AdControl
    {

        public static bool ad_alarm;
        public static bool sound_muted;

        public static void decide()
        {
            if (Configuration.option_adblock)
            {
                if (PauseControl.spotifyWindowName != null && !ad_alarm && FileManager.adSet.Contains(PauseControl.spotifyWindowName))
                {
                    ad_alarm = true;
                }
                else if (PauseControl.spotifyWindowName != null && ad_alarm && !FileManager.adSet.Contains(PauseControl.spotifyWindowName))
                {
                    ad_alarm = false;
                }
            }
        }

        public static void handleUserUnmute()
        {
            AdControl.sound_muted = false;
            ad_alarm = false;
            Configuration.option_adblock = false;
            FileManager.changeConfig("adblock", "0");
            Program.processIcon.showNotification(5000, Configuration.appName, "It seems that you unmuted spotify from Volume Mixer, so the adblock system is disabled until you enable it again.", System.Windows.Forms.ToolTipIcon.None);
        }

        public static void disable()
        {
            Configuration.option_adblock = false;
            ad_alarm = false;
            FileManager.changeConfig("adblock", "0");

            if (SessionOperation.sessionVolume == 0f && Configuration.option_remember)
            {
                SessionOperation.sessionVolume = 1f;
            }
            if (!Configuration.option_remember)
            {
                SessionOperation.sessionVolume = Configuration.spotify_volume / 100;
            }
            SessionOperation.changeSpotifyVolume(SessionOperation.sessionVolume);
        }
    }
}
