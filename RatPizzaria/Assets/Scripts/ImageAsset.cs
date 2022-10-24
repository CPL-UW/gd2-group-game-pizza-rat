using System.Collections;
using UnityEngine;

public class ImageAsset : MonoBehaviour
{
    public static ImageAsset Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Transform pfItemCollectable;

    public Sprite cheeseSprite;
    public Sprite mushroomSprite;
    public Sprite pepperoniSprite;
    public Sprite cheesePizzaSprite;
    public Sprite mushroomPizzaSprite;
    public Sprite pepperoniPizzaSprite;
}
