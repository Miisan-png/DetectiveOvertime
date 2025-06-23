using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject unpackButton;
    public GameObject propsGroup;
    public GameObject clueCasePanel;
    public GameObject finishButton;
    public int totalClues = 3;

    int foundClues = 0;

    void Start()
    {
        propsGroup.SetActive(false);
        clueCasePanel.SetActive(false);
        finishButton.SetActive(false);
    }

    public void OnUnpack()
    {
        propsGroup.SetActive(true);
        clueCasePanel.SetActive(true);
        unpackButton.SetActive(false);
    }

    public void ClueFound(GameObject clueObject)
    {
        clueObject.SetActive(false);
        foundClues++;
        if (foundClues >= totalClues)
        {
            finishButton.SetActive(true);
        }
    }
}
