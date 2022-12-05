using System.Collections;
using UnityEngine;

public class ImageAsset : MonoBehaviour
{
    public static ImageAsset Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Transform pfItemCollectable;
    public Transform pfPlayer;

    public Sprite doughSprite;
    public Sprite sauceSprite;
    public Sprite cheeseSprite;
    public Sprite mushroomSprite;
    public Sprite pepperoniSprite;
    public Sprite onionSprite;
    public Sprite jalapenoSprite;
    public Sprite pepperSprite;
    
    public Sprite OnionPizzaSprite;
    public Sprite PepperoniPizzaSprite;
    public Sprite CheesePizzaSprite;
    public Sprite JalapenoPizzaSprite;
    public Sprite PepperPizzaSprite;
    public Sprite MushroomPizzaSprite;

    public Sprite PepperMushroomPizzaSprite;
    public Sprite CheeseOnionPizzaSprite;
    public Sprite CheesePepperPizzaSprite;
    public Sprite JalapenoPepperoniPizzaSprite;
    public Sprite JalapenoOnionPizzaSprite;
    public Sprite MushroomPepperoniPizzaSprite;
    public Sprite CheeseJalapenoPizzaSprite;

    public Sprite PepperoniMushroomOnionPizzaSprite;
    public Sprite PepperoniPepperMushroomPizzaSprite;
    public Sprite CheeseJalapenoOnionPizzaSprite;
    public Sprite CheesePepperoniPepperPizzaSprite;
    public Sprite JalapenoPepperoniMushroomPizzaSprite;
    public Sprite JalapenoOnionOnionPizzaSprite;
    public Sprite CheeseMushroomPepperoniPizzaSprite;
    public Sprite CheesePepperJalapenoPizzaSprite;

    public Sprite BiggECheeseSprite;
    public Sprite TopHatSprite;
    public Sprite ClownRatSprite;
    public Sprite MasterSilverSprite;
    public Sprite NYCStreetRatSprite;
    public Sprite JerrieSprite;

    public Sprite BiggECheeseGlow;
    public Sprite TopHatGlow;
    public Sprite ClownRatGlow;
    public Sprite MasterSilverGlow;
    public Sprite NYCStreetRatGlow;
    public Sprite JerrieGlow;
}
