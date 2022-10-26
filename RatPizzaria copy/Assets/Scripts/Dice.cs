using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;
    private int players = 4;

	// Use this for initialization
	private void Start () {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
	}

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        if (whosTurn == 1) {
            GameControl.MovePlayer(1);
        } else if (whosTurn == 2) {
            GameControl.MovePlayer(2);
        } else if (whosTurn == 3) {
            GameControl.MovePlayer(3);
        } else if (whosTurn == 4) {
            GameControl.MovePlayer(4);
        }
        whosTurn += 1;
        if (whosTurn > players) {
            whosTurn = 1;
        }
        coroutineAllowed = true;
    }
}
