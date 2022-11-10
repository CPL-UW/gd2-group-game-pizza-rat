using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory {
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory() {
        itemList = new List<Item>();
    }

    public void AddItem(Item item) {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Remove(Item item) {
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return itemList;
    }

    public void SetItemList(List<Item> list) {
        this.itemList = list;
    }
}
