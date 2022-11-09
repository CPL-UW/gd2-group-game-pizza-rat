using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;
    private static GameObject player1, player2;
    private static GameObject dice;
    private Dictionary<Item.ItemType, int[]> ingredientsDict;

    public static int diceSideThrown = 0;
    public static int turn = 0;
    public static int[] player1StartWaypoint = new int[] { 0, 0 };
    public static int[] player2StartWaypoint = new int[] { 0, 9 };

    public static GameObject whosTurn = player1;
    public static bool gameOver = false;
    public static bool waitForDice = false;
    public Transform[][] waypoints;

    // Use this for initialization
    void Start () {
        Transform waypointParent = GameObject.Find("BoardWaypoints").GetComponent<Transform>();
        waypoints = new Transform[waypointParent.childCount][];
        for (int i = 0; i < waypointParent.childCount; i++) {
            Transform row = waypointParent.GetChild(i);
            waypoints[i] = new Transform[row.childCount];
            for (int j = 0; j < row.childCount; j++) {
                waypoints[i][j] = row.GetChild(j);
            }
        }

        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<Player>().moveAllowed = false;
        player2.GetComponent<Player>().moveAllowed = false;

        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);

        dice = GameObject.Find("Dice");

        ingredientsDict = new Dictionary<Item.ItemType, int[]>();
        SpawnItemCollectable();

        waitForDice = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForDice) SpawnItemCollectable();
        if (diceSideThrown == 0 && !waitForDice) {
            waitForDice = true;
            turn++;
            if (player1MoveText.activeSelf) {
                player1.GetComponent<Player>().moveAllowed = false;
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(true);
                dice.GetComponent<Dice>().RefreshDiceNumber(player2.GetComponent<Player>().maxDice);
            } else {
                player2.GetComponent<Player>().moveAllowed = false;
                player2MoveText.gameObject.SetActive(false);
                player1MoveText.gameObject.SetActive(true);
                dice.GetComponent<Dice>().RefreshDiceNumber(player1.GetComponent<Player>().maxDice);
            }
        }

        //if (reachDest(player1.GetComponent<Player>().waypointIndex,
        //    player1StartWaypoint, diceSideThrown))
        //{
        //    player1.GetComponent<Player>().moveAllowed = false;
        //    player1MoveText.gameObject.SetActive(false);
        //    player2MoveText.gameObject.SetActive(true);
        //    player1StartWaypoint = player1.GetComponent<Player>().waypointIndex - 1;
        //    //if (player1StartWaypoint == -1) player1StartWaypoint = waypoints.Length - 1;
        //}

        //if (reachDest(player2.GetComponent<Player>().waypointIndex,
        //    player2StartWaypoint, diceSideThrown))
        //{
        //    player2.GetComponent<Player>().moveAllowed = false;
        //    player2MoveText.gameObject.SetActive(false);
        //    player1MoveText.gameObject.SetActive(true);
        //    player2StartWaypoint = player2.GetComponent<Player>().waypointIndex - 1;
        //    //if (player2StartWaypoint == -1) player2StartWaypoint = waypoints.Length - 1;
        //}

        // Determind if the game ends
        //if (player1.GetComponent<FollowThePath>().waypointIndex == 
        //    player1.GetComponent<FollowThePath>().waypoints.Length)
        //{
        //    whoWinsText.gameObject.SetActive(true);
        //    player1MoveText.gameObject.SetActive(false);
        //    player2MoveText.gameObject.SetActive(false);
        //    whoWinsText.GetComponent<Text>().text = "Player 1 Wins";
        //    gameOver = true;
        //}

        //if (player2.GetComponent<FollowThePath>().waypointIndex ==
        //    player2.GetComponent<FollowThePath>().waypoints.Length)
        //{
        //    whoWinsText.gameObject.SetActive(true);
        //    player1MoveText.gameObject.SetActive(false);
        //    player2MoveText.gameObject.SetActive(false);
        //    whoWinsText.GetComponent<Text>().text = "Player 2 Wins";
        //    gameOver = true;
        //}
    }

    //private bool reachDest(int[] nextMoveIndex, int[] startIndex, int[] dist) {
    //    int currIndex = nextMoveIndex - 1;
    //    if (currIndex == -1) currIndex = waypoints.Length - 1;

    //    if (currIndex == (startIndex + dist) % waypoints.Length) return true;
    //    return false;
    //}

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                whosTurn = player1;
                player1.GetComponent<Player>().moveAllowed = true;
                break;
            case 2:
                whosTurn = player2;
                player2.GetComponent<Player>().moveAllowed = true;
                break;
        }
        waitForDice = false;
    }

    public void DecreaseIngredientCount(Item.ItemType type) {
        ingredientsDict.Remove(type);
    }

    private void SpawnItemCollectable() {
        if (turn%5 != 0) return;
        Array typeList = Enum.GetValues(typeof(Item.ItemType));
        for (int i=0; i< typeList.Length; i++) {
            Item.ItemType type = (Item.ItemType)typeList.GetValue(i);
            if (!ingredientsDict.ContainsKey(type)) {
                int x = Random.Range(0, waypoints.Length);
                int y = Random.Range(0, waypoints[0].Length);
                while (!checkPosOverlay(x, y)) {
                    x = Random.Range(0, waypoints.Length);
                    y = Random.Range(0, waypoints[0].Length);
                }
                ItemCollectable.SpawnItemCollectable(waypoints[x][y].position,
                    new Item { itemType = type, amount = 1 });
                ingredientsDict.Add(type, new int[] { x, y });
            }
        }
    }

    private bool checkPosOverlay(int x, int y) {
        int[] playerPos = player1.GetComponent<Player>().currIndex;
        if (x == playerPos[0] && y == playerPos[1]) return false;
        playerPos = player2.GetComponent<Player>().currIndex;
        if (x == playerPos[0] && y == playerPos[1]) return false;

        foreach (int[] pos in ingredientsDict.Values) {
            if (x == pos[0] && y == pos[1]) return false;
        }
        return true;
    }
}
