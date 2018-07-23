using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using Stuff.Exceptions;

namespace SynthLib.Settings
{
    public class SynthSettings
    {
        public int SampleRate { get; }
        public int DesiredLatency { get; }

        private readonly string rootPath;

        public PathList BoardPaths { get; }

        public PathList MidiPaths { get; }

        public PathList WavPaths { get; }

        private SettingsManager settings;

        public SynthSettings(string settingsPath = "Assets/Settings")
        {
            settings = SettingsLoader.LoadSettings(settingsPath);

            SampleRate = settings.GetInt("main", "sampleRate");
            DesiredLatency = settings.GetInt("main", "desiredLatency");

            rootPath = settings.GetString("paths", "root");
            Console.WriteLine("Should error log if root path isn't a directory.");
            if (!File.GetAttributes(rootPath).HasFlag(FileAttributes.Directory))
                throw new SettingsException("paths", "key", "root path must be a directory");
            BoardPaths = new PathList(settings.GetStrings("paths", "boards"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
            MidiPaths = new PathList(settings.GetStrings("paths", "midi"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
            WavPaths = new PathList(settings.GetStrings("paths", "wav"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
        }
    }
}
