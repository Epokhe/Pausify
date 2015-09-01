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

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessageGetText(IntPtr hWnd, uint msg, UIntPtr wParam, StringBuilder lParam);
        
        
        

        public static uint spotify_pid;
        public static float sessionVolume = 0;
        public static bool changeOrder = false;
        public static float changeValue = 0;

        public static int myctr = 0;
        

        public static void processCurrentPeaks(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue)
        {
            IMMDevice speakers;
            object audioSessionInterface;
            Guid IID_IAudioSessionManager2;
            IMMDeviceEnumerator deviceEnumerator;
            IAudioSessionEnumerator sessionEnumerator;
            IAudioSessionManager2 mgr;

            deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);
            IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out audioSessionInterface);
            mgr = (IAudioSessionManager2)audioSessionInterface;

            myctr++;

            mgr.GetSessionEnumerator(out sessionEnumerator);

            int sessionCount;
            sessionEnumerator.GetCount(out sessionCount);



            var hWnd = FindWindow("SpotifyMainWindow", null);
            if (hWnd == IntPtr.Zero && !PauseControl.spotifyWarningShown)
            {

                //Program.processIcon.showNotification(5000, Constants.appName, "Spotify is not running", ToolTipIcon.None);
                PauseControl.spotifyWarningShown = true;
            }
            else if (!(hWnd == IntPtr.Zero))
            {
                uint pID;
                GetWindowThreadProcessId(hWnd, out pID);
                spotify_pid = pID;
                if (pID == 0 && !PauseControl.unknownErrorShown)
                {
                    //Program.processIcon.showNotification(5000, Constants.appName, "Something bad happened", ToolTipIcon.None);
                    PauseControl.unknownErrorShown = true;
                }


                StringBuilder sb = new StringBuilder(Configuration.MAX_WINDOWNAME_SIZE);
                SendMessageGetText(hWnd, Configuration.WM_GETTEXT, new UIntPtr(Configuration.MAX_WINDOWNAME_SIZE), sb);
                PauseControl.spotifyWindowName = sb.ToString(); //think about a better way

                float maxPeak = 0;


                for (int i = 0; i < sessionCount; i++)
                {
                    IAudioSessionControl audioSessionControl;
                    IAudioSessionControl2 audioSessionControl2;
                    IAudioMeterInformation audioMeterInformation;
                    ISimpleAudioVolume simpleAudioVolume;


                    sessionEnumerator.GetSession(i, out audioSessionControl);

                    audioMeterInformation = audioSessionControl as IAudioMeterInformation;

                    audioSessionControl2 = audioSessionControl as IAudioSessionControl2;

                    simpleAudioVolume = audioSessionControl as ISimpleAudioVolume;

                    float currentPeak;
                    audioMeterInformation.GetPeakValue(out currentPeak);

                    uint sessionProcessId = 0;
                    audioSessionControl2.GetProcessId(out sessionProcessId);



                    /*
                    finally figured out how to solve a very frustrating problem. Basically spotify has multiple
                    sessions open for reasons I don't know, which makes volumemixer(and me indirectly) go crazy,
                    eg. it shows spotify volume at lowest but spotify still plays sound, but if you manually click
                    to lowest level, spotify gets muted. Apparently, the visual indicator in volume mixer gets 
                    its value from *one* of spotify sessions, but if you control the indicator yourself, you
                    set all spotify sessions to the same value.
                    */
                    if (sessionProcessId == pID) //Spotify Session
                    {
                        QueueControl.dequeue(ref spotifySoundQueue); //Remove the oldest peak from queue
                        QueueControl.enqueue(ref spotifySoundQueue, ref currentPeak); //Add current sound level to queue


                        simpleAudioVolume.GetMasterVolume(out sessionVolume);

                        if (sessionVolume != 0f && AdControl.sound_muted)
                        {
                            //this is not handled properly now
                            //AdControl.handleUserUnmute();
                        }

                        //Ads part
                        if (AdControl.ad_alarm && !AdControl.sound_muted)
                        {
                            Guid guid = Guid.Empty;
                            sessionVolume = 0f;
                            simpleAudioVolume.SetMasterVolume(0f, ref guid);
                        }
                        else if (!AdControl.ad_alarm && AdControl.sound_muted)
                        {
                            Guid guid = Guid.Empty;
                            if (sessionVolume == 0f && Configuration.option_remember)
                            {
                                sessionVolume = 1f;
                            }
                            if (!Configuration.option_remember)
                            {
                                sessionVolume = Configuration.spotify_volume / 100;
                            }
                            simpleAudioVolume.SetMasterVolume(sessionVolume, ref guid);
                            
                        }

                    }
                    else if (sessionProcessId != 0)
                    {
                        if (maxPeak < currentPeak)
                        {
                            maxPeak = currentPeak;
                        }
                    }

                    Marshal.ReleaseComObject(audioSessionControl);
                    Marshal.ReleaseComObject(audioSessionControl2);
                    Marshal.ReleaseComObject(audioMeterInformation);
                }


                if (AdControl.ad_alarm && !AdControl.sound_muted)
                {
                    Guid guid = Guid.Empty;
                    sessionVolume = 0f;
                    AdControl.sound_muted = true;
                }
                else if (!AdControl.ad_alarm && AdControl.sound_muted)
                {
                    Guid guid = Guid.Empty;
                    if (sessionVolume == 0f && Configuration.option_remember)
                    {
                        sessionVolume = 1f;
                    }
                    if (!Configuration.option_remember)
                    {
                        sessionVolume = Configuration.spotify_volume / 100;
                    }
                    AdControl.sound_muted = false;
                }

                

                if (maxPeak > Configuration.NORMALIZE_VALUE) { maxPeak = Configuration.NORMALIZE_VALUE; } //Normalization
                QueueControl.dequeue(ref otherSoundQueue); //Remove the oldest peak from queue
                QueueControl.enqueue(ref otherSoundQueue, ref maxPeak); //Add current sound level to queue
            }

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(mgr);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
        }


        //not needed currently
        public static void changeSpotifyVolume(float value)
        {

            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());

            IMMDevice speakers;
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;

            IAudioSessionEnumerator sessionEnumerator;

            object audioSessionInterface;
            speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out audioSessionInterface);


            IAudioSessionManager2 mgr = (IAudioSessionManager2)audioSessionInterface;

            myctr++;

            mgr.GetSessionEnumerator(out sessionEnumerator);
            

            int sessionCount;
            sessionEnumerator.GetCount(out sessionCount);

            var hWnd = FindWindow("SpotifyMainWindow", null);
            if (hWnd == IntPtr.Zero && !PauseControl.spotifyWarningShown)
            {
                //Program.processIcon.showNotification(5000, Constants.appName, "Spotify is not running", ToolTipIcon.None);
                PauseControl.spotifyWarningShown = true;
            }
            else if (!(hWnd == IntPtr.Zero))
            {
                uint pID;
                GetWindowThreadProcessId(hWnd, out pID);
                spotify_pid = pID;
                if (pID == 0 && !PauseControl.unknownErrorShown)
                {
                    //Program.processIcon.showNotification(5000, Constants.appName, "Something bad happened", ToolTipIcon.None);
                    PauseControl.unknownErrorShown = true;
                }

                for (int i = 0; i < sessionCount; i++)
                {
                    IAudioSessionControl audioSessionControl;
                    IAudioSessionControl2 audioSessionControl2;
                    ISimpleAudioVolume simpleAudioVolume;

                    sessionEnumerator.GetSession(i, out audioSessionControl);

                    audioSessionControl2 = audioSessionControl as IAudioSessionControl2;

                    simpleAudioVolume = audioSessionControl as ISimpleAudioVolume;

                    uint sessionProcessId = 0;
                    audioSessionControl2.GetProcessId(out sessionProcessId);

                    if (sessionProcessId == pID) //Spotify Session
                    {
                        Guid guid = Guid.Empty;
                        simpleAudioVolume.SetMasterVolume(value, ref guid);
                    }

                    Marshal.ReleaseComObject(audioSessionControl);
                    Marshal.ReleaseComObject(audioSessionControl2);
                }
            }

            Marshal.ReleaseComObject(sessionEnumerator);
            Marshal.ReleaseComObject(mgr);
            Marshal.ReleaseComObject(speakers);
            Marshal.ReleaseComObject(deviceEnumerator);
        }



    }

}
