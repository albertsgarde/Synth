using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.ValueProviders;

namespace SynthLib.Board
{
    public class ValueReciever
    {
        public IValueProvider ValueProvider { get; set; }

        public float Min { get; set; }

        public float Max { get; set; }

        public ValueReciever(IValueProvider valueProvider, float min = -1, float max = 1)
        {
            ValueProvider = valueProvider;
            Min = min;
            Max = max;
        }

        public void SetConstant(float c)
        {
            ValueProvider = new Constant(c);
        }

        public void Next()
        {
            ValueProvider.Next();
        }

        public float CurrentValue()
        {
            return (ValueProvider.CurrentValue()+1)*(Max - Min) / 2 + Min;
        }

        public float NextValue()
        {
            Next();
            return CurrentValue();
        }
    }
}
