using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using Stuff.Exceptions;
using SynthLib.Board;
using SynthLib.Board.Modules;
using SynthLib.Effects;
using SynthLib.Oscillators;

namespace SynthLib.Data
{
    /// <summary>
    /// Holds all global synth data and settings
    /// </summary>
    public class SynthData
    {
        public int SampleRate { get; }
        public int DesiredLatency { get; }

        public string Root { get; }

        public PathList BoardPaths { get; }

        public PathList MidiPaths { get; }

        public PathList WavPaths { get; }

        public PathList ModuleTypePaths { get; }

        public PathList OscillatorPaths { get; }

        public PathList EffectPaths { get; }

        private SettingsManager settings;

        public LoaderTypes<Module> ModuleTypes { get; }

        public LoaderTypes<Oscillator> OscillatorTypes { get; }

        public LoaderTypes<Effect> EffectTypes { get; }

        public SynthData(string settingsPath = "Assets/Settings")
        {
            settings = SettingsLoader.LoadSettings(settingsPath);

            SampleRate = settings.GetInt("main", "sampleRate");
            DesiredLatency = settings.GetInt("main", "desiredLatency");

            Root = settings.GetString("paths", "root");
            Console.WriteLine("Should error log if root path isn't a directory.");
            if (!File.GetAttributes(Root).HasFlag(FileAttributes.Directory))
                throw new SettingsException("paths", "key", "root path must be a directory");
            BoardPaths = new PathList(settings.GetStrings("paths", "boards"), Root);
            Console.WriteLine("Should error log if no directory paths are given.");
            MidiPaths = new PathList(settings.GetStrings("paths", "midi"), Root);
            Console.WriteLine("Should error log if no directory paths are given.");
            WavPaths = new PathList(settings.GetStrings("paths", "wav"), Root);
            Console.WriteLine("Should error log if no directory paths are given.");

            ModuleTypePaths = new PathList(settings.GetStrings("paths", "moduleTypes"), Root);
            OscillatorPaths = new PathList(settings.GetStrings("paths", "oscillatorTypes"), Root);
            EffectPaths = new PathList(settings.GetStrings("paths", "effectTypes"), Root);

            ModuleTypes = new LoaderTypes<Module>(ModuleTypePaths, "moduleType", "SynthLib");
            OscillatorTypes = new LoaderTypes<Oscillator>(OscillatorPaths, "oscillatorType", "SynthLib");
            EffectTypes = new LoaderTypes<Effect>(EffectPaths, "effectType", "SynthLib");
        }
    }
}
