using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Stuff;
using SynthLib.Data;

namespace SynthLib.Oscillators
{
    public class DCOscillator : Oscillator
    {
        public override string Type => "DCOscillator";

        public override float Frequency { get; set; }

        public override int SampleRate { get; }

        private readonly float value;

        public DCOscillator()
        {
            useable = false;
        }

        public DCOscillator(float value)
        {
            this.value = value;
        }

        public override Oscillator Clone(int sampleRate)
        {
            return new DCOscillator(value);
        }

        public override Oscillator CreateInstance(XElement element, SynthData data)
        {
            return new DCOscillator(InvalidOscillatorSaveElementException.ParseFloat(element.Element("value")));
        }

        public override float CurrentValue(float min = -1F, float max = 1) => value * (max - min) + min;

        public override void Next()
        { 

        }

        public override void Reset()
        {  

        }

        protected override float NextValue() => value;

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            element.AddValue("value", value);
            return element;
        }
    }
}
