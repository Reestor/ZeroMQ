using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;

namespace Server
{
    static partial class Program
    {
        public static void Main(string[] args)
        {

            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Сервер\n");
                args = new string[] { null };
            }

            using (var context = new ZContext())
            using (var publisher = new ZSocket(context, ZSocketType.PUB))
            {
                if (args[0] != null)
                {
                    publisher.Connect(args[0]);
                }
                else
                {
                    publisher.Bind("tcp://192.168.1.33:8005");
                }

                var rnd = new Random();

                while (true)
                {
                    publisher.SendMore(new ZFrame(string.Format("{0:D3}", rnd.Next(10))));
                    publisher.Send(new ZFrame("Здравствуйте клиент!"));
                    Console.WriteLine("Отправка сообщения");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}