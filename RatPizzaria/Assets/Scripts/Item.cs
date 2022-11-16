using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Dough,
        TomatoSauce,
        Cheese,
        Mushroom,
        Pepperoni,
        Onion,
        Jalapeno,
        Pepper,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch(itemType) {
            default:
            case ItemType.Dough: return ImageAsset.Instance.doughSprite;
            case ItemType.TomatoSauce: return ImageAsset.Instance.sauceSprite;
            case ItemType.Cheese: return ImageAsset.Instance.cheeseSprite;
            case ItemType.Mushroom: return ImageAsset.Instance.mushroomSprite;
            case ItemType.Pepperoni: return ImageAsset.Instance.pepperoniSprite;
            case ItemType.Onion: return ImageAsset.Instance.onionSprite;
            case ItemType.Jalapeno: return ImageAsset.Instance.jalapenoSprite;
            case ItemType.Pepper: return ImageAsset.Instance.pepperSprite;
        }
    }
}
