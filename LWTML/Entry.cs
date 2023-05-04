using System;
using System.Reflection;
using Terraria;

namespace LWTML
{
    public class Entry
    {
        public static int Main(string[] args)
        {
            var terrariaAssembly = Assembly.GetAssembly(typeof(Main));
            
            AssemblyLoader.LoadAssemblies(terrariaAssembly);

            var loader = new Loader();

            Console.WriteLine("Loading mods");

            try
            {
                loader.LoadMods();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("An error occured during mod loading");
                Console.Error.WriteLine(exception);

                return 1;
            }
            
            Console.WriteLine("Initializing mods");

            try
            {
                loader.Init();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("An error occured during mod initialization");
                Console.Error.WriteLine(exception);

                return 1;
            }
            
            Console.WriteLine("Launching Terraria");

            try
            {
                terrariaAssembly.EntryPoint?.Invoke(null, new [] {args});
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Terraria has crashed!");
                Console.Error.WriteLine(exception);

                return 1;
            }
            
            Console.WriteLine("Terraria exited");
            
            loader.Exit();

            return 0;
        }
    }
}