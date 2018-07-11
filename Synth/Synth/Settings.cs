using Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib
{
    public static class Settings
    {
        private readonly static SettingsManager settings;

        static Settings()
        {
            settings = SettingsLoader.LoadSettings();
        }

        public static string GetString(string category, string key, int index = 0) => settings.GetString(category, key, index);
    }
}
