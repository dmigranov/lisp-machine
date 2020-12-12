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

        public static double Multiply(double x, double y)
        {
            return x * y;
        }

        public static double Divide(double x, double y)
        {
            return x / y;
        }

        public static bool More(double x, double y)
        {
            return x > y;
        }

        public static bool MoreEqual(double x, double y)
        {
            return x >= y;
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

        public static string Println(object x)
        {
            string ret;
            if(x != null)
                ret =  x.ToString();
            else 
                ret = "nullptr/void";
            Console.WriteLine(ret);
            return ret;
        }

        public static int Count(List<object> arr)
        {
            return arr.Count;
        }

        public static List<object> Cons(object x, List<object> list)
        {
            list.Insert(0, x);
            return list;
        }
    }
}
