using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Cheese,
        Mushroom,
        Pepperoni,
        Garlic,
        Jalapeno,
        Pepper,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch(itemType) {
            default:
            case ItemType.Cheese: return ImageAsset.Instance.cheeseSprite;
            case ItemType.Mushroom: return ImageAsset.Instance.mushroomSprite;
            case ItemType.Pepperoni: return ImageAsset.Instance.pepperoniSprite;
            case ItemType.Garlic: return ImageAsset.Instance.garlicSprite;
            case ItemType.Jalapeno: return ImageAsset.Instance.jalapenoSprite;
            case ItemType.Pepper: return ImageAsset.Instance.pepperSprite;
        }
    }
}
