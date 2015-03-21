using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pausify
{
    class Constants
    {
        //Time
        public static int TICK_PERIOD = 250;
        public static int MS_IN_SEC = 1000;
        public static int TICKS_IN_SEC = MS_IN_SEC / TICK_PERIOD;
        public static int TICKS_BEFORE_START = 2 * TICKS_IN_SEC;

        //Queue
        public static int SHORT_QUEUE_SIZE = (int)(2 * TICKS_IN_SEC);
        public static int LONG_QUEUE_SIZE = (int)(4.5 * TICKS_IN_SEC);
        public static int SPOTIFY_QUEUE_SIZE = 7 * TICKS_IN_SEC;

        //State
        public static int SPOTIFY_MUSIC_PLAYING = 1;
        public static int OTHER_SOUND_PLAYING = 2;

        //Sound
        public static float SOUND_LOW_LIMIT = 0.00001f;
        public static float SPOTIFY_SOUND_HIGH_LIMIT = 0.005f;
        public static float OTHER_SOUND_HIGH_LIMIT = 0.008f;
        public static float NORMALIZE_VALUE = 0.01f;

        public static string appName = "Pausify";
    }
}
