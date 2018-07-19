using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SynthLib.Board.Modules;

using Stuff;

namespace SynthLib.Board
{
    public class ModuleBoard
    {
        private Module[] modules;

        private InputTable inputTable;

        private float frequency;

        /// <summary>
        /// Time in milliseconds since last note event.
        /// </summary>
        public int Time { get; private set; }

        /// <summary>
        /// The number of samples since last note event;
        /// </summary>
        private int samples;

        public bool IsNoteOn { get; private set; }

        public int Note { get; private set; }

        public int SampleRate { get; }

        public ModuleBoard(Module[] modules, int sampleRate = 44100)
        {
            this.modules = modules;
            SortModules();
            for (int i = 0; i < modules.Length; ++i)
                this.modules[i].num = i;

            inputTable = new InputTable(this.modules);
            
            SampleRate = sampleRate;

            frequency = 0;

            Time = 0;
            samples = 0;
        }

        public float Frequency
        {
            get => frequency;
            set
            {
                frequency = value;
                foreach (var mod in modules)
                    mod.UpdateFrequency(value);
            }
        }

        public void NoteOn(int note)
        {
            Frequency = Midi.Frequencies[note];
            Note = note;
            Time = 0;
            IsNoteOn = true;
            samples = 0;
        }

        public void NoteOff()
        {
            Time = 0;
            IsNoteOn = false;
            samples = 0;
        }

        private void SortModules()
        {
            Validate();
            modules = modules.TopologicalSort(m => m.Inputs.Where(con => con != null).Select(con => con.Source)).ToArray();
        }

        /// <summary>
        /// Validates the state of the module network. Does nothing if not in debug.
        /// </summary>
        public void Validate()
        {
            foreach (var mod in modules)
            {
                foreach (var con in mod.Connections().Where(c => c != null))
                    con.Validate();
                //TODO: Validate modules' order.
            }
        }

        public float Next()
        {
            if (frequency == 0)
                return 0;
            ++samples;
            Time = samples / (SampleRate / 1000); 

            float result = 0;
            Module curModule;

            inputTable.ResetInputs();
            for (int i = 0; i < modules.Length; ++i)
            {
                curModule = inputTable.modules[i];
                var output = curModule.Process(inputTable.input[i], (int)Time, IsNoteOn);
                if (curModule.Type == "E")
                    result += output[0];
                else
                {
                    for (int j = 0; j < output.Length; ++j)
                    {
                        if (curModule.Outputs[j] != null)
                        {
                            var dest = curModule.Outputs[j];
                            inputTable.input[dest.Destination.num][dest.DestinationIndex] = output[j];
                        }
                    }
                }
            }
            return result;
        }

        private struct InputTable
        {
            public Module[] modules;

            public float[][] input;

            private int i;

            public InputTable(Module[] modules)
            {
                this.modules = modules;
                input = new float[modules.Length][];
                for (int i = 0; i < modules.Length; ++i)
                    input[i] = new float[modules[i].Inputs.Count];
                i = -1;
            }

            public float[] GetInput(Module mod)
            {
                i = -1;
                while (modules[++i] != mod)
                    ;
                return input[i];
            }

            public void ResetInputs()
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    for (int j = 0; j < input[i].Length; ++j)
                        input[i][j] = 0;
                }
            }
        }
    }
}
