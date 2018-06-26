using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SynthLib.SynthProviders;
using SynthLib.Board.Modules;
using Stuff;

namespace SynthLib.Board
{
    public class ModuleBoard : ISynthProvider, IEnumerable<Module>
    {
        private IList<Module> modules;

        public int SampleRate { get; }

        public bool Finished { get; private set; }

        public ModuleBoard(int sampleRate = 44100)
        {
            modules = new List<Module>();
            SampleRate = sampleRate;
            Finished = false;
        }

        private void SortModules()
        {
            Validate();
            modules = modules.TopologicalSort(m => m.Inputs.Where(con => con != null).Select(con => con.Source));
        }

        public void Add(params Module[] modules)
        {
            foreach (var mod in modules)
                this.modules.Add(mod);
            SortModules();
        }

        public void Remove(params Module[] modules)
        {
            foreach (var mod in modules)
            {
                this.modules.Remove(mod);
                foreach (var con in mod.Connections())
                    con.Destroy();
            }
            SortModules();
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

        public IEnumerator<Module> GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        public float Next()
        {
            float result = 0;

            var inputTable = new Dictionary<Module, float[]>();
            foreach (var mod in modules)
                inputTable.Add(mod, new float[mod.Inputs.Count]);
            foreach (var mod in modules)
            {
                var output = mod.Process(inputTable[mod]);
                if (mod.Type == "End")
                    result += output[0];
                else
                {
                    for (int i = 0; i < output.Length; ++i)
                        inputTable[mod.Outputs[i].Destination][mod.Outputs[i].DestinationIndex] = output[i];
                }
            }
            return result;
        }
    }
}
