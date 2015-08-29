﻿using System;
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
    //A LOT OF CHANGES
    //TODO: show start message only the first time application is opened
    //TODO: Shorten the background sound reaction a little
    //TODO: change how installation and update works
    

    class Program
    {
        public static ProcessIcon processIcon;
        public static SettingsForm settingsForm;
        

        public static bool settingsOpen = false;
        


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            // Show the system tray icon.					
            processIcon = new ProcessIcon();
            

            
            PauseControl.setInitials();
            FileManager.checkFiles();

            processIcon.Display();

            



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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            


            AdControl.decide();
            PauseControl.processTicks();
           
            sw.Stop();
            //Console.WriteLine("Elapsed={0} - Window name: {1}", sw.Elapsed,PauseControl.spotifyWindowName);
        }

    }
}
