using LR01.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR01.Controller
{
    public class Terminal
    {
        public static void Write(IdentificatorRow val)
        {
            Console.WriteLine($"{val.Name}={val.Value}");
        }

        public static void Read(ref IdentificatorRow val)
        {
            Console.Write($"{val.Name}=");
            string str = Console.ReadLine();

            string sPattern = "^[a-zE0-9\\.\\+\\-]*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(str, sPattern) != true)
            {
                throw new Exception("Error, недопустимий формат вводу");
            }
            val.Value = str;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
