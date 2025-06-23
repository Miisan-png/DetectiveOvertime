using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TargetPlaceableZone : MonoBehaviour
{
    public List<Vector2> localPoints = new List<Vector2>();

    public bool ContainsPoint(Vector2 localPos)
    {
        int count = localPoints.Count;
        bool inside = false;
        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            Vector2 pi = localPoints[i];
            Vector2 pj = localPoints[j];
            if (((pi.y > localPos.y) != (pj.y > localPos.y)) &&
                (localPos.x < (pj.x - pi.x) * (localPos.y - pi.y) / (pj.y - pi.y) + pi.x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    public void ResetZone()
    {
        localPoints.Clear();
        localPoints.Add(new Vector2(-50, -50));
        localPoints.Add(new Vector2(50, -50));
        localPoints.Add(new Vector2(50, 50));
        localPoints.Add(new Vector2(-50, 50));
    }
}