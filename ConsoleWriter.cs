using System;
using System.Runtime.InteropServices;

namespace WebCam
{
    class ConsoleWriter
    {
        public ConsoleWriter()
        {
            AllocConsole();
        }
        
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void ReadLine()
        {
            Console.ReadLine();
        }
    }
}
