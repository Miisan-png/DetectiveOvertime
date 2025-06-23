using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RawImage iconLeft;
    public RawImage iconRight;
    public TextMeshProUGUI text;

    public Color defaultColor = Color.white;
    public Color hoverColor = Color.cyan;
    public float hoverOffset = 50f;
    public float moveSpeed = 10f;
    public float popScale = 1.2f;
    public float popSpeed = 8f;

    public UnityEvent onClick;

    Vector3 leftOriginalPos;
    Vector3 rightOriginalPos;
    bool isHovering;
    Vector3 targetScale;
    Vector3 originalScale;
    RawImage[] icons;

    void Start()
    {
        leftOriginalPos = iconLeft.rectTransform.anchoredPosition;
        rightOriginalPos = iconRight.rectTransform.anchoredPosition;
        originalScale = transform.localScale;
        targetScale = originalScale;

        icons = new RawImage[] { iconLeft, iconRight };
        SetColor(defaultColor);
    }

    void Update()
    {
        Vector3 leftTarget = isHovering ? leftOriginalPos + Vector3.left * hoverOffset : leftOriginalPos;
        Vector3 rightTarget = isHovering ? rightOriginalPos + Vector3.right * hoverOffset : rightOriginalPos;

        iconLeft.rectTransform.anchoredPosition = Vector3.Lerp(iconLeft.rectTransform.anchoredPosition, leftTarget, Time.deltaTime * moveSpeed);
        iconRight.rectTransform.anchoredPosition = Vector3.Lerp(iconRight.rectTransform.anchoredPosition, rightTarget, Time.deltaTime * moveSpeed);

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * popSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        SetColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        SetColor(defaultColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetScale = originalScale * popScale;
        StopAllCoroutines();
        StartCoroutine(ResetScale());
        if (onClick != null) onClick.Invoke();
    }

    System.Collections.IEnumerator ResetScale()
    {
        yield return new WaitForSeconds(0.1f);
        targetScale = originalScale;
    }

    void SetColor(Color color)
    {
        if (text != null) text.color = color;
        foreach (var icon in icons)
            if (icon != null) icon.color = color;
    }
}
