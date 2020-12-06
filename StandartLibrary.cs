using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class StandardLibrary
    {
        public static double Plus(double x, double y)
        {
            return x + y;
        }

        public static double Minus(double x, double y)
        {
            return x - y;
        }

        public static bool More(double x, double y)
        {
            return x > y;
        }

        public static bool Equal(double x, double y)
        {
            return x == y;
        }

        public static bool Or(bool x, bool y)
        {
            return x || y;
        }

        public static bool And(bool x, bool y)
        {
            return x && y;
        }
    }
}
