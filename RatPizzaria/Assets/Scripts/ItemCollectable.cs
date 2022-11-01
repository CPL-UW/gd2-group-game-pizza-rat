using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour {

    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public static ItemCollectable SpawnItemCollectable(Vector3 position, Item item) {
        Transform transform = Instantiate(ImageAsset.Instance.pfItemCollectable, position, Quaternion.identity);
        ItemCollectable itemCollectable = transform.GetComponent<ItemCollectable>();
        itemCollectable.SetItem(item);

        return itemCollectable;
    }

    public void SetItem(Item item) {
        this.item = item;
        this.spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem() { return this.item; }

    public void DestroySelf() {
        Destroy(gameObject);

        Debug.Log("Start Turn: " + GameControl.turn);
        GameControl gc = GameObject.Find("GameControl").GetComponent<GameControl>();
        gc.SpawnItemCollectable(item.itemType);
    }


}
