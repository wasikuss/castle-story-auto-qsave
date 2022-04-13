using System.IO;
using Brix.Lua;
using HarmonyLib;
using MoonSharp.Interpreter;
using UnityEngine;

namespace CastleStory_AutoQsave
{
    [HarmonyPatch(typeof(LuaLoader), nameof(LuaLoader.LoadUserKeyBindingLuaFile))]
    public class LuaLoaderPatch
    { 
        static FileSystemWatcher watcher;
        static void Prefix()
        {
            LoadQsaveConfig();

            watcher = new FileSystemWatcher("Info/Lua/modconf");
            watcher.Filter = "auto_qsave.lua";
            watcher.Changed += (object sender, FileSystemEventArgs e) => {
                Debug.Log("Reloading auto quick save config.");
                LoadQsaveConfig();
            };
            watcher.EnableRaisingEvents = true;
        }

        static void LoadQsaveConfig() {
            Script script;
			DynValue result = LuaLoader.Load(out script, "modconf/auto_qsave.lua");
            AutoQsave.Instance.Start(AutoQsaveConfig.Parse(result.Table));
            Script.Kill(ref script);
        }
    }
}
