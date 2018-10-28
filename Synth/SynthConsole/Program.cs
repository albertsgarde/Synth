using System;
using NAudio.Wave;
using SynthLib.Data;
using Stuff;
using System.Threading;

namespace SynthConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            var synth = new SynthLib.Synth(new SynthData());
            Console.ReadLine();
            Console.WriteLine(synth);
        }
    }
}
