using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;

namespace LWTML
{
    public class AssemblyLoader
    {
        private static readonly Dictionary<string, Assembly> AssemblyCache = new Dictionary<string, Assembly>();

        public static void LoadAssemblies(Assembly terrariaAssembly)
        {
            foreach (var resource in terrariaAssembly.GetManifestResourceNames())
            {
                if (!resource.EndsWith(".dll")) continue;
                
                var resourceStream = terrariaAssembly.GetManifestResourceStream(resource);

                if (resourceStream == null) continue;

                var memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);
                    
                var assemblyData = memoryStream.GetBuffer();
                var assembly = Assembly.Load(assemblyData);
                    
                AssemblyCache.Add(assembly.GetName().FullName, assembly);
                    
                Console.WriteLine($"Loaded assembly: {assembly.GetName().Name} version: {assembly.GetName().Version}");
            }

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                AssemblyCache.TryGetValue(args.Name, out var assembly);
                
                return assembly;
            };
        }
    }
}