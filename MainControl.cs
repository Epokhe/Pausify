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

namespace Pausify
{
    class MainControl
    {
        
        //Queues
        //Spotify sound queue just holds peak levels of the last SPOTIFY_QUEUE_SIZE / TICKS_IN_SEC seconds.
        public static Queue<float> spotifySoundQueue = new Queue<float>();

        /*Other sound queue holds normalized values of peak levels of ... seconds. Bigger peak values than NORMALIZE_VALUE are dropped down to its value.
        I think this is the logical approach because if there is sound, its level doesn't matter, but I didn't use this on spotify queue size, it doesn't need it.*/
        public static Queue<float> otherSoundQueue = new Queue<float>();


        //Variables
        //Program starts to determine its state after ticksInactive reaches 0
        public static int ticksInactive;

        //Program starts to get peak levels after tickDelay reaches 0
        public static int tickDelay; 

        /*Other(Background) queue uses two different sizes, small one for fast reaction to background sound, 
        long one for a tolerance to brief silence of background sound, since videos can have silent moments*/
        public static int currentQueueSize; 

        //Holds what playPause() function did in last case
        public static int lastPress;

        //1 if user pressed spotify pause button
        public static int programStatus;

        //Transition state indicator
        public static bool transition;

        //State value
        public static int currentState;

        //True if user deactivated Pausify
        public static bool userDeactivated;

        public static bool spotifyWarningShown;
        public static bool unknownErrorShown;

        
        public enum SoundState
        {
            None,
            Spotify,
            Other,
            Both
        }

        public static void processTicks()
        {
            if (!userDeactivated)
            {
                if (ticksInactive == -1) //Logical part starts to run here
                {
                    checkStates();
                    decide();
                }
                else if (ticksInactive == 0)
                {
                    //Queues are not full, so this fills the queues with their average values
                    QueueControl.fillQueuesWithAverage(ref otherSoundQueue, ref spotifySoundQueue, ref Constants.SPOTIFY_QUEUE_SIZE, ref currentQueueSize);
                    ticksInactive--;
                }
                else if (ticksInactive > 0)
                {
                    ticksInactive--;
                }

                //2 ticks after pause/play, because transition is not instant which causes noise
                if (tickDelay == 0)
                {
                    SessionOperation.processCurrentPeaks(ref otherSoundQueue, ref spotifySoundQueue);
                }
                else
                {
                    tickDelay--;
                }
            }
        }

        private static void checkStates()
        {
            //Find Spotify Sound Average
            float spotifySoundAverage = 0;
            foreach (float spotifyLevel in spotifySoundQueue)
            {
                spotifySoundAverage += spotifyLevel;
            }
            spotifySoundAverage /= Constants.SPOTIFY_QUEUE_SIZE;

            //Find Other Sound Average
            float otherSoundAverage = 0;
            foreach (float otherLevel in otherSoundQueue)
            {
                otherSoundAverage += otherLevel; 
            }
            otherSoundAverage /= currentQueueSize;

            //Determine the state
            currentState = 0;

            if ((otherSoundAverage > Constants.SOUND_LOW_LIMIT && otherSoundAverage < Constants.OTHER_SOUND_HIGH_LIMIT) || 
                (spotifySoundAverage > Constants.SOUND_LOW_LIMIT && spotifySoundAverage < Constants.SPOTIFY_SOUND_HIGH_LIMIT))
            {
                transition = true;
            }
            else
            {
                transition = false;
                currentState += (spotifySoundAverage > Constants.SPOTIFY_SOUND_HIGH_LIMIT) ? Constants.SPOTIFY_MUSIC_PLAYING : 0;
                currentState += (otherSoundAverage > Constants.OTHER_SOUND_HIGH_LIMIT) ? Constants.OTHER_SOUND_PLAYING : 0;
            }
        }

        private static void decide()
        {
            if (!transition)
            {
                switch ((SoundState)currentState)
                {
                    case SoundState.None: //No sound
                        if (lastPress == 0) //Last pausePlay() paused Spotify
                        {
                            lastPress = 1;
                            Program.processIcon.setPlayingIcon();
                            SpotifyControl.playSpotify(ref otherSoundQueue, ref spotifySoundQueue, ref Constants.SPOTIFY_QUEUE_SIZE, ref currentQueueSize, ref ticksInactive, ref tickDelay);
                        }
                        else if (lastPress == 1 && programStatus == 1) //Last pausePlay() played Spotify and user didn't pause spotify
                        {
                            //Program.processIcon.showNotification(5000, Constants.appName, "Did you pause Spotify? If you start it, I'm ready!", ToolTipIcon.None);
                            Program.processIcon.setInactiveIcon();
                            programStatus = 0; 
                        }
                        break;

                    case SoundState.Spotify: //Spotify sound
                        if (programStatus == 0) //User paused spotify before
                        {
                            //Program.processIcon.showNotification(5000, Constants.appName, "Here we go", ToolTipIcon.None);
                            Program.processIcon.setPlayingIcon();
                            restart();
                        }
                        //Do nothing
                        break;

                    case SoundState.Other: //Background sound
                        //Do nothing
                        break;

                    case SoundState.Both: //Both sounds
                        if (programStatus == 0) //User paused spotify before
                        {
                            //Program.processIcon.showNotification(5000, Constants.appName, "I'm working now", System.Windows.Forms.ToolTipIcon.None);
                            restart();
                        }
                        lastPress = 0;
                        Program.processIcon.setPausedIcon();
                        SpotifyControl.pauseSpotify(ref otherSoundQueue, ref spotifySoundQueue, ref Constants.SPOTIFY_QUEUE_SIZE, ref currentQueueSize, ref ticksInactive, ref tickDelay);
                        break;
                }
            }
        }

        //Brings program to the initial state
        public static void setInitials()
        {
            lastPress = 1;
            programStatus = 1;
            currentState = 0;
            tickDelay = 0;

            userDeactivated = false;
            transition = false;

            spotifyWarningShown = false;
            unknownErrorShown = false;

            currentQueueSize = Constants.SHORT_QUEUE_SIZE;
            ticksInactive = Constants.TICKS_BEFORE_START;

            QueueControl.fillQueuesInitial(ref otherSoundQueue, ref spotifySoundQueue, ref Constants.SPOTIFY_QUEUE_SIZE, ref currentQueueSize);

        }

        //called when user deactivates Pausify
        public static void deactivate() 
        {
            userDeactivated = true;
        }

        //called when user activates Pausify
        public static void activate() 
        {
            //userDeactivated = false;
            restart();
        }

        //restarting is done by setting everything to initial state
        public static void restart()
        {
            ticksInactive = 1;
            MainControl.setInitials();
        }
        
    }
}
