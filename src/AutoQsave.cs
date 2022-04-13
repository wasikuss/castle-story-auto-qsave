using System;
using System.Collections;
using Brix.Assets;
using Brix.Game;
using Smooth.Slinq;
using UnityEngine;

namespace CastleStory_AutoQsave
{
    class AutoQsave
    {
        static private AutoQsave _instance;
        public static AutoQsave Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AutoQsave();
                }

                return _instance;
            }
        }

        private AutoQsave() { }

        private AutoQsaveConfig config;

        public void Start(AutoQsaveConfig config)
        {
            this.config = config;
            Plugin.Instance.StopAllCoroutines();
            if (config.interval == TimeSpan.Zero)
            {
                return;
            }
            Plugin.Instance.StartCoroutine(DoSave(((float)config.interval.TotalSeconds)));
        }

        private IEnumerator DoSave(float interval)
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(interval);

                try
                {
                    Debug.Log("Starting auto quick save.");
                    StatsTracker.SaveStats();
                    Asset_Map.SaveType saveType = config.type == AutoQsaveType.Auto ? Asset_Map.SaveType.Autosave : Asset_Map.SaveType.Quicksave;
                    Asset_Map asset_Map = AssetDispensary.saves.SendSaveQuery(saveType).Slinq<Asset_Map>().FirstOrDefault();
                    if (asset_Map == null)
                    {
                        asset_Map = Asset_Map.CreateShellFrom(GameParam.Map, Asset_Map.MapType.Save);
                        if (config.type == AutoQsaveType.Auto)
                        {
                            asset_Map.codename = "autoquicksave";
                            asset_Map.customName = "--Auto Quicksave--";
                        }
                        else
                        {
                            asset_Map.codename = "quicksave";
                            asset_Map.rawDisplayName = "--Quicksave--";
                            asset_Map.customName = "--Quicksave--";
                        }
                        asset_Map.saveType = saveType;
                        asset_Map.Disk_WriteAsNew(AssetAuthorType.Official);
                    }
                    else
                    {
                        asset_Map.isDebug = GameParam.Map.isDebug;
                        asset_Map.Disk_Overwrite();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    break;
                }
            }
        }
    }
}
