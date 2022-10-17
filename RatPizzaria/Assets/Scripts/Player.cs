using UnityEngine;

public class Player : MonoBehaviour {

    public Transform[] waypoints;

    private Inventory inventory;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private UI_Inventory uiInventory;

    [HideInInspector]
    public int waypointIndex = 0;
    public bool moveAllowed = false;

    // Use this for initialization
    private void Start() {
        transform.position = waypoints[0].transform.position;

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
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
}
