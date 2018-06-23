using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthLib
{
    public interface IValueProvider
    {
        int SampleRate { get;  }

        float Value();

        void Next(); 
    }
}
