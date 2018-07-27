using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Board.Modules
{
    /// <summary>
    /// Outputs a value between -1 and 1 depending on the value of the given midicontroller.
    /// </summary>
    public class MidiControllerModule : Module
    {
        public override Connections Inputs { get; }

        public override Connections Outputs { get; }

        public override BoardOutput OutputType => BoardOutput.None;

        private readonly int controller;

        private readonly float[] output;

        public const float TWO_OVER_128 = 2f/128;

        public MidiControllerModule()
        {
            useable = false;
        }

        public MidiControllerModule(int controller)
        {
            this.controller = controller;
            output = new float[1];
            Inputs = new ConnectionsArray(0);
            Outputs = new ConnectionsArray(1);
        }

        public override Module Clone(int sampleRate = 44100)
        {
            return new MidiControllerModule(controller);
        }

        public override Module CreateInstance(XElement element, SynthData data)
        {
            return new MidiControllerModule(InvalidModuleSaveElementException.ParseInt(element.Element("controller")));
        }

        protected override float[] IntProcess(float[] inputs, long time, bool noteOn, ModuleBoard moduleBoard)
        {
            output[0] = moduleBoard.ControllerValues[controller] * TWO_OVER_128 - 1;
            return output;
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("controller", controller);
            return element;
        }
    }
}
