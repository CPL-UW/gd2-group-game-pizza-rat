using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake() {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
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
        //foreach (Transform child in itemSlotContainer) {
        //    if (child == itemSlotTemplate) continue;
        //    Destroy(child.gameObject);
        //}

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
            if (x>3) {
                x = 0;
                y--;
            }
        }
    }
}
