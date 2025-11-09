using Duckov;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Airborne
{
    public static class AudioHelper
    {
        private static string ModFolder => System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void PlaySFX(string fileName, GameObject? attachObject = default, bool isLoop = false)
        {
            var filePath = Path.Combine(ModFolder, fileName);
            Debug.LogFormat("Airborne: Playing SFX from [{0}]",filePath);
            AudioManager.PostCustomSFX(filePath, attachObject, isLoop);
        }
    }
}
