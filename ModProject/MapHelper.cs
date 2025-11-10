using Duckov;
using Duckov.MiniMaps;
using Duckov.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Airborne
{
    internal static class MapHelper
    {
        public static Color ToColor(this string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out var color))
            {
                return color;
            }
            return Color.white;
        }

        public static CountDownArea[] GetExitPoints()
        {
            Debug.Log("=== 正在获取撤离点位置 ===");

            // 查找场景中所有的CountDownArea组件
            var exitPoints = GameObject.FindObjectsOfType<CountDownArea>();

            return exitPoints;
        }

        public static bool TryGetFurthestExitPoint(Vector3 fromPosition, out Vector3 exitPosition)
        {
            var exitPoints = GetExitPoints();
            if (exitPoints == null || exitPoints.Length == 0)
            {
                exitPosition = Vector3.zero;
                return false;
            }

            float maxDistance = -1f;
            Vector3 furthestPoint = Vector3.zero;
            foreach (var exitPoint in exitPoints)
            {
                float distance = Vector3.Distance(fromPosition, exitPoint.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    furthestPoint = exitPoint.transform.position;
                }
            }
            exitPosition = furthestPoint;
            return true;
        }



        // 生成飞机路径
        public static bool TryGenerateFlightPath(Transform player, out (Vector3 startPos, Vector3 endPos) result) 
        {
           
            float flightHeight = 25f; // 飞行高度
            result = (Vector3.zero, Vector3.zero);
            Vector3 startPos = player.transform.position;
            startPos.y = flightHeight;
            
            if(TryGetFurthestExitPoint(startPos, out var endPos))
            {
                endPos.y = flightHeight;
                result = (startPos, endPos);
                return true;
            }

            return false;
        }

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

            return true;
        }
    }
}
