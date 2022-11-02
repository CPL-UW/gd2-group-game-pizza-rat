using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;
    private static GameObject player1, player2;
    private static GameObject dice;

    public static int diceSideThrown = 0;
    public static int turn = 0;
    public static int[] player1StartWaypoint = new int[] { 0, 0 };
    public static int[] player2StartWaypoint = new int[] { 0, 9 };

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

        foreach (Item.ItemType type in Enum.GetValues(typeof(Item.ItemType))){
            SpawnItemCollectable(type);
        }

        waitForDice = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine when to stop
        if (diceSideThrown == 0 && !waitForDice) {
            waitForDice = true;
            if (player1MoveText.activeSelf) {
                player1.GetComponent<Player>().moveAllowed = false;
                player1MoveText.gameObject.SetActive(false);
                player2MoveText.gameObject.SetActive(true);
                turn++;
                dice.GetComponent<Dice>().RefreshDiceNumber(player2.GetComponent<Player>().maxDice);
            } else {
                player2.GetComponent<Player>().moveAllowed = false;
                player2MoveText.gameObject.SetActive(false);
                player1MoveText.gameObject.SetActive(true);
                turn++;
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
                player1.GetComponent<Player>().moveAllowed = true;
                break;
            case 2:
                player2.GetComponent<Player>().moveAllowed = true;
                break;
        }
        waitForDice = false;
    }

    public void SpawnItemCollectable(Item.ItemType type) {
        int x = Random.Range(0, waypoints.Length);
        int y = Random.Range(0, waypoints[0].Length);
        ItemCollectable.SpawnItemCollectable(waypoints[x][y].position, new Item { itemType = type, amount = 1 });
    }
}
