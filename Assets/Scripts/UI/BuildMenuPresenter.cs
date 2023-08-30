using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuPresenter : MonoBehaviour
{
    [SerializeField] private GameObject[] buildButtons;
    [SerializeField] private Tower tower;
    [SerializeField] private Poster poster;
    [SerializeField] private Tank tank;

    [SerializeField] private Sprite defaultBuildSprite;
    [SerializeField] private Sprite selectedBuildSprite;

    [SerializeField] private TextMeshProUGUI displayTowerCost;
    [SerializeField] private TextMeshProUGUI displayPosterCost;
    [SerializeField] private TextMeshProUGUI displayTankCost;

    public event Action<int?> onBuildingSelectionChanged;    

    private void Start()
    {
        displayTowerCost.text = "$ " + tower.Cost;
        displayPosterCost.text = "$ " + poster.Cost;
        displayTankCost.text = "$ " + tank.Cost;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            DeselectAllButtons();
            NotifyListeners(null);
        }
    }

    public void OnBuildingSelected(int index)
    {        
        DeselectAllButtons();
        SetButtonState(index);
        AudioPlayer.Instance.PlayButtonClickClip();
        NotifyListeners(index);
    }

    private void SetButtonState(int index, bool setAlltrue = false)
    {
        Button button = buildButtons[index].GetComponent<Button>();
        Image buttonImage = buildButtons[index].GetComponent<Image>();

        button.interactable = false;
        buttonImage.sprite = selectedBuildSprite;       
    }

    private void DeselectAllButtons()
    {
        foreach(var buttonGameObject in buildButtons)
        {
            Button button = buttonGameObject.GetComponent<Button>();
            Image buttonImage = buttonGameObject.GetComponent<Image>();

            button.interactable = true;
            buttonImage.sprite = defaultBuildSprite;
        }
    }

    private void NotifyListeners(int? index)
    {
        if (onBuildingSelectionChanged != null)
        {   
            onBuildingSelectionChanged(index);            
        }
    }
}
