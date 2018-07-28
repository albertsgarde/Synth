using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        private SettingsManager settings;

        public Logger Log { get; }

        public int SampleRate { get; }
        public int DesiredLatency { get; }
        /// <summary>
        /// How far the pitch wheel can bend. In semitones.
        /// </summary>
        public float PitchWheelRange { get; }


        public string Root { get; }


        public PathList SavedBoardsPaths { get; }
        public PathList DefaultBoardsPaths { get; }
        public PathList MidiPaths { get; }
        public PathList WavPaths { get; }

        public PathList ModuleTypePaths { get; }
        public PathList OscillatorPaths { get; }
        public PathList EffectPaths { get; }


        public LoaderTypes<Module> ModuleTypes { get; }
        public LoaderTypes<Oscillator> OscillatorTypes { get; }
        public LoaderTypes<Effect> EffectTypes { get; }


        public IReadOnlyDictionary<string, BoardTemplate> SubBoards { get; }

        public SynthData(string settingsPath = "Assets/Settings")
        {
            settings = SettingsLoader.LoadSettings(settingsPath);

            Root = settings.GetString("paths", "root");
            if (!File.GetAttributes(Root).HasFlag(FileAttributes.Directory))
                throw new SettingsException("paths", "key", "root path must be a directory");

            Log = new Logger(new PathList(settings.GetStrings("paths", "log"), Root).Root);

            SampleRate = settings.GetInt("main", "sampleRate");
            DesiredLatency = settings.GetInt("main", "desiredLatency");
            PitchWheelRange = settings.GetFloat("main", "pitchWheelChange");


            SavedBoardsPaths = new PathList(settings.GetStrings("paths", "savedBoards"), Root);
            DefaultBoardsPaths = new PathList(settings.GetStrings("paths", "defaultBoards"), Root);
            MidiPaths = new PathList(settings.GetStrings("paths", "midi"), Root);
            WavPaths = new PathList(settings.GetStrings("paths", "wav"), Root);

            ModuleTypePaths = new PathList(settings.GetStrings("paths", "moduleTypes"), Root);
            OscillatorPaths = new PathList(settings.GetStrings("paths", "oscillatorTypes"), Root);
            EffectPaths = new PathList(settings.GetStrings("paths", "effectTypes"), Root);


            ModuleTypes = new LoaderTypes<Module>(ModuleTypePaths, "moduleType", "SynthLib");
            OscillatorTypes = new LoaderTypes<Oscillator>(OscillatorPaths, "oscillatorType", "SynthLib");
            EffectTypes = new LoaderTypes<Effect>(EffectPaths, "effectType", "SynthLib");


            var subBoards = new Dictionary<string, BoardTemplate>();
            foreach (var f in DefaultBoardsPaths.Files())
            {
                var element = XDocument.Load(f).Root;
                var subBoard = new BoardTemplate(element, this);
                subBoards[Path.GetFileNameWithoutExtension(f)] = subBoard;
            }
            SubBoards = subBoards;

        }
    }
}
