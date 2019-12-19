using System;

namespace Stone
{
    class Program
    {
        static void Main(string[] args)
        {
            var evn = StoneAPI.CreatEnv();
            string input;
            while (true)
            {
                input = Console.ReadLine();
                var value = StoneAPI.DoScript(evn, input);
                if (value != null)
                    Console.WriteLine(value);
            }

        }
    }
}
