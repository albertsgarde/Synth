using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynthLib.Effects
{
    public class InvalidEffectSaveElementException : Exception
    {
        public XElement Element { get; }

        public InvalidEffectSaveElementException(XElement element) : base()
        {
            Element = element;
        }

        public InvalidEffectSaveElementException(XElement element, string message) : base(message)
        {
            Element = element;
        }

        public static float ParseFloat(XElement element)
        {
            if (!float.TryParse(element.Value, out float result))
                throw new InvalidEffectSaveElementException(element, element.Name + " not a float. value: " + element.Value);
            return result;
        }

        public static int ParseInt(XElement element)
        {
            if (!int.TryParse(element.Value, out int result))
                throw new InvalidEffectSaveElementException(element, element.Name + " not a int. value: " + element.Value);
            return result;
        }
    }
}
