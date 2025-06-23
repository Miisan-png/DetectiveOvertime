using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<GameObject> placeables;

    public void SpawnRandomItem()
    {
        if (placeables.Count == 0) return;
        int index = Random.Range(0, placeables.Count);
        GameObject obj = placeables[index];
        placeables.RemoveAt(index);
        obj.SetActive(true);
        StartCoroutine(PopIn(obj.transform));
    }

    System.Collections.IEnumerator PopIn(Transform t)
    {
        Vector3 start = t.localPosition;
        Vector3 target = start + Vector3.up * 100f;
        float scale = 0.2f;
        t.localScale = Vector3.one * scale;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 4f;
            t.localPosition = Vector3.Lerp(start, target, time);
            t.localScale = Vector3.Lerp(Vector3.one * scale, Vector3.one, time);
            yield return null;
        }
    }
}
