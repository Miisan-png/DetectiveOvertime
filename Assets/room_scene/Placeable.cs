using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Placeable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public PlaceableZone[] zones;
    public TargetPlaceableZone[] targetZones;
    public TargetPlaceableZone[] finalDropZones;
    public Image highlight;
    public GameObject placedPrefab;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public Color targetColor = Color.blue;
    public Color finalColor = Color.yellow;
    public Color glowColor = new Color(1f, 0.7f, 0.2f);

    RectTransform rt;
    Canvas canvas;
    Vector3 offset;
    bool dragging;
    bool glowing;
    bool finalDropped;
    PlaceableZone lastValidZone;
    Image img;
    bool cluePlaced;
    CluePlaceable clueComponent;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        img = GetComponent<Image>();
        clueComponent = GetComponent<CluePlaceable>();
        if (placedPrefab != null) placedPrefab.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance.AllCluesPlaced() && clueComponent != null && clueComponent.isPlacedInClueZone && !finalDropped)
        {
            glowing = true;
        }

        if (glowing && clueComponent != null && clueComponent.isPlacedInClueZone && img != null && !finalDropped)
        {
            float glow = Mathf.PingPong(Time.time, 1f);
            img.color = Color.Lerp(Color.white, glowColor, glow);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (finalDropped) return;
        dragging = true;
        if (highlight != null) highlight.color = Color.white;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var local);
        offset = rt.localPosition - (Vector3)local;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || finalDropped) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var local);
        rt.localPosition = (Vector3)local + offset;

        foreach (var zone in finalDropZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                highlight.color = finalColor;
                return;
            }
        }

        foreach (var zone in targetZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                highlight.color = targetColor;
                return;
            }
        }

        foreach (var zone in zones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                highlight.color = validColor;
                lastValidZone = zone;
                return;
            }
        }

        highlight.color = invalidColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        foreach (var zone in finalDropZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint) && clueComponent != null && glowing)
            {
                finalDropped = true;
                if (placedPrefab != null)
                {
                    placedPrefab.SetActive(false);
                }
                Destroy(gameObject);
                GameManager.Instance.ReportFinalClue();
                return;
            }
        }

        foreach (var zone in targetZones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                if (placedPrefab != null)
                {
                    placedPrefab.SetActive(true);
                    placedPrefab.transform.SetAsLastSibling();
                    clueComponent = placedPrefab.GetComponent<CluePlaceable>();
                    if (clueComponent != null) clueComponent.isPlacedInClueZone = true;
                }
                gameObject.SetActive(false);
                return;
            }
        }

        if (clueComponent != null && !cluePlaced)
        {
            foreach (var zone in targetZones)
            {
                Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
                if (zone.ContainsPoint(localPoint))
                {
                    clueComponent.isPlacedInClueZone = true;
                    cluePlaced = true;
                    return;
                }
            }
        }

        bool inZone = false;
        foreach (var zone in zones)
        {
            Vector2 localPoint = zone.transform.InverseTransformPoint(rt.position);
            if (zone.ContainsPoint(localPoint))
            {
                inZone = true;
                break;
            }
        }

        if (!inZone && lastValidZone != null)
        {
            rt.position = lastValidZone.transform.position;
        }

        highlight.color = inZone ? Color.white : invalidColor;
    }
}
