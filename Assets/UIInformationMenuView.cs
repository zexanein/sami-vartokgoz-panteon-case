using BuildingPlacementSystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInformationMenuView : MonoBehaviour
{
    [SerializeField] private UIInformationMenuController controller;
    [SerializeField] private GameObject menuContent;
    [SerializeField] private GameObject unitProductionPanel;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button demolishKillButton;
    [SerializeField] private TMP_Text demolishKillButtonText;
    
    public void DisplayBuilding(Building building)
    {
        menuContent.SetActive(true);
        labelText.text = building.Blueprint.buildingName;
        descriptionText.text = building.Blueprint.buildingDescription;
        iconImage.sprite = building.Blueprint.displaySprite;
        unitProductionPanel.SetActive(building.Blueprint.producingUnits);
        demolishKillButtonText.text = "Demolish";
        UpdateDemolishKillEvent(() => controller.DemolishBuilding(building));
    }

    public void ClearInformation()
    {
        labelText.text = "";
        iconImage.sprite = null;
        menuContent.SetActive(false);
    }

    private void UpdateDemolishKillEvent(UnityAction onClickAction)
    {
        demolishKillButton.onClick.RemoveAllListeners();
        demolishKillButton.onClick.AddListener(onClickAction);
    }
}
