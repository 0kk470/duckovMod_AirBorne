using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZhanGuoWuxia.Editor
{

    public class BundleBuilder : MonoBehaviour
    {
        [MenuItem("ModBundle/AB_Win64", false, 3000)]
        private static void BuildABWin()
        {
            BuildBundlesInStreamingAssets(BuildTarget.StandaloneWindows64);
        }

        private static void BuildBundlesInStreamingAssets(BuildTarget targetPlatform)
        {
            ClearUnUsedBundleNames();
            InitStreamingAssetsFolder();
            GenerateModBundles(targetPlatform);
            EditorUtility.DisplayDialog("Tip", "Generate UnityBundle successfully", "Ok");
        }

        private static void GenerateModBundles(BuildTarget targetPlatform)
        {
            var prevFolder = EditorPrefs.GetString("XfgModBundleOutputPath", Application.dataPath);
            var folder = EditorUtility.OpenFolderPanel("Choose Bundle Output Path", prevFolder, "");
            EditorPrefs.SetString("XfgModBundleOutputPath", folder);
            if(string.IsNullOrEmpty(folder) || !Directory.Exists(folder))
            {
                EditorUtility.DisplayDialog("Error", "Please select a valid folder", "OK");
                return;
            }
            var manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, targetPlatform);
            if (manifest != null)
            {
                CopyAllAssetBundles(Application.streamingAssetsPath, folder);
            }
        }

        private static void InitStreamingAssetsFolder()
        {
            if(!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
        }


        private static void ClearUnUsedBundleNames()
        {
            var obsoleteNames = AssetDatabase.GetUnusedAssetBundleNames();
            if (obsoleteNames == null || obsoleteNames.Length == 0)
                return;
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        private static void CopyAllAssetBundles(string sourceFolder, string desFolder)
        {
            if (!Directory.Exists(desFolder))
                Directory.CreateDirectory(desFolder);

            var allBundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundleName in allBundleNames)
            {
                DoCopyTargetBundle(sourceFolder, desFolder, bundleName);
            }
            var bundleConfig = new BundleConfig(desFolder, allBundleNames);
            bundleConfig.SaveToFile();
        }

        private static void DoCopyTargetBundle(string sourceFolder, string desFolder, string bundleName)
        {
            var abPath = Path.Combine(sourceFolder, bundleName);
            var abDestPath = Path.Combine(desFolder, bundleName);
            if (!File.Exists(abPath))
            {
                Debug.LogError("The Source assetbundle does not exist!!!, Path:" + abPath);
                return;
            }
            abDestPath = abDestPath.WithExtension(".assetbundle");
            File.Copy(abPath, abDestPath, true);
        }
    }
}
