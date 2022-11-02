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

    public enum BonusType {
        IncreaseInventory,
        IncreaseDiceNumber,
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
                itemList.Add(Item.ItemType.Pepper);
                itemList.Add(Item.ItemType.Jalapeno);
                return itemList;
        }
    }

    public int GetOrderPoints() {
        switch (bonusType) {
            case BonusType.None:
                return 2;
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

    public string GetBonusString() {
        switch (bonusType) {
            case BonusType.IncreaseInventory:
                return "You can increase your inventory limit by ONE!";
            case BonusType.IncreaseDiceNumber:
                return "You can increase the maximum number of your dice ONE!";
            default:
                return "Special pizza that is worth 2 points!";
        }
    }
}
