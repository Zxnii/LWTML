using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LWTML.Api;
using Newtonsoft.Json;

namespace LWTML
{
    public class Loader
    {
        private static readonly string ModDirectory = Path.Combine(MultiVersionUtil.GetStorageSubdirectory("Terraria"), "Mods");

        private readonly List<Mod> _loadedMods = new List<Mod>();
        private readonly Harmony _harmony = new Harmony("wtf.zani.lwtml");

        public void LoadMods()
        {
            Console.WriteLine($"Looking for mods in {ModDirectory}");

            var mods = CollectMods();

            foreach (var mod in mods)
            {
                var assembly = Assembly.LoadFile(mod);
                var modConfigStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.lwtml.json");
                var readerOptions = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include };

                if (modConfigStream == null) throw new LoaderException($"{assembly.GetName().Name} does not contain lwtml.json");

                var rawConfig = new StreamReader(modConfigStream).ReadToEnd();
                var configuration = JsonConvert.DeserializeObject<ModConfiguration>(rawConfig, readerOptions);

                modConfigStream.Close();

                _loadedMods.Add(new Mod(assembly, configuration));
            }

            ApplyPatches();
        }

        public void Init()
        {
            foreach (var mod in _loadedMods)
            {
                try
                {
                    mod.Entry.Init();
                }
                catch (Exception exception)
                {
                    throw new LoaderException($"An error occured while loading ${mod.Assembly.GetName()}", exception);
                }
            }
        }

        public void Exit()
        {
            foreach (var mod in _loadedMods) mod.Entry.Exit();
        }

        private void ApplyPatches()
        {
            _harmony.PatchAll();

            foreach (var mod in _loadedMods.Where(mod => mod.Configuration.UseHarmony))
                mod.Harmony.PatchAll(mod.Assembly);
        }

        private static List<string> CollectMods()
        {
            if (!Directory.Exists(ModDirectory)) Directory.CreateDirectory(ModDirectory);

            var entries = Directory.EnumerateFiles(ModDirectory);

            return (from file in entries where file.EndsWith(".dll") select Path.Combine(ModDirectory, file)).ToList();
        }
    }
}