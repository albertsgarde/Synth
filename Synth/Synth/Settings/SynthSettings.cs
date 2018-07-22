using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using Stuff.Exceptions;

namespace SynthLib.Settings
{
    public class SynthSettings
    {
        public int SampleRate { get; }
        public int DesiredLatency { get; }

        private readonly string rootPath;

        public PathList BoardPaths { get; }

        public PathList MidiPaths { get; }

        public PathList WavPaths { get; }

        private SettingsManager settings;

        public SynthSettings(string settingsPath = "Assets/Settings")
        {
            settings = SettingsLoader.LoadSettings(settingsPath);

            SampleRate = settings.GetInt("main", "sampleRate");
            DesiredLatency = settings.GetInt("main", "desiredLatency");

            rootPath = settings.GetString("paths", "root");
            Console.WriteLine("Should error log if root path isn't a directory.");
            if (!File.GetAttributes(rootPath).HasFlag(FileAttributes.Directory))
                throw new SettingsException("paths", "key", "root path must be a directory");
            BoardPaths = new PathList(settings.GetStrings("paths", "boards"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
            MidiPaths = new PathList(settings.GetStrings("paths", "midi"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
            WavPaths = new PathList(settings.GetStrings("paths", "wav"), rootPath);
            Console.WriteLine("Should error log if no directory paths are given.");
        }

        public class PathList : IEnumerable<string>
        {
            public string Root { get; }

            private List<string> paths;

            /// <summary>
            /// The First path is taken as the root.
            /// </summary>
            /// <param name="paths"></param>
            public PathList(IEnumerable<string> paths, string superRoot = "")
            {
                Root = Path.IsPathRooted(paths.First()) ? paths.First() : Path.Combine(superRoot, paths.First());
                if (!File.GetAttributes(Root).HasFlag(FileAttributes.Directory))
                    throw new DirectoryNotFoundException();
                this.paths = new List<string>();
                this.paths.Add(Root);
                foreach (var path in (from p in paths where p != paths.First() select FilePath(p)))
                {
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory) && !path.EndsWith("/"))
                        Add(path + "/");
                    else
                        Add(path);
                }
            }

            public PathList(params string[] paths) : this(paths.AsEnumerable()) { }

            public void Add(string path)
            {
                if (Path.IsPathRooted(path))
                    paths.Add(path);
                else
                    paths.Add(Path.Combine(Root, path));
            }

            public void Remove(string path)
            {
                paths.Remove(path);
            }

            public string FirstFile()
            {
                return Files().First();
            }

            public string FilePath(string path)
            {
                return Path.IsPathRooted(path) ? path : Path.Combine(Root, path);
            }

            public int DirCount()
            {
                return paths.Count(path => File.GetAttributes(path).HasFlag(FileAttributes.Directory));
            }

            public IEnumerable<string> Files()
            {
                foreach (var path in paths)
                {
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                    {
                        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                            yield return file;
                    }
                    else
                        yield return path;
                }
            }

            public IEnumerator<string> GetEnumerator()
            {
                return paths.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
