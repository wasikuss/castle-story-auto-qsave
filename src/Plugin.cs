using System;
using System.IO;
using BepInEx;
using HarmonyLib;

namespace CastleStory_AutoQsave
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin _instance;
        public static Plugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;

            // ensure modconf folder exists
            Directory.CreateDirectory("Info/Lua/modconf");

            new Harmony("com.wasikuss.castlestory.autoqsave").PatchAll();
        }
    }
}
