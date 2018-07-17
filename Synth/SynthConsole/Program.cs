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

namespace SynthConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            MidiFile file = new MidiFile("D:/PenguinAgen/Documents/Synth/midi/SimpleMidiTest.mid");
            Console.WriteLine(file.Tracks);
            Console.WriteLine(file.Events[0].AsString());
            var synth = new SynthLib.Synth(new SynthLib.SynthSettings());
            Console.ReadLine();
        }
    }
}
