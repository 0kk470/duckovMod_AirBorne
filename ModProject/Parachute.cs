using UnityEngine;

namespace Airborne
{
    public class Parachute : MonoBehaviour
    {
        private CharacterMainControl m_Character;

        void Update()
        {
            var assetPaths = Airborne.AssetManager.Instance.GetAssetPathsInBundle("airborne");
            foreach(var assetPath in assetPaths)
            {
                Debug.Log(assetPath);
            }
        }
    }
}
