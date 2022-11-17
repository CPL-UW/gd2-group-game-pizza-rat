using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

    private ItemCollectable itemCollected;
    private Player opponent;
    private Player player;

    public void DisplayIngredientPanel(ItemCollectable itemCollected, Player player) {
        gameObject.SetActive(true);
        Transform ingredientImage = transform.Find("Image");
        Transform itemText = transform.Find("Text");
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

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

    public void DisplayOpponentPanel(GameObject opponent, Player player) {
        gameObject.SetActive(true);
        Transform ingredientImage = transform.Find("Image");
        Transform itemText = transform.Find("Text");
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

        this.opponent = opponent.GetComponent<Player>();
        this.player = player;

        ingredientImage.GetComponent<Image>().sprite = opponent.GetComponent<SpriteRenderer>().sprite;
        itemText.GetComponent<TextMeshProUGUI>().text = "You ran into another rat!\n" + 
            "Do you want to start a fight to win his ingredients?";

        Transform buttonTrans = Instantiate(buttonTemplate, transform);
        buttonTrans.gameObject.SetActive(true);
        buttonTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
        buttonTrans.Find("Text").GetComponent<TextMeshProUGUI>().text = "Yes";
        buttonTrans.GetComponent<Button>().onClick.AddListener(() => FightOpponent(this, this.opponent, player));

        Transform buttonTrans2 = Instantiate(buttonTemplate, transform);
        buttonTrans2.gameObject.SetActive(true);
        buttonTrans2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        buttonTrans2.Find("Text").GetComponent<TextMeshProUGUI>().text = "No";
        buttonTrans2.GetComponent<Button>().onClick.AddListener(CloseWindow);
    }



    public void DisplayBasicPanel(String displayText, Sprite sprite = null) {
        gameObject.SetActive(true);
        Transform text = transform.Find("Text");
        text.GetComponent<TextMeshProUGUI>().text = displayText;
        Transform image = transform.Find("Image");
        if (sprite) image.GetComponent<Image>().sprite = sprite;
        else image.gameObject.SetActive(false);
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.Find("Text").GetComponent<TextMeshProUGUI>().text = "Close";
        buttonTemplate.GetComponent<Button>().onClick.AddListener(CloseWindow);

    }

    public void DisplayDiscardPanel(Player player) {
        gameObject.SetActive(true);
        Transform ingredientImage = transform.Find("Image");
        ingredientImage.gameObject.SetActive(false);
        Transform itemText = transform.Find("Text");
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

        this.player = player;

        itemText.GetComponent<TextMeshProUGUI>().text = "Choose one of your ingredients to discard.";

        HashSet<Item.ItemType> set = new HashSet<Item.ItemType>();
        int y = 1;
        Transform buttonTrans;
        foreach (Item item in player.GetInventory().GetItemList()) {
            if (set.Contains(item.itemType)) continue;
            set.Add(item.itemType);

            buttonTrans = Instantiate(buttonTemplate, transform);
            buttonTrans.gameObject.SetActive(true);
            buttonTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 * y++);
            buttonTrans.Find("Text").GetComponent<TextMeshProUGUI>().text = item.itemType.ToString();
            buttonTrans.GetComponent<Button>().onClick.AddListener(() => DiscardIngredient(this.player, item));
        }
        buttonTrans = Instantiate(buttonTemplate, transform);
        buttonTrans.gameObject.SetActive(true);
        buttonTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50 * y);
        buttonTrans.Find("Text").GetComponent<TextMeshProUGUI>().text = "Close";
        buttonTrans.GetComponent<Button>().onClick.AddListener(CloseWindow);
    }

    private void DisplayWinningPanel(Player opponent, Player player) {
        gameObject.SetActive(true);
        Transform ingredientImage = transform.Find("Image");
        Transform itemText = transform.Find("Text");
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

        this.opponent = opponent.GetComponent<Player>();
        this.player = player;

        ingredientImage.GetComponent<Image>().sprite = opponent.GetComponent<SpriteRenderer>().sprite;
        itemText.GetComponent<TextMeshProUGUI>().text = "You won!\n" +
            "Choose one of his ingredients to steal!";

        HashSet<Item.ItemType> set = new HashSet<Item.ItemType>();
        int y = 1;
        foreach (Item item in opponent.GetInventory().GetItemList()) {
            if (set.Contains(item.itemType)) continue;
            set.Add(item.itemType);

            Transform buttonTrans = Instantiate(buttonTemplate, transform);
            buttonTrans.gameObject.SetActive(true);
            buttonTrans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50*y++);
            buttonTrans.Find("Text").GetComponent<TextMeshProUGUI>().text = item.itemType.ToString();
            buttonTrans.GetComponent<Button>().onClick.AddListener(() => StealIngredient(this.player, this.opponent, item));
        }
    }

    private void StealIngredient(Player player, Player opponent, Item item) {
        opponent.GetInventory().Remove(item);
        player.GetInventory().AddItem(item);
        CloseWindow();
    }

    private void DiscardIngredient(Player player, Item item) {
        player.GetInventory().Remove(item);
        CloseWindow();
    }

    private void FightOpponent(OptionPanel prevPanel, Player opponent, Player player) {
        prevPanel.CloseWindow();

        Transform canvas = GameObject.Find("Canvas").transform;
        Transform panelTemplate = canvas.Find("Panel");
        OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();

        if (player.strength > opponent.strength) panel.DisplayWinningPanel(opponent, player);
        else panel.DisplayBasicPanel("You tried to sneak up on your opponent,\n" +
            "but he spotted you right away...\nNo one wins and nothing happens!",
            opponent.GetComponent<SpriteRenderer>().sprite);
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
