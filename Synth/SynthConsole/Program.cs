using System;
using NAudio.Wave;
using SynthLib.Data;
using Stuff;

namespace SynthConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var synth = new SynthLib.Synth(new SynthData());
            Console.ReadLine();
            Console.WriteLine(synth);
        }
    }
}
