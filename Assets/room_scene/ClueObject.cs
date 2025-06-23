using UnityEngine;
using UnityEngine.EventSystems;

public class ClueObject : MonoBehaviour, IPointerClickHandler
{
    public GameManager manager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager != null)
        {
            manager.ClueFound(gameObject);
        }
    }
}
