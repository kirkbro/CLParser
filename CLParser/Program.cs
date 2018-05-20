using CLParser.Parsing;
using System;
using System.Net;

namespace CLParser
{
    public abstract class RpcOptions
    {
            [Option("rpc-port", ShortName = 'p')]
            public ushort RpcPort { get; set; }
    }

    [Category("net")]
    public class Network
    {
        [Category("ip")]
        public class IPDatabase
        {
            [Verb("add")]
            public class Add : RpcOptions
            {
                [Option(0)]
                public IPAddress IP { get; set; }
            }

            [Verb("remove")]
            public class Remove : RpcOptions
            {
                [Option(0)]
                public IPAddress IP { get; set; }
            }

            [Verb("clear")]
            public class Clear : RpcOptions
            {
            }
        }

        [Verb("ping")]
        public class Ping
        {
            [Option(0)]
            public IPEndPoint EndPoint { get; set; }
        }
    }

    public class Program
    {
        public static void Ping(Network.Ping options)
        {
            Console.WriteLine($"Ping {options.EndPoint}");
        }

        public static void Add(Network.IPDatabase.Add options)
        {
            Console.WriteLine($"Add {options.IP}");
        }
        
        public static void Remove(Network.IPDatabase.Remove options)
        {
            Console.WriteLine($"Add {options.IP}");
        }

        public static void Clear(Network.IPDatabase.Clear options)
        {
            Console.WriteLine("Clear");
        }

        public static IPEndPoint ParseIPEndPoint(string str)
        {
            string[] split = str.Split(':');
            IPAddress ip = IPAddress.Parse(split[0]);
            int port = int.Parse(split[1]);
            return new IPEndPoint(ip, port);
        }
        
        public static void Main(string[] args)
        {
            Parser.Create()
              .AddVerb<Network.Ping>(Ping)
              .AddVerb<Network.IPDatabase.Add>(Add)
              .AddVerb<Network.IPDatabase.Remove>(Remove)
              .AddVerb<Network.IPDatabase.Clear>(Clear)
              .AddParser(IPAddress.Parse)
              .AddParser(ParseIPEndPoint)
              .Process(new[] { "net", "ping", "127.0.0.1:8080" });
        }
    }
}
