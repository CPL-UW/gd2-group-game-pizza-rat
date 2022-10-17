using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Cheese,
        Mushroom,
        Pepperoni,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch(itemType) {
            default:
            case ItemType.Cheese: return ItemAsset.Instance.cheeseSprite;
            case ItemType.Mushroom: return ItemAsset.Instance.mushroomSprite;
            case ItemType.Pepperoni: return ItemAsset.Instance.pepperoniSprite;
        }
    }
}
