using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceableZone))]
public class PlaceableZoneEditor : Editor
{
    PlaceableZone zone;

    void OnEnable()
    {
        zone = (PlaceableZone)target;
        if (zone.localPoints.Count == 0)
            zone.ResetZone();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reset to Square"))
        {
            Undo.RecordObject(zone, "Reset Placeable Zone");
            zone.ResetZone();
        }
    }

    void OnSceneGUI()
    {
        Transform t = zone.transform;
        Handles.color = Color.green;

        for (int i = 0; i < zone.localPoints.Count; i++)
        {
            Vector3 worldPos = t.TransformPoint(zone.localPoints[i]);
            EditorGUI.BeginChangeCheck();
            var fmh_36_68_638863330544733975 = Quaternion.identity; Vector3 newWorldPos = Handles.FreeMoveHandle(worldPos, 10f, Vector3.zero, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(zone, "Move Zone Point");
                zone.localPoints[i] = t.InverseTransformPoint(newWorldPos);
            }

            Handles.DrawLine(worldPos, t.TransformPoint(zone.localPoints[(i + 1) % zone.localPoints.Count]));
        }
    }
}
