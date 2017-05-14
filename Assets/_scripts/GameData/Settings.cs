using UnityEngine.Events;

namespace GameData
{
    public partial class Settings : ResourceSingleton<Settings> {

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
