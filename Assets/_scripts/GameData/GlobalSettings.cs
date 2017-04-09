using UnityEngine;
using UnityEngine.Events;

namespace GameData
{
    public partial class GlobalSettings : ResourceSingleton<GlobalSettings> {

    #if UNITY_EDITOR

        [UnityEditor.MenuItem("Tools/Global Settings")]
        public static void SelectAsset()
        {
            UnityEditor.Selection.activeObject = instance;
        }
    #endif

        public static readonly UnityEvent OnChanged = new UnityEvent();

        private void OnValidate()
        {
            OnChanged.Invoke();
        }
    }
}
