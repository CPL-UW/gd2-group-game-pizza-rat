using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int[] currIndex = new int[] { 0, 0 };
    public Transform uiInfo;
    public GameObject moveText;

    private Inventory inventory;
    private UI_Inventory uiInventory;
    private TextMeshProUGUI statTextMeshPro;
    private TextMeshProUGUI pointsTextBox;
    private Transform[][] waypoints;

    [SerializeField] private float moveSpeed = 1f;

    [HideInInspector] public int waypointIndex = 0;
    [HideInInspector] public bool moveAllowed = false;
    [HideInInspector] public int maxDice = 0;
    [HideInInspector] public int inventoryLimit = 0;
    [HideInInspector] public int strength = 0;
    [HideInInspector] public int points = 0;

    // Use this for initialization
    private void Awake() {
        gameObject.SetActive(false);
        Transform waypointParent = GameObject.Find("BoardWaypoints").GetComponent<Transform>();
        waypoints = new Transform[waypointParent.childCount][];
        for (int i = 0; i < waypointParent.childCount; i++) {
            Transform row = waypointParent.GetChild(i);
            waypoints[i] = new Transform[row.childCount];
            for (int j = 0; j < row.childCount; j++) {
                waypoints[i][j] = row.GetChild(j);
            }
        }
    }

    // Update is called once per frame
    private void Update () {
        if (moveAllowed)
            Move();
	}

    public void FinishSetUpPlayer() {
        transform.position = waypoints[currIndex[0]][currIndex[1]].transform.position;

        statTextMeshPro = uiInfo.Find("Stat").Find("StatText").GetComponent<TextMeshProUGUI>();
        pointsTextBox = uiInfo.Find("PlayerPoints").Find("PointText").GetComponent<TextMeshProUGUI>();
        moveText = uiInfo.Find("PlayerMoveText").gameObject;

        uiInventory = uiInfo.Find("PlayerInventory").GetComponent<UI_Inventory>();
        inventory = new Inventory();
        uiInventory.SetInventory(inventory, this);
        uiInventory.CreateNewOrder(null, 1);
        uiInventory.CreateNewOrder(null, 2);
        uiInventory.CreateNewOrder(null, 3);

        RefreshPlayerInfo();
        gameObject.SetActive(true);
    }

    private void Move()
    {
        if (GameControl.diceSideThrown > 0) {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currIndex[0] > 0) { 
                Transform dest = waypoints[--currIndex[0]][currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currIndex[1] > 0) {
                Transform dest = waypoints[currIndex[0]][--currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currIndex[0]+1 < waypoints.Length) {
                Transform dest = waypoints[++currIndex[0]][currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
            else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currIndex[1] + 1 < waypoints[0].Length) {
                Transform dest = waypoints[currIndex[0]][++currIndex[1]].transform;
                while (transform.position != dest.position) {
                    transform.position = Vector2.MoveTowards(transform.position, dest.position, moveSpeed * Time.deltaTime);
                }
                GameControl.diceSideThrown--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() != null) {
            if (GameControl.whosTurn == gameObject) {
                Transform canvas = GameObject.Find("Canvas").transform;
                Transform panelTemplate = canvas.Find("Panel");
                OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();
                panel.DisplayOpponentPanel(other.gameObject, this);
            }
        }

        if (other.GetComponent<ItemCollectable>() != null) {
            ItemCollectable itemCollectable = other.GetComponent<ItemCollectable>();
            if (inventory.GetItemList().Count >= inventoryLimit) {
                Transform canvas = GameObject.Find("Canvas").transform;
                Transform panelTemplate = canvas.Find("Panel");
                OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();
                panel.DisplayBasicPanel("You reached the inventory limit!");
            }
            else {
                inventory.AddItem(itemCollectable.GetItem());
                itemCollectable.DestroySelf();
                //Transform canvas = GameObject.Find("Canvas").transform;
                //Transform panelTemplate = canvas.Find("Panel");
                //OptionPanel panel = Instantiate(panelTemplate, canvas).GetComponent<OptionPanel>();
                //panel.DisplayIngredientPanel(itemCollectable, this);
            }
        }
    }

    public Inventory GetInventory() { return this.inventory; }

    private void RefreshPlayerInfo() {
        statTextMeshPro.text = "Limit: " + inventoryLimit + "   Dice: " + maxDice
            + "   Strength: " + strength;
        pointsTextBox.text = "" + points;
    }

    public void TryFullfillOrder(Order order) {
        List<Item> items = inventory.GetItemList();
        List<Item> itemsCopy = new List<Item>(items);
        List<Item.ItemType> recipe = order.GetRecipe();

        foreach (Item.ItemType type in recipe) {
            bool found = false;
            for (int i = 0; i < itemsCopy.Count; i++) {
                Item curr = itemsCopy[i];
                if (curr.itemType.Equals(type)) {
                    itemsCopy.Remove(curr);
                    found = true;
                    break;
                }
            }
            if (!found) {
                Debug.Log("Unable to make this pizza! You don't have " + type + ".");
                return;
            }
        }

        // The player has all ingredients to fulfill the order
        Debug.Log("You are ready to make pizza!");
        inventory.SetItemList(itemsCopy);
        CompleteOrder(order);
        //foreach (Item.ItemType type in recipe) {
        //    foreach (Item item in items.ToList()) {
        //        if (item.itemType.Equals(type)) {
        //            inventory.Remove(item);
        //            break;
        //        }
        //    }
        //}
    }

    private void CompleteOrder(Order order) {
        points += order.GetOrderPoints();
        if (order.bonusType == Order.BonusType.IncreaseInventory) inventoryLimit++;
        else if (order.bonusType == Order.BonusType.IncreaseStrength) strength++;
        else if (order.bonusType == Order.BonusType.IncreaseDiceNumber) maxDice = Mathf.Min(maxDice+1, 8);
        RefreshPlayerInfo();

        uiInventory.CreateNewOrder(order);
    }
}
