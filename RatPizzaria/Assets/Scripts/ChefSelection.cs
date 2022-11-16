using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChefSelection : MonoBehaviour
{

    public SpriteRenderer rend;
    public int selectedChef = 0;
    private int chefCnt = 6;
    private TextMeshProUGUI nameBox;
    private Image imageBox;
    private TextMeshProUGUI inventoryBox;
    private TextMeshProUGUI diceBox;
    private TextMeshProUGUI strengthBox;

    private void Start() {
        ChefManager.currTakenChefs.Add(selectedChef);

        nameBox = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        imageBox = transform.Find("RatImage").GetComponent<Image>();
        nameBox.text = ChefManager.GetName(GetChef());
        imageBox.sprite = ChefManager.GetSprite(GetChef());

        inventoryBox = transform.Find("InventoryLimit").GetComponent<TextMeshProUGUI>();
        diceBox = transform.Find("MaxDice").GetComponent<TextMeshProUGUI>();
        strengthBox = transform.Find("Strength").GetComponent<TextMeshProUGUI>();
        UpdateProperties();
    }

    public void UpdateProperties() {
        int[] properties = ChefManager.GetProperties(GetChef());
        inventoryBox.text = "" + properties[0];
        diceBox.text = "" + properties[1];
        strengthBox.text = "" + properties[2];
    }

    public ChefManager.Chef GetChef() {
        Array chefs = Enum.GetValues(typeof(ChefManager.Chef));
        return (ChefManager.Chef)chefs.GetValue(selectedChef);
    }

    public void NextOption() {
        ChefManager.currTakenChefs.Remove(selectedChef);
        selectedChef += 1;
        selectedChef %= chefCnt;
        while (ChefManager.currTakenChefs.Contains(selectedChef)) {
            selectedChef += 1;
            selectedChef %= chefCnt;
        }
        ChefManager.currTakenChefs.Add(selectedChef);

        imageBox.sprite = ChefManager.GetSprite(GetChef());
        nameBox.text = ChefManager.GetName(GetChef());
        UpdateProperties();
    }

    public void PrevOption() {
        ChefManager.currTakenChefs.Remove(selectedChef);
        selectedChef -= 1;
        if (selectedChef == -1) selectedChef = chefCnt-1;
        while (ChefManager.currTakenChefs.Contains(selectedChef)) {
            selectedChef -= 1;
            if (selectedChef == -1) selectedChef = chefCnt - 1;
        }
        ChefManager.currTakenChefs.Add(selectedChef);

        imageBox.sprite = ChefManager.GetSprite(GetChef());
        nameBox.text = ChefManager.GetName(GetChef());
        UpdateProperties();
    }

    
}
