using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SynthLib.Board.Modules;

using Stuff;
using NAudio.Midi;
using SynthLib.Data;
using SynthLib.Music;

namespace SynthLib.Board
{
    public class ModuleBoard
    {
        private Module[] modules;

        private readonly InputTable inputTable;

        private readonly List<int> controllerValues;

        public IReadOnlyList<int> ControllerValues => controllerValues;

        public int PitchWheel { get; private set; }

        private readonly float pitchWheelRange;

        private float baseFrequency;

        private float frequencyModifier;

        private float frequency;

        /// <summary>
        /// Time in milliseconds since last note event.
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// The number of samples since last note event;
        /// </summary>
        private long samples;

        public bool IsNoteOn { get; private set; }

        public int Note { get; private set; }

        public int SampleRate { get; }

        public float GlideModifier { get; private set; }

        public ModuleBoard(Module[] modules, SynthData data)
        {
            SampleRate = data.SampleRate;
            pitchWheelRange = data.PitchWheelRange;

            this.modules = modules;
            SortModules();
            for (int i = 0; i < modules.Length; ++i)
                this.modules[i].num = i;

            controllerValues = new List<int>(128);
            for (int i = 0; i < controllerValues.Capacity; ++i)
                controllerValues.Add(64);
            PitchWheel = 8192;

            inputTable = new InputTable(this.modules);

            baseFrequency = 0;
            frequencyModifier = 1;
            frequency = baseFrequency;

            Time = 0;
            samples = 0;

            UpdateFrequency();
        }

        public float BaseFrequency
        {
            get => baseFrequency;
            set
            {
                baseFrequency = value;
                UpdateFrequency();
            }
        }

        private void UpdateFrequency()
        {
            frequency = baseFrequency * (float)Tone.FrequencyMultiplierFromNoteOffset(frequencyModifier);
            foreach (var mod in modules)
                mod.UpdateFrequency(frequency);
        }

        public void NoteOn(int note)
        {
            BaseFrequency = Midi.Frequencies[note];
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

        public void PitchWheelChange(int pitch)
        {
            PitchWheel = pitch;
        }

        public void ControllerChange(MidiController controller, int controllerValue)
        {
            controllerValues[(int)controller] = controllerValue;
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

        public (float left, float right) Next()
        {
            GlideModifier = 1;
            frequencyModifier = 1;

            ++samples;
            Time = samples * 1000 / SampleRate; 

            var result = (left: 0f, right: 0f);
            Module curModule;

            inputTable.ResetInputs();
            for (int i = 0; i < modules.Length; ++i)
            {
                curModule = inputTable.modules[i];
                var output = curModule.Process(inputTable.input[i], Time, IsNoteOn, this);
                switch (curModule.OutputType)
                {
                    case BoardOutput.Left:
                        result.left += output[0];
                        break;
                    case BoardOutput.Right:
                        result.right += output[0];
                        break;
                    case BoardOutput.GlideTime:
                        GlideModifier *= output[0];
                        break;
                    case BoardOutput.PitchShift:
                        frequencyModifier *= output[0] * pitchWheelRange;
                        UpdateFrequency();
                        break;
                    case BoardOutput.None:
                        for (int j = 0; j < output.Length; ++j)
                        {
                            if (curModule.Outputs[j] != null)
                            {
                                var dest = curModule.Outputs[j];
                                inputTable.input[dest.Destination.num][dest.DestinationIndex] = output[j];
                            }
                        }
                        break;
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
