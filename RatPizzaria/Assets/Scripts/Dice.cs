using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;
    private int maxDiceNum = 0;

    // Use this for initialization
    private void Start () {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        maxDiceNum = GameObject.Find("Player1").GetComponent<Player>().maxDice;
        rend.sprite = diceSides[maxDiceNum - 1];
	}

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && GameControl.diceSideThrown == 0 && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, this.maxDiceNum);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        GameControl.MovePlayer(whosTurn);
        whosTurn++;
        if (whosTurn == 3) whosTurn = 1;

        coroutineAllowed = true;
    }

    public void RefreshDiceNumber(int maxDiceNum) {
        this.maxDiceNum = maxDiceNum;
        rend.sprite = diceSides[maxDiceNum - 1];
    }
}
