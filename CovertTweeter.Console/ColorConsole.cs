using System;

namespace CovertTweeter
{
    public static class ColorConsole
    {
        private static readonly object Sync = new object();
        private static ConsoleColor _defaultColor = ConsoleColor.White;

        public static ConsoleColor Default { get { return _defaultColor; } set { _defaultColor = value; } }

        public static void Write(ConsoleColor colour, string text, params object[] args)
        {
            lock (Sync)
            {
                Console.ForegroundColor = colour;
                Console.Write(text, args);
            }
        }

        public static void WriteLine(ConsoleColor color, string text, params object[] args)
        {
            Write(color, text + '\n', args);
        }        

        public static void WriteLine(string text, params object[] args)
        {
            Write(text + '\n', args);
        }

        public static void Write(string text, params object[] args)
        {
            Write(_defaultColor, text, args);
        }

        public static string ReadLine(ConsoleColor color)
        {
            lock (Sync)
            {
                Console.ForegroundColor = color;
                return Console.ReadLine();
            }
        }

        public static string ReadLine()
        {
            return ReadLine(_defaultColor);
        }

        public static void Newline(int count = 1)
        {
            if(count<0) throw new ArgumentOutOfRangeException("NewLine count cannot be a negative number","Count value: " + count);
            Console.Write("".PadRight(count,'\n'));
        }
    }
}