using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SynthLib.Data;
using Stuff;

namespace SynthLib.Effects
{
    public class Filter : Effect
    {
        public override string Type => "Filter";

        private const int VALUES = 0;

        public override int Values => VALUES;

        private readonly float[] kernel;

        private readonly CircularArray<float> prevs;

        public Filter()
        {
            useable = false;
        }

        public Filter(IReadOnlyList<float> kernel)
        {
            this.kernel = kernel.ToArray();
            prevs = new CircularArray<float>(kernel.Count());
        }

        protected override float Next(float[] input)
        {
            prevs.Add(input[VALUES]);
            float result = 0;
            for (int i = 0; i < prevs.Length; ++i)
                result += kernel[i] * prevs[i];
            return result;
        }

        public override Effect Clone()
        {
            return new Filter(kernel);
        }

        public override Effect CreateInstance(XElement element, SynthData data)
        {
            var impulseResponse = new List<float>();
            foreach (var value in element.Element("impulseResponse").Elements())
                impulseResponse.Add(InvalidEffectSaveElementException.ParseFloat(value));
            return new Filter(impulseResponse);
        }

        public override XElement ToXElement(string name)
        {
            var element = base.ToXElement(name);
            var ir = element.CreateElement("impulseResponse");
            for (int i = 0; i < kernel.Length; ++i)
                ir.AddValue("value", kernel[i]);
            return element;
        }

        /// <summary>
        /// Generates a kernel for a low pass filter with a specific cutoff frequency.
        /// </summary>
        /// <param name="cutoffFrequency">The cutoff frequency of the generated low pass filter kernel, given as fraction of sample rate.</param>
        /// <param name="length">The length of the kernel</param>
        /// <returns></returns>
        public static float[] GenerateLowPassKernel(float cutoffFrequency, int length)
        {
            if (length % 2 == 0)
                length++;
            var result = new float[length];
            var halfLength = length / 2;
            result[halfLength] = 0;
            for (int i = 1; i <= halfLength; i++)
                result[halfLength + i] = result[halfLength - i] = (float)(Math.Sin(2 * Math.PI * cutoffFrequency * i) / (i * Math.PI));
            Console.WriteLine(result.AsString());
            return result;
        }

        /// <summary>
        /// Generates a kernel for a low pass filter with a specific cutoff frequency.
        /// </summary>
        /// <param name="cutoffFrequency">Given as Hz</param>
        /// <param name="length">The length of the kernel</param>
        /// <returns></returns>
        public static float[] GenerateLowPassKernel(float cutoffFrequency, int length, int sampleRate)
        {
            return GenerateLowPassKernel(cutoffFrequency / sampleRate, length);
        }
    }
}
