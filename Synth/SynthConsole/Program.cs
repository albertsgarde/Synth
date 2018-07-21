using SynthLib.Board;
using SynthLib.Board.Modules;
using SynthLib.Oscillators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using Stuff;
using SynthLib.Settings;

namespace SynthConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var synth = new SynthLib.Synth(new SynthSettings());
            Console.ReadLine();
            Console.WriteLine(synth);
        }
    }
}
