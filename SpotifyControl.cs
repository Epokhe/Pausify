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


namespace Pausify
{
    //Actually both of these functions send the same command to spotify, because there is no distinct commands for pausing and playing Spotify AFAIK
    //So you need to get the sound levels in order to know if spotify is paused or not(considering the user can pause spotify too)
    class SpotifyControl
    {
        public static void playSpotify(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize, ref int ticksInactive, ref int tickDelay)
        {
            SpotifyController.pausePlay();

            currentQueueSize = Configuration.SHORT_QUEUE_SIZE;

            QueueControl.fillQueuesAfterPlay(ref otherSoundQueue, ref spotifySoundQueue, ref spotifyQueueSize, ref currentQueueSize);

            tickDelay = 2;
            ticksInactive = Configuration.TICKS_BEFORE_START;
        }

        public static void pauseSpotify(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize, ref int ticksInactive, ref int tickDelay)
        {
            SpotifyController.pausePlay();

            QueueControl.fillQueuesAfterPause(ref otherSoundQueue, ref spotifySoundQueue, ref spotifyQueueSize, ref currentQueueSize);

            tickDelay = 2;
            ticksInactive = Configuration.TICKS_BEFORE_START;
        }
    }
}
