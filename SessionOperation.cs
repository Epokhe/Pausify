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

//THANKS STACKOVERFLOW

namespace Pausify
{
    //Peak meters are enqueued here. This part seems really bad, a lot of reinstantiation, and they happen in every tick. I think that causes delay???
    //I think this part took 10 ms or something, IIRC after adding queue nothing changed, all operations in one tick still takes 10 ms, only because of this part.
    class SessionOperation
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static void processCurrentPeaks(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue)
        {
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());

            IMMDevice speakers;
            object audioSessionInterface;

            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out audioSessionInterface);

            IAudioSessionManager2 mgr = (IAudioSessionManager2)audioSessionInterface;

            IAudioSessionEnumerator sessionEnumerator;
            mgr.GetSessionEnumerator(out sessionEnumerator);

            int sessionCount;
            sessionEnumerator.GetCount(out sessionCount);

            var hWnd = FindWindow("SpotifyMainWindow", null);
            if (hWnd == IntPtr.Zero && !MainControl.spotifyWarningShown)
            {
                Program.processIcon.showNotification(5000, Constants.appName, "Spotify is not running", ToolTipIcon.None);
                MainControl.spotifyWarningShown = true;
                return;
            }

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);
            if (pID == 0 && !MainControl.unknownErrorShown)
            {
                //Program.processIcon.showNotification(5000, Constants.appName, "Something bad happened", ToolTipIcon.None);
                MainControl.unknownErrorShown = true;
                return;
            }

            float maxPeak = 0;

            for (int i = 0; i < sessionCount; i++)
            {
                IAudioSessionControl audioSessionControl;
                IAudioSessionControl2 audioSessionControl2;
                IAudioMeterInformation audioMeterInformation;

                sessionEnumerator.GetSession(i, out audioSessionControl);

                audioMeterInformation = audioSessionControl as IAudioMeterInformation;

                audioSessionControl2 = audioSessionControl as IAudioSessionControl2;

                float currentPeak;
                audioMeterInformation.GetPeakValue(out currentPeak);

                uint sessionProcessId = 0;
                audioSessionControl2.GetProcessId(out sessionProcessId);

                if (sessionProcessId == pID) //Spotify Session
                {
                    QueueControl.dequeue(ref spotifySoundQueue); //Remove the oldest peak from queue
                    QueueControl.enqueue(ref spotifySoundQueue, ref currentPeak); //Add current sound level to queue
                }
                else if(sessionProcessId != 0)
                {
                    maxPeak = (maxPeak > currentPeak) ? maxPeak : currentPeak;
                }

                Marshal.ReleaseComObject(audioSessionControl);
                Marshal.ReleaseComObject(audioSessionControl2);
                Marshal.ReleaseComObject(audioMeterInformation);
            }


            if (maxPeak > Constants.NORMALIZE_VALUE) { maxPeak = Constants.NORMALIZE_VALUE; } //Normalization
            QueueControl.dequeue(ref otherSoundQueue); //Remove the oldest peak from queue
            QueueControl.enqueue(ref otherSoundQueue, ref maxPeak); //Add current sound level to queue

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(mgr);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
        }

    }

}
