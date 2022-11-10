using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order: MonoBehaviour {

    public enum OrderType
    {
        PepperoniMushroomOnionPizza,
        CheeseMushroomPepperoniPizza,
        CheesePepperJalapenoPizza,
        CheeseJalapenoOnionPizza,
        JalapenoOnionOnionPizza,
    }

    public enum BonusType {
        IncreaseInventory,
        IncreaseDiceNumber,
        IncreaseStrength,
        BonusPoints,
        None,
    }

    public OrderType orderType;
    public BonusType bonusType;
    private Button button;
    private Player associatedPlayer;

    private void Start() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CheckOrderStatus);
    }

    public void SetOrderType(OrderType type) {
        this.orderType = type;
        SetBonusType();
        gameObject.GetComponent<HoverTip>().tipToShow = GetBonusString();
        gameObject.GetComponent<Image>().sprite = GetSprite();
    }

    private void SetBonusType() {
        switch (orderType) {
            case OrderType.CheesePepperJalapenoPizza:
                bonusType = BonusType.IncreaseInventory;
                break;
            case OrderType.CheeseMushroomPepperoniPizza:
                bonusType = BonusType.IncreaseDiceNumber;
                break;
            case OrderType.CheeseJalapenoOnionPizza:
                bonusType = BonusType.IncreaseStrength;
                break;
            case OrderType.JalapenoOnionOnionPizza:
                bonusType = BonusType.BonusPoints;
                break;
            default:
                bonusType = BonusType.None;
                break;
        }
    }

    public void SetPlayer(Player player) { this.associatedPlayer = player; }

    private void CheckOrderStatus() {
        associatedPlayer.TryFullfillOrder(this);
    }

    public List<Item.ItemType> GetRecipe() {
        List<Item.ItemType> itemList = new List<Item.ItemType>();

        switch (orderType) {
            default:
            case OrderType.PepperoniMushroomOnionPizza:
                itemList.Add(Item.ItemType.Pepperoni);
                itemList.Add(Item.ItemType.Mushroom);
                itemList.Add(Item.ItemType.Onion);
                return itemList;
            case OrderType.CheeseMushroomPepperoniPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Mushroom);
                itemList.Add(Item.ItemType.Pepperoni);
                return itemList;
            case OrderType.CheesePepperJalapenoPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Pepper);
                itemList.Add(Item.ItemType.Jalapeno);
                return itemList;
            case OrderType.CheeseJalapenoOnionPizza:
                itemList.Add(Item.ItemType.Cheese);
                itemList.Add(Item.ItemType.Onion);
                itemList.Add(Item.ItemType.Jalapeno);
                return itemList;
            case OrderType.JalapenoOnionOnionPizza:
                itemList.Add(Item.ItemType.Jalapeno);
                itemList.Add(Item.ItemType.Onion);
                itemList.Add(Item.ItemType.Onion);
                return itemList;
        }
    }

    public int GetOrderPoints() {
        switch (bonusType) {
            case BonusType.BonusPoints:
                return 2;
            default:
                return 1;
        }
    }

    public Sprite GetSprite() {
        switch (orderType) {
            default:
            case OrderType.PepperoniMushroomOnionPizza: return ImageAsset.Instance.PepperoniMushroomOnionPizzaSprite;
            case OrderType.CheeseMushroomPepperoniPizza: return ImageAsset.Instance.CheeseMushroomPepperoniPizzaSprite;
            case OrderType.CheesePepperJalapenoPizza: return ImageAsset.Instance.CheesePepperJalapenoPizzaSprite;
            case OrderType.CheeseJalapenoOnionPizza: return ImageAsset.Instance.CheeseJalapenoOnionPizzaSprite;
            case OrderType.JalapenoOnionOnionPizza: return ImageAsset.Instance.JalapenoOnionOnionPizzaSprite;
        }
    }

    public string GetBonusString() {
        switch (bonusType) {
            case BonusType.IncreaseInventory:
                return "You may increase your inventory limit!";
            case BonusType.IncreaseDiceNumber:
                return "You may upgrade your dice!";
            case BonusType.IncreaseStrength:
                return "You may increase your strength!";
            case BonusType.BonusPoints:
                return "Special pizza that is worth 2 points!";
            default:
                return "Just an ordinary pizza...";

        }
    }
}
