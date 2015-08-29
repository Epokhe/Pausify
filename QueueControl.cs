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
    //Queue worker class, just does what it's told
    class QueueControl
    {
        public static void fillQueuesAfterPlay(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize)
        {
            otherSoundQueue.Clear();
            spotifySoundQueue.Clear();

            for (int i = 0; i <currentQueueSize; i++)
            {
                otherSoundQueue.Enqueue(0f);
            }

            for (int i = 0; i < spotifyQueueSize; i++)
            {
                spotifySoundQueue.Enqueue(0.2f);
            }
        }

        public static void fillQueuesInitial(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize)
        {
            spotifySoundQueue.Clear();
            otherSoundQueue.Clear();

            for (int i = 0; i < spotifyQueueSize; i++)
            {
                spotifySoundQueue.Enqueue(0f);
            }
            for (int i = 0; i < currentQueueSize; i++)
            {
                otherSoundQueue.Enqueue(0f);
            }
        }

        public static void fillQueuesAfterPause(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize)
        {
            float average = 0;

            for (int i = 0; i < currentQueueSize; i++)
            {
                average += otherSoundQueue.Dequeue();
            }
            average /= currentQueueSize;

            currentQueueSize = Configuration.LONG_QUEUE_SIZE;

            for (int i = 0; i < currentQueueSize; i++)
            {
                otherSoundQueue.Enqueue(average);
            }

            spotifySoundQueue.Clear();

            for (int i = 0; i < spotifyQueueSize; i++)
            {
                spotifySoundQueue.Enqueue(0f);
            }
        }

        public static void fillQueuesWithAverage(ref Queue<float> otherSoundQueue, ref Queue<float> spotifySoundQueue, ref int spotifyQueueSize, ref int currentQueueSize)
        {
           
            float average = 0;
            foreach (float soundLevel in spotifySoundQueue)
            {
                average += soundLevel;
            }
            average /= Configuration.TICKS_BEFORE_START;


            spotifySoundQueue.Clear();
            for (int i = 0; i < spotifyQueueSize; i++)
            {
                spotifySoundQueue.Enqueue(average);
            }

            average = 0;
            foreach (float soundLevel in otherSoundQueue)
            {
                average += soundLevel;
            }
            average /= Configuration.TICKS_BEFORE_START;

            otherSoundQueue.Clear();
            for (int i = 0; i < currentQueueSize; i++)
            {
                otherSoundQueue.Enqueue(average);
            }
        
        }

        public static void dequeue(ref Queue<float> queue)
        {
            queue.Dequeue();
        }

        public static void enqueue(ref Queue<float> queue, ref float value)
        {
            queue.Enqueue(value);
        }

    }
}
