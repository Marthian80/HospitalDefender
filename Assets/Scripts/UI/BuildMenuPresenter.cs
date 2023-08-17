using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuPresenter : MonoBehaviour
{
    [SerializeField] private GameObject[] buildButtons;
    [SerializeField] private Tower tower;

    [SerializeField] private Sprite defaultBuildSprite;
    [SerializeField] private Sprite selectedBuildSprite;

    [SerializeField] private TextMeshProUGUI displayTowerCost;

    public event Action<bool> onBuildTowerSelectionChanged;

    private int? currentSelection = null;
    private AudioPlayer audioPlayer;

    private const int BUILDTOWERBUTTON = 0;


    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        displayTowerCost.text = "$ " + tower.Cost;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            currentSelection = null;
            SetButtonState(currentSelection, true);
            NotifyListeners();
        }
    }

    public void OnBuildingSelected(int index)
    {
        currentSelection = index;
        SetButtonState(index);
        audioPlayer.PlayButtonClickClip();
        NotifyListeners();
    }

    private void SetButtonState(int? index, bool setAlltrue = false)
    {
        for (int i = 0; i < buildButtons.Length; i++)
        {
            Button button = buildButtons[i].GetComponent<Button>();
            Image buttonImage = buildButtons[i].GetComponent<Image>();

            if (i == index && !setAlltrue)
            {
                button.interactable = false;
                buttonImage.sprite = selectedBuildSprite;
            }
            else
            {
                button.interactable = true;
                buttonImage.sprite = defaultBuildSprite;
            }
        }
    }

    private void NotifyListeners()
    {
        if (onBuildTowerSelectionChanged != null)
        {
            if (currentSelection == BUILDTOWERBUTTON)
            {
                onBuildTowerSelectionChanged(true);
            }
            else
            {
                onBuildTowerSelectionChanged(false);
            }
        }
    }
}
