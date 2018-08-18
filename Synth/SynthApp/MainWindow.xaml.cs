using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SynthLib;
using SynthLib.Data;
using System.Xml;
using SynthLib.MidiSampleProviders;
using SynthLib.Board;
using SynthLib.Board.Modules;
using SynthLib.Oscillators;
using SynthLib.Effects;
using System.Threading;

namespace SynthApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Synth synth;

        private readonly SynthData data;
        
        private Thread updateThread;

        public MainWindow()
        {
            data = new SynthData();
            synth = new Synth(data)
            {
                MidiSampleProviderCreator = bt => new PolyBoard(bt, 6, data),
                BoardTemplate = new BoardTemplate()
            };
            InitializeComponent();
            updateThread = new Thread(UpdateGUI);
            updateThread.Start();
            RefreshBoard();
        }

        private void SaveBoard(object sender, RoutedEventArgs e)
        {
            synth.BoardTemplate.SaveToFile(boardName.Text, synth.Data);
        }

        private void RefreshBoard()
        {
            synth.BoardTemplate = SetupBoard();
        }

        private void RefreshBoard(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RefreshBoard();
        }

        private void UpdateGUI()
        {
            long updateTime = (long)1e5;
            long prevUpdate = DateTime.Now.Ticks;
            while (true)
            {
                while (DateTime.Now.Ticks - prevUpdate < updateTime) ;
                prevUpdate = DateTime.Now.Ticks;
                Dispatcher.BeginInvoke(
                new ThreadStart(() =>
                {
                    leftMax.Text = "" + synth.MaxValue.left;
                    rightMax.Text = "" + synth.MaxValue.right;
                }));
            }
        }

        private BoardTemplate SetupBoard()
        {
            var board = new BoardTemplate();

            board.AddBoard(data.SubBoards["pitchWheelSubBoard"]);

            board.AddBoard(data.SubBoards["glideSubBoard"]);

            board.AddBoard(data.SubBoards["volumeControlSubBoard"]);


            var env1 = new Envelope(30, 240, 0.6f, 40, 3);

            var o1 = new OscillatorModule(new SawOscillator(), 1);

            var o2 = new OscillatorModule(new SawOscillator(), 1, 0.08f);

            var o3 = new OscillatorModule(new SawOscillator(), 1, 11.92f);

            var d1 = new Distributer(new float[] { 1, 0.7f, 0.0f }, new float[] { 1 });

            var sf1 = new EffectModule(new SimpleFilter((int)SimpleFilter.Value));

            var g1 = new EffectModule(new Boost(0.2f));

            var dist = new EffectModule(new Limiter(0.3f));

            var lfo2 = new ConstantOscillatorModule(new SineOscillator(), 1, 0.5f);

            var p1 = new Pan();

            var endLeft = new EndModule(false);
            var endRight = new EndModule(true);

            board.Add(endLeft, endRight, sf1, d1, o3, o2, o1, env1, g1, lfo2, p1, dist /*, volumeControl, glideIn, glideOut, glideTranslate, pitchShift, pitchWheel, boardGain, volumeTranslate*/);

            //board.AddConnections(glideIn, glideTranslate, glideOut);

            //board.AddConnection(pitchWheel, pitchShift);

            //board.AddConnections(volumeControl, volumeTranslate, boardGain);


            board.AddConnection(env1, o1, destIndex: 0);
            board.AddConnection(env1, o2, destIndex: 0);
            board.AddConnection(env1, o3, destIndex: 0);


            board.AddConnections(o1, d1);
            board.AddConnections(o2, d1);
            board.AddConnections(o3, d1);

            board.AddConnections(d1, sf1, g1, dist, p1);

            board.AddConnection(p1, endLeft);
            board.AddConnection(p1, endRight);

            return board;
        }
    }
}
