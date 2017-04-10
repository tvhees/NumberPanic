using Managers;
using UnityEditor;
using UnityEngine;

namespace GameStates
{
    [CustomEditor(typeof(State))]
    public class StateEditor : Editor {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying)
                return;

            var state = target as State;
            if (state == null)
                return;

            var oldColor = GUI.color;
            GUI.color = Color.red;

            // Draw the custom "Current State" field...
            var newState = EditorGUILayout.ObjectField(
                "CurrentState", state.Current,
                typeof(GameObject), true ) as GameObject;

            // And execute the change state code if
            // we've received a new game object
            if ( newState != state.Current )
            {
                state.GetManager<StateManager>().MoveToState(newState);
            }

            // Restore the old color so the
            // GUI doesn't stay red
            GUI.color = oldColor;
        }
    }
}
