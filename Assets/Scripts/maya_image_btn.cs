using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class maya_image_btn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.cyan;
    public float hoverScale = 1.05f;
    public float popScale = 1.2f;
    public float scaleSpeed = 8f;

    public UnityEvent onClick;

    Vector3 originalScale;
    Vector3 targetScale;
    RawImage image;
    bool isHovering;

    void Start()
    {
        image = GetComponent<RawImage>();
        originalScale = transform.localScale;
        targetScale = originalScale;
        if (image != null) image.color = defaultColor;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        targetScale = originalScale * hoverScale;
        if (image != null) image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        targetScale = originalScale;
        if (image != null) image.color = defaultColor;
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
        targetScale = isHovering ? originalScale * hoverScale : originalScale;
    }
}
