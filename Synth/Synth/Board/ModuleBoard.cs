using System;
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

        private readonly List<ValueProvider> valueProviders;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public ModuleBoard(Module[] modules, int sampleRate = 44100)
        {
            this.modules = modules;
            SortModules();

            inputTable = new InputTable(this.modules);

            valueProviders = new List<ValueProvider>();
            SampleRate = sampleRate;
            Finished = false;
        }

        private void SortModules()
        {
            Validate();
            modules = modules.TopologicalSort(m => m.Inputs.Where(con => con != null).Select(con => con.Source)).ToArray();
        }

        public void AddValueProvider(params ValueProvider[] valueProviders)
        {
            this.valueProviders.AddRange(valueProviders);
        }

        public void RemoveValueProvider(params ValueProvider[] valueProviders)
        {
            foreach (var vp in valueProviders)
                this.valueProviders.Add(vp);
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

        public void Reset()
        {
            foreach (var mod in modules)
                mod.Reset();
        }

        public float Next(float frequency)
        {
            foreach (var vp in valueProviders)
                vp.Next();

            float result = 0;

            inputTable.ResetInputs();
            for (int i = 0; i < modules.Length; ++i)
            {
                var output = inputTable.modules[i].Process(inputTable.input[i], frequency);
                if (inputTable.modules[i].Type == "End")
                    result += output[0];
                else
                {
                    for (int j = 0; j < output.Length; ++j)
                    {
                        if (inputTable.modules[i].Outputs[j] != null)
                            inputTable.GetInput(inputTable.modules[i].Outputs[j].Destination)[inputTable.modules[i].Outputs[j].DestinationIndex] = output[j];
                    }
                }
            }
            return result;
        }

        private struct InputTable
        {
            public Module[] modules;

            public float[][] input;

            public InputTable(Module[] modules)
            {
                this.modules = modules;
                input = new float[modules.Length][];
                for (int i = 0; i < modules.Length; ++i)
                    input[i] = new float[modules[i].Inputs.Count];
            }

            public float[] GetInput(Module mod)
            {
                int i = -1;
                while (modules[++i] != mod) ;
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
