using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;

namespace Client1
{
    static partial class Program
    {
        public static void Main(string[] args)
        {

            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Клиент Гжелин П.В.:\n");
                args = new string[] { "tcp://192.168.1.33:8005" };
            }

            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                subscriber.Connect(args[0]);

                var rnd = new Random();
                var subscription = string.Format("{0:D3}", rnd.Next(10));
                subscriber.Subscribe(subscription);

                ZMessage msg;
                ZError error;
                while (true)
                {
                    if (null == (msg = subscriber.ReceiveMessage(out error)))
                    {
                        if (error == ZError.ETERM)
                            break;  
                        throw new ZException(error);
                    }
                    using (msg)
                    {
                        if (msg[0].ReadString() != subscription)
                        {
                            throw new InvalidOperationException();
                        }
                        Console.WriteLine(msg[1].ReadString());
                    }
                }
            }
        }
    }
}