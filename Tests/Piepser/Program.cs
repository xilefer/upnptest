using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piepser
{
    class Program
    {
        static void Main(string[] args)
        {
            while (Console.ReadLine() != "start")
            {
                Console.WriteLine("Ich habe sie nicht verstanden!");    
            }
            do
            {
                Song();
            } while (Console.ReadLine() != "N");



        }

        public static void Song()
        {
            Peep(1000, 500);
            Wait(500);
            Peep(1200, 500);
            Wait(500);
            Peep(1300, 500);
            Wait(500);
            Peep(1000, 250);
            Wait(250);
            Peep(500, 1000);
            Wait(500);
            Console.WriteLine("Nochmal?(Y/N)");
        }


        public static void Peep(int nFrequency, int nLength)
        {
            Console.Beep(nFrequency, nLength);
        }

        public static void Wait(int nLength)
        {
            System.Threading.Thread.Sleep(nLength);
        }
    }
}
