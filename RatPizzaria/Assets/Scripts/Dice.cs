using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 0;
    private bool coroutineAllowed = true;
    private int maxDiceNum = 0;

    // Use this for initialization
    private void Awake () {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (!GameControl.gameOver && GameControl.diceSideThrown == 0 && coroutineAllowed)
                StartCoroutine("RollTheDice");
        }
        
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(1, this.maxDiceNum);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        GameControl.MovePlayer(whosTurn);
        whosTurn = (whosTurn+1) % 4;

        coroutineAllowed = true;
    }

    public void RefreshDiceNumber(int maxDiceNum) {
        this.maxDiceNum = maxDiceNum;
        rend.sprite = diceSides[maxDiceNum - 1];
    }
}
