using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetPlaceableZone))]
public class TargetPlaceableZoneEditor : Editor
{
    TargetPlaceableZone zone;

    void OnEnable()
    {
        zone = (TargetPlaceableZone)target;
        if (zone.localPoints.Count == 0)
            zone.ResetZone();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reset to Square"))
        {
            Undo.RecordObject(zone, "Reset Target Zone");
            zone.ResetZone();
        }
    }

    void OnSceneGUI()
    {
        Transform t = zone.transform;
        Handles.color = Color.blue;

        for (int i = 0; i < zone.localPoints.Count; i++)
        {
            Vector3 worldPos = t.TransformPoint(zone.localPoints[i]);
            EditorGUI.BeginChangeCheck();
            var fmh_36_68_638863361541504175 = Quaternion.identity; Vector3 newWorldPos = Handles.FreeMoveHandle(worldPos, 10f, Vector3.zero, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(zone, "Move Target Zone Point");
                zone.localPoints[i] = t.InverseTransformPoint(newWorldPos);
            }

            Handles.DrawLine(worldPos, t.TransformPoint(zone.localPoints[(i + 1) % zone.localPoints.Count]));
        }
    }
}
