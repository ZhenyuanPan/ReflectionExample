using System;
using System.Reflection;

namespace ReflenctionAssemblyIntro
{
    class Animal 
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Object obj = assembly.CreateInstance("ReflenctionAssemblyIntro.Animal");

        }
    }
}
