using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pausify
{
    class FileManager
    {
        public static HashSet<string> adSet;
        public static Dictionary<string, string> configTable;
        public static string adFilePath = Application.StartupPath + "\\ads.txt";
        public static string configFilePath = Application.StartupPath + "\\config.ini";
        public static void checkFiles()
        {

            if (!File.Exists(adFilePath))
            {
                createDefaultAdFile();
            }

            if (!File.Exists(configFilePath))
            {
                Configuration.firsttime = true;
                createDefaultConfigFile();
            }
            else
            {

            }

            parseAds();
            parseConfig();


        }

        public static void createDefaultFiles()
        {
            createDefaultAdFile();
            createDefaultConfigFile();            
        }

        private static void createDefaultConfigFile()
        {
            string[] tmp = new string[5];
            tmp[0] = "FirstTime=0";
            tmp[1] = "AutoPause=0";
            tmp[2] = "Adblock=1";
            tmp[3] = "Remember=0";
            tmp[4] = "SpotifyVolume=100";
            File.WriteAllLines(configFilePath, tmp);
        }
        private static void createDefaultAdFile()
        {
            string[] tmp = new string[1];
            tmp[0] = "Spotify";
            File.WriteAllLines(adFilePath, tmp);
        }

        public static void refresh()
        {
            parseConfig();
            parseAds();
        }

        private static void parseAds()
        {
            if (!File.Exists(adFilePath))
            {
                createDefaultAdFile();
            }

            string[] allAds = File.ReadAllLines(adFilePath);
            adSet = new HashSet<string>(allAds, StringComparer.OrdinalIgnoreCase);
        }

        public static void changeConfig(string option, string value)
        {
            //parseConfig();

            configTable[option] = value;
            string[] configArray = new string[configTable.Count];
            int i=0;

            foreach(KeyValuePair<string,string> entry in configTable)
            {
                configArray[i++] = entry.Key + "=" + entry.Value;
            }
            File.WriteAllLines(configFilePath, configArray);
        }

        private static void parseConfig()
        {

            if (!File.Exists(configFilePath))
            {
                createDefaultConfigFile();
            }

            string[] allConfig = File.ReadAllLines(configFilePath);
            configTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < allConfig.Length; i++)
            {
                string[] splitted = new string[2];
                splitted = allConfig[i].Split('=');
                configTable.Add(splitted[0], splitted[1]);
            }

            string str;
            if (configTable.TryGetValue("adblock", out str))
            {
                Configuration.option_adblock = (str.Equals("1") ? true : false);
            }
            if (configTable.TryGetValue("autopause", out str))
            {
                Configuration.option_autopause = (str.Equals("1") ? true : false);
            }
            if (configTable.TryGetValue("remember", out str))
            {
                Configuration.option_remember = (str.Equals("1") ? true : false);
            }
            if (configTable.TryGetValue("spotifyvalue", out str))
            {
                int tmp;
                Int32.TryParse(str, out tmp);
                Configuration.spotify_volume = (float)tmp;
            }
        }

        public static void addAd(string name)
        {
            if (!adSet.Contains(name))
            {
                using (StreamWriter sw = File.AppendText(adFilePath))
                {
                    sw.WriteLine(name);
                }

                adSet.Add(name);
            }
        }

        public static void removeAds(string[] ads)
        {
            parseAds();
            for (int i = 0; i < ads.Length; i++)
            {
                FileManager.adSet.Remove(ads[i]);
            }

            File.WriteAllLines(adFilePath, adSet);
        }
    }
}
