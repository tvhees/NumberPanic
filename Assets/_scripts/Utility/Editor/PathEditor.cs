using Utility;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    private Path path;
    private Transform pathTransform;
    private Quaternion pathRotation;

    private int selectedIndex = -1;

    private void GetPath()
    {
        path = target as Path;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GetPath();
        if (path == null)
        {
            return;
        }

        if (GUILayout.Button("Add Point")) {
            Undo.RecordObject(path, "Add Point");
            path.AddPoint();
            EditorUtility.SetDirty(path);
        }
    }

    private void OnSceneGUI()
    {
        GetPath();

        pathTransform = path.transform;
        pathRotation = Tools.pivotRotation == PivotRotation.Local ? pathTransform.rotation : Quaternion.identity;

        var points = new Vector3[path.Points.Length];
        var segmentIndices = new int[points.Length * 2 - 2];
        var index = 0;
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = ShowPoint(i);

            if (i > 0)
            {
                segmentIndices[index++] = i;
                segmentIndices[index++] = i - 1;
            }
        }

        Handles.color = path.PathColor;
        Handles.DrawLines(points, segmentIndices);
    }

    private Vector3 ShowPoint(int index)
    {
        var point = pathTransform.TransformPoint(path.GetPoint(index));

        Handles.color = Color.white;
        if (Handles.Button(point, pathRotation, path.HandleSize, path.PickSize, Handles.SphereCap)) {
            selectedIndex = index;
        }

        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, pathRotation);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(path, "Move Point");
                EditorUtility.SetDirty(path);
                path.SetPoint(index, pathTransform.InverseTransformPoint(point));
            }
        }

        return point;
    }
}