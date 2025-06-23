using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> placeables;
    public CluePlaceable[] clueObjects;
    public GameObject completeCanvas;


    int finalCluesDelivered = 0;

    void Awake()
    {
        Instance = this;
        completeCanvas.SetActive(false);
    }

    void Update()
    {
        if (AllCluesGlowing() && finalCluesDelivered == 0)
        {
            foreach (var clue in clueObjects)
            {
                var p = clue.GetComponent<Placeable>();
                if (p != null)
                    p.enabled = true; // Reactivate for second phase
            }
        }
    }

    public void ReportFinalClue()
    {
        finalCluesDelivered++;
        if (finalCluesDelivered >= clueObjects.Length)
        {
            completeCanvas.SetActive(true);
        }
    }

    bool AllCluesGlowing()
    {
        foreach (var clue in clueObjects)
        {
            if (!clue.isPlacedInClueZone) return false;
        }
        return true;
    }

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
    
    public bool AllCluesPlaced()
{
    foreach (var clue in clueObjects)
    {
        if (!clue.isPlacedInClueZone) return false;
    }
    return true;
}

}
