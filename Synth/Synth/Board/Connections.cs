using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthLib.Board.Modules;
using Stuff;
using System.Xml.Linq;

namespace SynthLib.Board
{
    public abstract class Connections : IEnumerable<Connection>, ISaveable
    {
        public abstract Connection this[int index]
        {
            get;
        }

        public abstract int Count { get; }

        /// <summary>
        /// From which index non-specifically assigned connections can be connected.
        /// </summary>
        public int FreeConnectionsStart { get; }

        public Connections(int freeConnectionsStart)
        {
            FreeConnectionsStart = freeConnectionsStart;
        }

        public Connections(XElement element)
        {
            if (int.TryParse(element.ElementValue("freeConnectionsStart"), out int freeConnectionsStart))
                FreeConnectionsStart = freeConnectionsStart;
            else
                throw new InvalidModuleSaveElementException(element);
        }

        /// <summary>
        /// Creates a new connection between two modules at the specified indexes.
        /// If the modules already have a connection at those indexes, the existing connections will be destroyed and returned.
        /// </summary>
        /// <param name="source">The module to connect from.</param>
        /// <param name="dest">The module to connect to.</param>
        /// <param name="destIndex">Which of the destinations input indexes to plug the connection in to.</param>
        /// <returns>A tuple of first the source's old connection and then the destination's old connection. null if there were none.</returns>
        public static (Connection, Connection) NewConnection(Module source, int sourceIndex, Module dest, int destIndex)
        {
            var con = new Connection(source, sourceIndex, dest, destIndex);
            var oldSourceCon = source.Outputs.AddConnection(con, sourceIndex);
            var oldDestCon = dest.Inputs.AddConnection(con, destIndex);
            if (oldSourceCon != null)
                Destroy(oldSourceCon);
            if (oldDestCon != null)
                Destroy(oldDestCon);
            con.Validate();
            return (oldSourceCon, oldDestCon);
        }

        /// <summary>
        /// Creates a new connection between the lowest available index in the source module and the specified index in the destination module.
        /// If the destination module already has a connection in the specified index, that connection is destroyed and returned
        /// </summary>
        /// <param name="source">The module to connect from.</param>
        /// <param name="dest">The module to connect to.</param>
        /// <param name="destIndex">Which of the destinations input indexes to plug the connection in to.</param>
        /// <returns>The destination's old connection. null if there was none.</returns>
        /// <exception cref="System.Exception">Thrown when the source module has no free connections.</exception>
        public static Connection NewConnection(Module source, Module dest, int destIndex)
        {
            if (source.Outputs.FirstFreeConnection(out int sourceIndex))
                return NewConnection(source, sourceIndex, dest, destIndex).Item2;
            else
                throw new NoFreeConnectionsException(source, "Source module has no free connections.");
        }

        /// <summary>
        /// Creates a new connection between the specified index in the source module and the lowest available index in the destination module.
        /// If the source module already has a connection in the specified index, that connection is destroyed and returned
        /// </summary>
        /// <param name="source">The module to connect from.</param>
        /// <param name="sourceIndex">Which of the sources output indexes to plug the connection in to.</param>
        /// <param name="dest">The module to connect to.</param>
        /// <returns>The source's old connection. null if there was none.</returns>
        /// <exception cref="System.Exception">Thrown when the destination module has no free connections.</exception>
        public static Connection NewConnection(Module source, int sourceIndex, Module dest)
        {
            if (dest.Inputs.FirstFreeConnection(out int destIndex))
                return NewConnection(source, sourceIndex, dest, destIndex).Item1;
            else
                throw new NoFreeConnectionsException(dest, "Destination module has no free connections.");
        }

        /// <summary>
        /// Creates a new connection between the lowest available indexes in the specified modules.
        /// </summary>
        /// <param name="source">The module to connect from.</param>
        /// <param name="dest">The module to connect to.</param>
        /// <exception cref="System.Exception">Thrown when either the source or the destination modules have no free connections.</exception>
        public static void NewConnection(Module source, Module dest)
        {
            if (source.Outputs.FirstFreeConnection(out int sourceIndex))
            {
                if (dest.Inputs.FirstFreeConnection(out int destIndex))
                    NewConnection(source, sourceIndex, dest, destIndex);
                else
                    throw new NoFreeConnectionsException(dest, "Destination module has no free connections.");
            }
            else
                throw new NoFreeConnectionsException(source, "Source module has no free connections.");
        }

        public static void Destroy(Connection con)
        {
            con.Source.Outputs.RemoveConnection(con, con.SourceIndex);
            con.Destination.Inputs.RemoveConnection(con, con.DestinationIndex);
        }

        /// <summary>
        /// Plugs in the specified connection to the specified index.
        /// If there already is an in that index, that connection will be replaced and returned.
        /// </summary>
        protected abstract Connection AddConnection(Connection con, int index);

        /// <summary>
        /// Unpluggs the specified connection at the specified index.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the Connection isn't at the specified index.</exception>
        protected abstract void RemoveConnection(Connection con, int index);
        
        /// <param name="index">The lowest index with no connection. -1 if no such index exists.</param>
        /// <returns>Whether and index with no connection exists.</returns>
        protected abstract bool FirstFreeConnection(out int index);

        public abstract IEnumerator<Connection> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Starts the construction of an XElement that describes the connections. Should be called at the start of any Connections ToXElement implementation.
        /// Saves data about the number of connections and start of the free connections, the rest should be saved by the individual connections.
        /// </summary>
        /// <remarks>
        /// Note that the connections should not be saved, as they are a property of the module board, not the individual modules.
        /// </remarks>
        public virtual XElement ToXElement(string name)
        {
            var element = new XElement(name);
            element.AddValue("count", Count);
            element.AddValue("freeConnectionsStart", FreeConnectionsStart);
            return element;
        }
    }
}
