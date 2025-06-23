using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Placeable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public PlaceableZone[] zones;
    public TargetPlaceableZone[] targetZones;
    public GameObject placedPrefab;
    public Image highlight;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public Color targetColor = Color.blue;

    RectTransform rt;
    Canvas canvas;
    Vector3 offset;
    bool dragging;
    bool isPlaced;
    PlaceableZone lastValidZone;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        gameObject.SetActive(false);
        if (placedPrefab != null) placedPrefab.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(PopIn());
    }

    System.Collections.IEnumerator PopIn()
    {
        Vector3 startPos = rt.localPosition;
        Vector3 targetPos = startPos + Vector3.up * 100f;
        Vector3 startScale = Vector3.one * 0.3f;
        Vector3 endScale = Vector3.one;
        float time = 0f;
        rt.localScale = startScale;
        while (time < 1f)
        {
            time += Time.deltaTime * 4f;
            rt.localPosition = Vector3.Lerp(startPos, targetPos, time);
            rt.localScale = Vector3.Lerp(startScale, endScale, time);
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPlaced = false;
        dragging = true;
        if (highlight != null) highlight.color = Color.white;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var local);
        offset = rt.localPosition - (Vector3)local;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var local);
        rt.localPosition = (Vector3)local + offset;

        bool inTarget = false;
        foreach (var zone in targetZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                highlight.color = targetColor;
                inTarget = true;
                return;
            }
        }

        bool inNormal = false;
        foreach (var zone in zones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                highlight.color = validColor;
                lastValidZone = zone;
                inNormal = true;
                break;
            }
        }

        if (!inNormal) highlight.color = invalidColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        foreach (var zone in targetZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                SpawnPlaced();
                return;
            }
        }

        bool insideNormal = false;
        foreach (var zone in zones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                insideNormal = true;
                break;
            }
        }

        if (insideNormal)
        {
            isPlaced = true;
            if (highlight != null) highlight.color = Color.white;
        }
        else if (lastValidZone != null)
        {
            rt.position = lastValidZone.transform.position;
            if (highlight != null) highlight.color = Color.white;
        }
        else
        {
            if (highlight != null) highlight.color = invalidColor;
        }
    }

    void SpawnPlaced()
    {
        if (placedPrefab != null)
        {
            GameObject placed = Instantiate(placedPrefab, placedPrefab.transform.position, placedPrefab.transform.rotation, transform.parent);
            placed.transform.localScale = placedPrefab.transform.localScale;
            placed.SetActive(true);
        }
        Destroy(gameObject);
    }
}
