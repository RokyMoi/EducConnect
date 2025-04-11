using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EduConnect.Utilities
{
    public class PrintObjectUtility
    {
        public static void PrintObjectProperties(object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Object is null");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Console.WriteLine($"Properties of {type.Name}:");
            foreach (var property in properties)
            {
                object value = property.GetValue(obj) ?? "null";
                Console.WriteLine($"{property.Name}: {value}");
            }
        }
    }
}