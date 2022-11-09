using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random=System.Random;

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
            "Do you want to start a fight to win their ingredients?";

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

    private void DisplayOpponentIngredientPanel(Player opponent, Player player) {
        gameObject.SetActive(true);
        Transform ingredientImage = transform.Find("Image");
        Transform itemText = transform.Find("Text");
        Transform buttonTemplate = transform.Find("Button");
        buttonTemplate.gameObject.SetActive(false);

        this.opponent = opponent.GetComponent<Player>();
        this.player = player;
        string name = player.ToString().Substring(0,7);
        string loser = opponent.ToString().Substring(0,7);

        ingredientImage.GetComponent<Image>().sprite = opponent.GetComponent<SpriteRenderer>().sprite;
        // check if there are ingredients to take
        if (opponent.GetInventory().GetItemList().Count == 0) {
            itemText.GetComponent<TextMeshProUGUI>().text = name + " wins, but " + loser + " has no ingredients to steal!";
        } else {
            itemText.GetComponent<TextMeshProUGUI>().text = name + " wins!\n" +
                name + ", choose one of your opponent's ingredients to steal!";
        }

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

        //exit button
        Transform exitButton = Instantiate(buttonTemplate, transform);
        exitButton.gameObject.SetActive(true);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50*y++);
        exitButton.Find("Text").GetComponent<TextMeshProUGUI>().text = "Exit";
        exitButton.GetComponent<Button>().onClick.AddListener(() => CloseWindow());
    }

    private void StealIngredient(Player player, Player opponent, Item item) {
        opponent.GetInventory().Remove(item);
        player.GetInventory().AddItem(item);
        CloseWindow();
    }

    private void FightOpponent(OptionPanel prevPanel, Player opponent, Player player) {
        prevPanel.CloseWindow();

        Transform canvas = GameObject.Find("Canvas").transform;
        Transform panelTemplate = canvas.Find("Panel");
        OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();

        Random rand = new Random();
        int f = rand.Next(0, 1);
        if (f == 0) {
            //attacker wins
            panel.DisplayOpponentIngredientPanel(opponent, player);
        } else {
            //defender wins
            panel.DisplayOpponentIngredientPanel(player, opponent);
        }
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
