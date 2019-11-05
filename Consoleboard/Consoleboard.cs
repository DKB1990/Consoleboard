using System;

namespace Consoleboard
{
    public class Consoleboard
    {
        /// <summary>
        /// The main point from where the application starts.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        static void Main(string[] args)
        {
            EventHandler board = new EventHandler();
            Console.ReadKey();
        }   
    }
}
