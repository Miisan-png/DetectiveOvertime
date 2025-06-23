using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CriminalManager : MonoBehaviour
{
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _confirmationUI;
    [SerializeField] private TextMeshProUGUI _confirmationText;

    private int _selectedCriminal = -1;

    public void ChooseCriminal(int criminalIndex)
    {
        // Logic to handle the selection of a criminal based on the index
        Debug.Log("Criminal chosen: " + criminalIndex);

        _selectedCriminal = criminalIndex;

        _confirmationUI.SetActive(true);

        string criminalName;
        if (criminalIndex == 0)
        {
            criminalName = "John";
        }
        else if (criminalIndex == 1)
        {
            criminalName = "Sarah";
        }
        else
        {
            criminalName = "Mike";
        }

        _confirmationText.text = $"Select {criminalName} as the criminal?";
    }

    public void ConfirmSelection()
    {
        _confirmationUI.SetActive(false);


        if (_selectedCriminal == 1)
        {
            _winUI.SetActive(true);
        }
        else
        {
            _loseUI.SetActive(true);
        }
    }

    public void CancelSelection()
    {
        _selectedCriminal = -1;
        _confirmationUI.SetActive(false);
    }

    public void ToMainMenu()
    {
        Debug.Log("To main menu");

        SceneManager.LoadScene(0);
    }
}
