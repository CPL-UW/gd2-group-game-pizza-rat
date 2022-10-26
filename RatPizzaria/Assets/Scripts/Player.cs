using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Transform[] waypoints;

    private Inventory inventory;
    private int points = 0;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private UI_Inventory uiInventory;

    [HideInInspector] public int waypointIndex = 0;
    [HideInInspector] public Text textBox;
    [HideInInspector] public bool moveAllowed = false;

    // Use this for initialization
    private void Start() {
        transform.position = waypoints[0].transform.position;

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        uiInventory.CreateNewOrder();
        uiInventory.CreateNewOrder();
    }

    // Update is called once per frame
    private void Update () {
        if (moveAllowed)
            Move();
	}

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position) {
            waypointIndex += 1;
            waypointIndex = waypointIndex % waypoints.Length;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ItemCollectable itemCollectable = other.GetComponent<ItemCollectable>();
        if (itemCollectable != null) {
            inventory.AddItem(itemCollectable.GetItem());
            itemCollectable.DestroySelf();
        }
    }

    public void TryFullfillOrder(Order order) {
        List<Item> items = inventory.GetItemList();
        List<Item.ItemType> recipe = order.GetRecipe();
        foreach (Item.ItemType type in recipe) {
            bool found = false;
            foreach (Item item in items) {
                if (item.itemType.Equals(type)) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                Debug.Log("Unable to make this pizza!");
                return;
            }
        }

        Debug.Log("Ready to make pizza!");
        // The player has all ingredients to fulfill the order
        foreach (Item.ItemType type in recipe) {
            foreach (Item item in items.ToList()) {
                if (item.itemType.Equals(type)) {
                    inventory.Remove(item);
                    break;
                }
            }
        }
        points += order.GetOrderPoints();
        textBox.text = "" + points;
        uiInventory.CreateNewOrder(order);
    }
}
