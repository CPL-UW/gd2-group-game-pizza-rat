using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

    private ItemCollectable itemCollected;
    private GameObject opponent;
    private Player player;
    private PanelType panelType;
    private Transform ingredientImage;
    private Transform itemText;

    public enum PanelType {
        IngredientPanel,
        CombatPanel,
    }

    void Start() {
        ingredientImage = transform.Find("Image");
        itemText = transform.Find("Text");
        gameObject.SetActive(false);
    }

    public void DisplayIngredientPanel(ItemCollectable itemCollected, Player player) {
        this.itemCollected = itemCollected;
        this.player = player;
        ingredientImage.GetComponent<Image>().sprite = itemCollected.GetItem().GetSprite();
        itemText.GetComponent<TextMeshProUGUI>().text = "Take the ingredient or not?\nYou currently have [" +
            player.GetInventory().GetItemList().Count + " / " +
            player.inventoryLimit + "] items in your inventory.";
        panelType = PanelType.IngredientPanel;
        gameObject.SetActive(true);
    }

    public void DisplayPlayerPanel(GameObject opponent, Player player) {
        this.opponent = opponent;
        this.player = player;
        ingredientImage.GetComponent<Image>().sprite = opponent.GetComponent<SpriteRenderer>().sprite;
        itemText.GetComponent<TextMeshProUGUI>().text = "Fight with your opponent?\n" +
            "You may have a chance to steal his ingredient!";
        panelType = PanelType.CombatPanel;
        gameObject.SetActive(true);
    }

    public void PlayerDesicion(bool consent) {
        gameObject.SetActive(false);
        if (!consent) return;
        switch (panelType) {
            case PanelType.IngredientPanel:
                player.GetInventory().AddItem(itemCollected.GetItem());
                itemCollected.DestroySelf();
                return;
            case PanelType.CombatPanel:
                return;
        }

    }
}
