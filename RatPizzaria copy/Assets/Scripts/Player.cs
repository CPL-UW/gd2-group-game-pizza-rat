using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //public Transform[] waypoints;

    private Inventory inventory;

    //[SerializeField] private float moveSpeed = 1f;
    [SerializeField] private UI_Inventory uiInventory;


    //[HideInInspector] public int waypointIndex = 0;

    public bool moveAllowed  = false;
    [HideInInspector] public int x = 0;
    [HideInInspector] public int y = 0;
    [HideInInspector] public bool done = false;
    // Use this for initialization
    private void Start() {
        transform.position = transform.position;

        inventory = new Inventory();
        //uiInventory.SetInventory(inventory);
        //uiInventory.CreateNewOrder();
        //uiInventory.CreateNewOrder();
    }

    // Update is called once per frame
    private void Update () {
        if (moveAllowed)
            Move();
	}

    private void Move()
    {
        Debug.Log("Reaches Move");
        // LOOP FOR DICE NUM
        for (int i=0; i<GameControl.diceSideThrown; i++) {
            StartCoroutine(WaitForMove());
        }
        Debug.Log("("+x+","+y+")");
        moveAllowed = false;
        done = true;
    }

    IEnumerator WaitForMove()
    {
        for (;;) {
            if (Input.GetKeyDown(KeyCode.UpArrow) && y>0) {
                transform.Translate(Vector2.up * 1.23f);
                y--;
                break;
            } else if (Input.GetKeyDown(KeyCode.DownArrow) && y<7) {  
                transform.Translate(Vector2.down * 1.23f);
                y++;
                break;
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) && x>0) {  
                transform.Translate(Vector2.left * 1.23f);
                x--;
                break;
            } else if (Input.GetKeyDown(KeyCode.RightArrow) && x<7) {  
                this.transform.Translate(Vector2.right * 1.23f);
                x++;
                break;
            }
            yield return null;
        }
        Debug.Log("1 move");
        //moveAllowed = false;
        //done = true;
        yield return null;
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
        uiInventory.CreateNewOrder(order);
    }
}
