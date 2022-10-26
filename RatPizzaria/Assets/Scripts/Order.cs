using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order: MonoBehaviour {

    public enum OrderType
    {
        CheesePizza,
        MushroomPizza,
        PepperoniPizza,
    }

    public OrderType orderType;
    private Button button;

    private void Start() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CheckOrderStatus);
    }

    private void CheckOrderStatus() {
        Player associatedPlayer = null;
        if (transform.parent.parent.name.Contains("1")) associatedPlayer = GameObject.Find("Player1").GetComponent<Player>();
        else associatedPlayer = GameObject.Find("Player2").GetComponent<Player>();
        associatedPlayer.TryFullfillOrder(this);
    }

    public List<Item.ItemType> GetRecipe() {
        List<Item.ItemType> itemList = new List<Item.ItemType>();

        switch (orderType) {
            default:
            case OrderType.CheesePizza:
                itemList.Add(Item.ItemType.Cheese);
                return itemList;
            case OrderType.MushroomPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Mushroom);
                return itemList;
            case OrderType.PepperoniPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Pepperoni);
                return itemList;
        }
    }

    public string GetRecipeString() {
        switch (orderType) {
            default:
            case OrderType.CheesePizza:
                return "Cheese";
            case OrderType.MushroomPizza:
                return "Cheese, Mushroom";
            case OrderType.PepperoniPizza:
                return "Cheese, Pepperoni";
        }
    }

    public string GetOrderString() {
        switch (orderType) {
            default:
            case OrderType.CheesePizza:
                return "Cheese Pizza";
            case OrderType.MushroomPizza:
                return "Mushroom Pizza";
            case OrderType.PepperoniPizza:
                return "Pepperoni Pizza";
        }
    }

    public Sprite GetSprite() {
        switch (orderType) {
            default:
            case OrderType.CheesePizza: return ImageAsset.Instance.cheesePizzaSprite;
            case OrderType.MushroomPizza: return ImageAsset.Instance.mushroomPizzaSprite;
            case OrderType.PepperoniPizza: return ImageAsset.Instance.pepperoniPizzaSprite;
        }
    }
}
