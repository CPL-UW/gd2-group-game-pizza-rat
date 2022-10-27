using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order: MonoBehaviour {

    public enum OrderType
    {
        PepperoniMushroomGarlicPizza,
        CheeseMushroomPepperoniPizza,
        CheesePepperJalapenoPizza,
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
            case OrderType.PepperoniMushroomGarlicPizza:
                itemList.Add(Item.ItemType.Pepperoni);
                itemList.Add(Item.ItemType.Mushroom);
                itemList.Add(Item.ItemType.Garlic);
                return itemList;
            case OrderType.CheeseMushroomPepperoniPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Mushroom);
                itemList.Add(Item.ItemType.Pepperoni);
                return itemList;
            case OrderType.CheesePepperJalapenoPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Pepperoni);
                itemList.Add(Item.ItemType.Jalapeno);
                return itemList;
        }
    }

    public int GetOrderPoints() {
        switch (orderType) {
            default:
                return 1;
        }
    }

    public Sprite GetSprite() {
        switch (orderType) {
            default:
            case OrderType.PepperoniMushroomGarlicPizza: return ImageAsset.Instance.PepperoniMushroomGarlicPizzaSprite;
            case OrderType.CheeseMushroomPepperoniPizza: return ImageAsset.Instance.CheeseMushroomPepperoniPizzaSprite;
            case OrderType.CheesePepperJalapenoPizza: return ImageAsset.Instance.CheesePepperJalapenoPizzaSprite;
        }
    }
}
