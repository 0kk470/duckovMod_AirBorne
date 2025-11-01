using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ZhanGuoWuxia.Editor
{
    public class BundleConfig
    {
        private const string ConfigFileName = "bundles.json";

        [JsonIgnore]
        public string ConfigPath => Path.Combine(m_Folder, ConfigFileName);

        [JsonIgnore]
        private string m_Folder = string.Empty;

        [JsonProperty("bundles")]
        public HashSet<string> AssetBundles { get; } = new HashSet<string>();

        public BundleConfig() { }

        public BundleConfig(string folder, IEnumerable<string> bundleNames)
        {
            m_Folder = folder;
            AssetBundles.UnionWith(bundleNames);
        }

        public void SaveToFile()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var outputPath = ConfigPath.NormalizePath();
            UnityEngine.Debug.Log(outputPath);
            File.WriteAllText(outputPath, json);
        }

        public IEnumerable<string> GetAssetBundlePaths()
        {
            if (string.IsNullOrEmpty(m_Folder))
                yield break;
            foreach (var bundleName in AssetBundles)
            {
                yield return Path.Combine(m_Folder, bundleName.WithExtension(".assetbundle"));
            }
        }
    }
}
