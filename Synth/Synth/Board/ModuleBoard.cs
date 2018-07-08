﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SynthLib.SynthProviders;
using SynthLib.Board.Modules;
using SynthLib.ValueProviders;
using Stuff;

namespace SynthLib.Board
{
    public class ModuleBoard
    {
        private Module[] modules;

        private InputTable inputTable;

        private float frequency;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public ModuleBoard(float frequency, Module[] modules, int sampleRate = 44100)
        {
            this.modules = modules;
            SortModules();
            for (int i = 0; i < modules.Length; ++i)
                this.modules[i].num = i;

            Frequency = frequency;

            inputTable = new InputTable(this.modules);
            
            SampleRate = sampleRate;
            Finished = false;
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
            float result = 0;
            Module curModule;

            inputTable.ResetInputs();
            for (int i = 0; i < modules.Length; ++i)
            {
                curModule = inputTable.modules[i];
                var output = curModule.Process(inputTable.input[i]);
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
