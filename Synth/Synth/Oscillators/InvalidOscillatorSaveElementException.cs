using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Oscillators
{
    public class InvalidOscillatorSaveElementException : Exception
    {
        public XElement Element { get; }

        public InvalidOscillatorSaveElementException(XElement element) : base()
        {
            Element = element;
        }

        public InvalidOscillatorSaveElementException(XElement element, string message) : base(message)
        {
            Element = element;
        }

        public static float ParseFloat(XElement element)
        {
            if (!float.TryParse(element.Value, out float result))
                throw new InvalidOscillatorSaveElementException(element, element.Name + " not a float. value: " + element.Value);
            return result;
        }

        public static int ParseInt(XElement element)
        {
            if (!int.TryParse(element.Value, out int result))
                throw new InvalidOscillatorSaveElementException(element, element.Name + " not a int. value: " + element.Value);
            return result;
        }
    }
}
