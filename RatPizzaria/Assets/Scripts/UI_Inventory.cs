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
    private Button button;
    private Player associatedPlayer;

    private void Awake() {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
        orderSlotContainer = transform.Find("OrderSlotContainer");
        orderSlotTemplate = orderSlotContainer.Find("OrderSlotTemplate");
        button = itemSlotContainer.GetComponent<Button>();
        button.onClick.AddListener(ThrowAwayItem);
    }

    private void ThrowAwayItem() {
        Transform canvas = GameObject.Find("Canvas").transform;
        Transform panelTemplate = canvas.Find("Panel");
        OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();
        panel.DisplayDiscardPanel(associatedPlayer);
    }

    public void SetInventory(Inventory inventory, Player player) {
        this.inventory = inventory;
        this.associatedPlayer = player;

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
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.GetItemList()) {
            RectTransform itemSlotRectTrans = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTrans.gameObject.SetActive(true);

            itemSlotRectTrans.anchoredPosition = new Vector2(20+x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTrans.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            x++;
            if (x > 3) {
                x = 0;
                y--;
            }
        }
    }

    public void CreateNewOrder(Order orderToRenew = null, int ingredientCount = 0) {
        Array types = Enum.GetValues(typeof(Order.OrderType));
        Order newOrder = orderToRenew;

        if (newOrder == null) {
            float orderSlotCellSize = 40f;
            int x = orderSlotContainer.childCount - 1;
            newOrder = Instantiate(orderSlotTemplate, orderSlotContainer).GetComponent<Order>();
            newOrder.SetPlayer(associatedPlayer);
            newOrder.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + x * orderSlotCellSize, -20);
            newOrder.gameObject.SetActive(true);
        } else {
            ingredientCount = newOrder.GetRecipe().Count;
        }

        if (ingredientCount == 1) newOrder.SetOrderType((Order.OrderType)types.GetValue(Random.Range(0, 6)));
        else if (ingredientCount == 2) newOrder.SetOrderType((Order.OrderType)types.GetValue(Random.Range(6, 13)));
        else newOrder.SetOrderType((Order.OrderType)types.GetValue(Random.Range(13, types.Length)));


    }
}