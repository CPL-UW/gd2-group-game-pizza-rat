using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Inventory : MonoBehaviour {
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Transform orderSlotContainer;
    private Transform orderSlotTemplate;

    private void Awake() {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
        orderSlotContainer = transform.Find("OrderSlotContainer");
        orderSlotTemplate = orderSlotContainer.Find("OrderSlotTemplate");
    }

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems() {
        foreach (Transform child in itemSlotContainer) {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 70f;
        foreach (Item item in inventory.GetItemList()) {
            RectTransform itemSlotRectTrans = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTrans.gameObject.SetActive(true);

            itemSlotRectTrans.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTrans.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            x++;
            if (x > 3) {
                x = 0;
                y--;
            }
        }
    }

    public void CreateNewOrder(Order orderToRenew = null) {
        Order newOrder = orderToRenew;
        if (newOrder == null) {
            float orderSlotCellSize = 120f;
            int y = orderSlotContainer.childCount - 1;
            newOrder = Instantiate(orderSlotTemplate, orderSlotContainer).GetComponent<Order>();
            newOrder.gameObject.SetActive(true);
            newOrder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y * orderSlotCellSize);
        }

        Array types = Enum.GetValues(typeof(Order.OrderType));
        newOrder.orderType = (Order.OrderType)types.GetValue(Random.Range(0, types.Length));
        newOrder.GetComponent<Image>().sprite = newOrder.GetComponent<Order>().GetSprite();
        newOrder.GetComponent<HoverTip>().tipToShow = "<" + newOrder.GetOrderString() + ">\n" + newOrder.GetRecipeString();

    }
}