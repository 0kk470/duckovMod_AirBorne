
using Duckov.Scenes;
using UnityEngine;

namespace Airborne
{

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {

        void Awake()
        {
            AssetManager.Instance.Initialize();
            LevelManager.OnAfterLevelInitialized += SceneLoader_onAfterSceneInitialize;
        }


        void OnDestroy()
        {
            AssetManager.Instance.Uninitialize();
            LevelManager.OnAfterLevelInitialized -= SceneLoader_onAfterSceneInitialize;
        }

        private void SceneLoader_onAfterSceneInitialize()
        {
            var mapId = MultiSceneCore.Instance.SceneInfo.ID;

            Debug.Log($"Airborne Mod: Scene initialized with Map ID: {mapId}");

            if (!MapHelper.IsParachuteCompatible(mapId))
                return;

            var player = LevelManager.Instance.MainCharacter;

            InitAirborne(player);
        }

        private void InitAirborne(CharacterMainControl player)
        {
            if (player == null)
            {
                Debug.LogError("Airborne Mod: MainCharacter is null!");
                return;
            }

            var go = AssetManager.Instance.CreateFromPath("Assets/Airborne/Jet.prefab");
            if (go == null)
            {
                Debug.LogError("Airborne Mod: Failed to create AirPlane prefab!");
                return;
            }



            AirPlane airPlane = go.AddComponent<AirPlane>();
            var path = MapHelper.GenerateFlightPath();
            airPlane.BeginFly(path.startPos, path.endPos, player);
        }
    }
}