using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChefSelection : MonoBehaviour
{

    public SpriteRenderer rend;
    public int selectedChef = 0;
    private int chefCnt = 6;

    private void Start() {
        rend.sprite = ChefManager.GetSprite(GetChef());
    }

    public ChefManager.Chef GetChef() {
        Array chefs = Enum.GetValues(typeof(ChefManager.Chef));
        return (ChefManager.Chef)chefs.GetValue(selectedChef);
    }

    public void NextOption() {
        selectedChef += 1;
        selectedChef %= chefCnt;
        rend.sprite = ChefManager.GetSprite(GetChef());
    }

    public void PrevOption() {
        selectedChef -= 1;
        if (selectedChef == -1) selectedChef = chefCnt-1;
        rend.sprite = ChefManager.GetSprite(GetChef());
    }

    
}
