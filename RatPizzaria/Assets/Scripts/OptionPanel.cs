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
    private Transform buttonTemplate;

    public enum PanelType {
        IngredientPanel,
        CombatPanel,
    }

    public void DisplayIngredientPanel(ItemCollectable itemCollected, Player player) {
        gameObject.SetActive(true);
        ingredientImage = transform.Find("Image");
        itemText = transform.Find("Text");
        buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

        panelType = PanelType.IngredientPanel;
        this.itemCollected = itemCollected;
        this.player = player;

        ingredientImage.GetComponent<Image>().sprite = itemCollected.GetItem().GetSprite();
        itemText.GetComponent<TextMeshProUGUI>().text = "Take the ingredient or not?\nYou currently have [" +
            player.GetInventory().GetItemList().Count + " / " +
            player.inventoryLimit + "] items in your inventory.";

        Transform buttonTrans = Instantiate(buttonTemplate, transform);
        buttonTrans.gameObject.SetActive(true);
        buttonTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
        buttonTrans.Find("Text").GetComponent<TextMeshProUGUI>().text = "Yes";
        buttonTrans.GetComponent<Button>().onClick.AddListener(TakeIngredient);

        Transform buttonTrans2 = Instantiate(buttonTemplate, transform);
        buttonTrans2.gameObject.SetActive(true);
        buttonTrans2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        buttonTrans2.Find("Text").GetComponent<TextMeshProUGUI>().text = "No";
        buttonTrans2.GetComponent<Button>().onClick.AddListener(CloseWindow);
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

    private void TakeIngredient() {
        player.GetInventory().AddItem(itemCollected.GetItem());
        itemCollected.DestroySelf();
        CloseWindow();
    }

    private void CloseWindow() {
        Destroy(gameObject);
    }
}
