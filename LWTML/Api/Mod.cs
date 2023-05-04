using System;
using System.Reflection;
using HarmonyLib;

namespace LWTML.Api
{
    internal class Mod
    {
        public ModEntry Entry { get; private set; }
        
        public Harmony Harmony { get; private set; }
        public Assembly Assembly { get; private set;  }
        
        public ModConfiguration Configuration { get; private set; }
        
        public Mod(Assembly assembly, ModConfiguration configuration)
        {
            Assembly = assembly;
            Configuration = configuration;

            if (configuration.UseHarmony) Harmony = new Harmony(assembly.GetName().Name);

            Entry = (ModEntry) assembly
                .GetType(configuration.EntryPoint)
                .GetConstructor(new Type[] {})
                ?.Invoke(new object[] {});
        }
    }
}