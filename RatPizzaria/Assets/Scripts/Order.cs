using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Order: MonoBehaviour {

    public enum OrderType
    {
        Onion,
        Pepperoni,
        Cheese,
        Jalapeno,
        Pepper,
        Mushroom,
        PepperMushroom,
        CheeseOnion,
        CheesePepper,
        JalapenoPepperoni,
        JalapenoOnion,
        MushroomPepperoni,
        CheeseJalapeno,
        PepperoniMushroomOnion,
        PepperoniPepperMushroom,
        CheeseJalapenoOnion,
        CheesePepperoniPepper,
        JalapenoPepperoniMushroom,
        JalapenoOnionOnion,
        CheeseMushroomPepperoni,
        CheesePepperJalapeno
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
            case OrderType.Cheese:
            case OrderType.Pepperoni:
            case OrderType.Mushroom:
                bonusType = BonusType.IncreaseInventory;
                break;
            case OrderType.CheeseOnion:
            case OrderType.CheesePepper:
            case OrderType.CheeseJalapeno:
                bonusType = BonusType.IncreaseDiceNumber;
                break;
            case OrderType.JalapenoPepperoni:
            case OrderType.MushroomPepperoni:
            case OrderType.PepperMushroom:
            case OrderType.JalapenoOnion:
                bonusType = BonusType.IncreaseStrength;
                break;
            default:
                bonusType = BonusType.BonusPoints;
                break;
        }
    }

    public void SetPlayer(Player player) { this.associatedPlayer = player; }

    private void CheckOrderStatus() {
        associatedPlayer.TryFullfillOrder(this);
    }

    public List<Item.ItemType> GetRecipe() {
        List<Item.ItemType> itemList = new List<Item.ItemType>();
        itemList.Add(Item.ItemType.Dough);
        itemList.Add(Item.ItemType.TomatoSauce);

        if (orderType.ToString().Contains("Cheese")) itemList.Add(Item.ItemType.Cheese);
        if (orderType.ToString().Contains("Pepperoni")) itemList.Add(Item.ItemType.Pepperoni);
        if (orderType.ToString().Contains("Mushroom")) itemList.Add(Item.ItemType.Mushroom);
        if (orderType.ToString().Contains("Onion")) {
            itemList.Add(Item.ItemType.Onion);
            if (orderType == OrderType.JalapenoOnionOnion) itemList.Add(Item.ItemType.Onion);
        }
        if (orderType.ToString().Contains("Jalapeno")) itemList.Add(Item.ItemType.Jalapeno);
        if (orderType == OrderType.Pepper || orderType == OrderType.PepperMushroom
            || orderType == OrderType.CheesePepper
            || orderType == OrderType.CheesePepperJalapeno) itemList.Add(Item.ItemType.Pepper);
        return itemList;
    }

    public int GetOrderPoints() {
        switch (bonusType) {
            case BonusType.BonusPoints:
                return GetRecipe().Count - 2;
            default: return 0;
        }
    }

    public Sprite GetSprite() {
        switch (orderType) {
            default:
            case OrderType.Onion: return ImageAsset.Instance.OnionPizzaSprite;
            case OrderType.Pepperoni: return ImageAsset.Instance.PepperoniPizzaSprite;
            case OrderType.Cheese: return ImageAsset.Instance.CheesePizzaSprite;
            case OrderType.Jalapeno: return ImageAsset.Instance.JalapenoPizzaSprite;
            case OrderType.Pepper: return ImageAsset.Instance.PepperPizzaSprite;
            case OrderType.Mushroom: return ImageAsset.Instance.MushroomPizzaSprite;

            case OrderType.PepperMushroom: return ImageAsset.Instance.PepperMushroomPizzaSprite;
            case OrderType.CheeseOnion: return ImageAsset.Instance.CheeseOnionPizzaSprite;
            case OrderType.CheesePepper: return ImageAsset.Instance.CheesePepperPizzaSprite;
            case OrderType.JalapenoPepperoni: return ImageAsset.Instance.JalapenoPepperoniPizzaSprite;
            case OrderType.JalapenoOnion: return ImageAsset.Instance.JalapenoOnionPizzaSprite;
            case OrderType.MushroomPepperoni: return ImageAsset.Instance.MushroomPepperoniPizzaSprite;
            case OrderType.CheeseJalapeno: return ImageAsset.Instance.CheeseJalapenoPizzaSprite;

            case OrderType.PepperoniMushroomOnion: return ImageAsset.Instance.PepperoniMushroomOnionPizzaSprite;
            case OrderType.PepperoniPepperMushroom: return ImageAsset.Instance.PepperoniPepperMushroomPizzaSprite;
            case OrderType.CheeseJalapenoOnion: return ImageAsset.Instance.CheeseJalapenoOnionPizzaSprite;
            case OrderType.CheesePepperoniPepper: return ImageAsset.Instance.CheesePepperoniPepperPizzaSprite;
            case OrderType.JalapenoPepperoniMushroom: return ImageAsset.Instance.JalapenoPepperoniMushroomPizzaSprite;
            case OrderType.JalapenoOnionOnion: return ImageAsset.Instance.JalapenoOnionOnionPizzaSprite;
            case OrderType.CheeseMushroomPepperoni: return ImageAsset.Instance.CheeseMushroomPepperoniPizzaSprite;
            case OrderType.CheesePepperJalapeno: return ImageAsset.Instance.CheesePepperJalapenoPizzaSprite;
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
                return "Special pizza that is worth " + GetOrderPoints() + " points!";
            default:
                return "Just an ordinary pizza...";

        }
    }
}
