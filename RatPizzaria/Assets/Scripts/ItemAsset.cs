using System.Collections;
using UnityEngine;

public class ItemAsset : MonoBehaviour
{
    public static ItemAsset Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Transform pfItemCollectable;

    public Sprite cheeseSprite;
    public Sprite mushroomSprite;
    public Sprite pepperoniSprite;
}
