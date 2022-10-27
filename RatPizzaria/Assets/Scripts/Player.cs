using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Transform[][] waypoints;
    public int[] currIndex = new int[] { 0, 0 };

    private Inventory inventory;
    private int points = 0;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private UI_Inventory uiInventory;

    [HideInInspector] public int waypointIndex = 0;
    [HideInInspector] public Text textBox;
    [HideInInspector] public bool moveAllowed = false;

    // Use this for initialization
    private void Start() {
        Transform waypointParent = GameObject.Find("BoardWaypoints").GetComponent<Transform>();
        waypoints = new Transform[waypointParent.childCount][];
        for (int i = 0; i < waypointParent.childCount; i++) {
            Transform row = waypointParent.GetChild(i);
            waypoints[i] = new Transform[row.childCount];
            for (int j = 0; j < row.childCount; j++) {
                waypoints[i][j] = row.GetChild(j);
            }
        }

        transform.position = waypoints[currIndex[0]][currIndex[1]].transform.position;

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
        if (GameControl.diceSideThrown > 0) {
            if (Input.GetKeyDown(KeyCode.W) && currIndex[0] > 0) { 
                Transform dest = waypoints[--currIndex[0]][currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if (Input.GetKeyDown(KeyCode.A) && currIndex[1] > 0) {
                Transform dest = waypoints[currIndex[0]][--currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if (Input.GetKeyDown(KeyCode.S) && currIndex[0]+1 < waypoints.Length) {
                Transform dest = waypoints[++currIndex[0]][currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if (Input.GetKeyDown(KeyCode.D) && currIndex[1] + 1 < waypoints[0].Length) {
                Transform dest = waypoints[currIndex[0]][++currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
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
