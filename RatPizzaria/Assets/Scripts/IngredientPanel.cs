using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanel : MonoBehaviour {

    private ItemCollectable itemCollected;
    private Player player;
    private Transform ingredientImage;
    private Transform itemText;

    void Start() {
        ingredientImage = transform.Find("IngredientImage");
        itemText = transform.Find("Text");
        gameObject.SetActive(false);
    }

    public void DisplayPanel(ItemCollectable itemCollected, Player player) {
        this.itemCollected = itemCollected;
        this.player = player;
        ingredientImage.GetComponent<Image>().sprite = itemCollected.GetItem().GetSprite();
        itemText.GetComponent<TextMeshProUGUI>().text = "Take the ingredient or not?\nYou currently have [" +
            player.GetInventory().GetItemList().Count + " / " +
            player.inventoryLimit + "] items in your inventory.";
        gameObject.SetActive(true);
    }

    public void DecisionOnItem(bool takeItem) {
        gameObject.SetActive(false);
        if (!takeItem) return;
        player.GetInventory().AddItem(itemCollected.GetItem());
        itemCollected.DestroySelf();
        //StartCoroutine(Replace());
        
    }
}
