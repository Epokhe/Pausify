using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;
using SpotifyLib;
using System.Collections;
using System.Diagnostics;
using WindowsAudio;
using Pausify.Properties;


//Thanks for SpotifyController class https://github.com/mscoolnerd/SpotifyLib/blob/master/SpotifyLib.cs
//Thanks to a lot of Stackoverflow posts

namespace Pausify
{
    //TODO: show start message only in first time application is opened
    //TODO: Shorten the background sound reaction a little

    static class Program
    {
        public static ProcessIcon processIcon;
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the system tray icon.					
            processIcon = new ProcessIcon();

            processIcon.Display();

            MainControl.setInitials();

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 250;
            aTimer.Enabled = true;

            // Make sure the application runs!
            Application.Run();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Timer for performance measurement
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            MainControl.processTicks();
            //sw.Stop();
            //Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

    }
}
