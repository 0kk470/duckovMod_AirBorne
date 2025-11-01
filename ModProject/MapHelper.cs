using Duckov.MiniMaps;
using Duckov.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Airborne
{
    internal class MapHelper
    {
        public static Vector3 GetMapBounds()
        {
            var miniMapSettings = MiniMapSettings.Instance;
            if (miniMapSettings != null)
            {
                float mapSize = miniMapSettings.combinedSize;
                Vector3 mapCenter = miniMapSettings.combinedCenter;
                return new Vector3(mapSize, 0, mapSize);
            }

            var miniMapCenter = GameObject.FindObjectOfType<MiniMapCenter>();
            if (miniMapCenter != null && miniMapCenter.WorldSize > 0)
            {
                float worldSize = miniMapCenter.WorldSize;
                return new Vector3(worldSize, 0, worldSize);
            }

            // 默认值
            return new Vector3(1000, 0, 1000);
        }

        public static Vector3 GetMapCenter()
        {
            MiniMapSettings instance = MiniMapSettings.Instance;
            if (instance == null)
            {
                return Vector3.zero;
            }

            return instance.combinedCenter;
        }

        // 生成飞机路径
        public static (Vector3 startPos, Vector3 endPos) GenerateFlightPath()
        {
            Vector3 mapBounds = GetMapBounds();
            Vector3 mapCenter = GetMapCenter();

            // 在地图边界外生成起始点和结束点
            float margin = 200f; // 边界外的距离
            float flightHeight = 500f; // 飞行高度

            // 随机选择飞行方向
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            // 计算起始点和结束点
            float halfSize = Mathf.Max(mapBounds.x, mapBounds.z) * 0.5f;
            Vector3 startPos = mapCenter - direction * (halfSize + margin);
            Vector3 endPos = mapCenter + direction * (halfSize + margin);

            // 设置高度
            startPos.y = flightHeight;
            endPos.y = flightHeight;

            return (startPos, endPos);
        }

        // 适合跳伞的室外地图列表
        private static readonly string[] PARACHUTE_COMPATIBLE_MAPS = new string[]
        {
            "Level_GroundZero_Main",      //零号区
            "Level_HiddenWarehouse_Main", //仓库
            "Level_Farm_Main",            //农场
            "Level_StormZone_Main"        //风暴区
        };

        // 需要排除的室内地图
        private static readonly string[] INDOOR_MAPS = new string[]
        {
            "Base",                         // 玩家基地
            "Level_JLab_Main",              // 实验室

        };

        public static bool IsParachuteCompatible(string sceneID)
        {
            // 检查是否为基地
            if (LevelConfig.IsBaseLevel)
                return false;

            if (INDOOR_MAPS.Contains(sceneID))
                return false;

            // 检查SubSceneEntry的isInDoor属性
            var subSceneInfo = MultiSceneCore.Instance?.GetSubSceneInfo();
            if (subSceneInfo != null && subSceneInfo.IsInDoor)
            {
                Debug.Log($"Airborne Mod: Scene '{sceneID}' is marked as indoor in SubSceneInfo.");
                return false;
            }

            return PARACHUTE_COMPATIBLE_MAPS.Contains(sceneID);
        }
    }
}
