using System.Collections;
using UnityEngine;
using System;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText, player3MoveText, player4MoveText;

    private static GameObject player1, player2, player3, player4;

    public static int diceSideThrown = 0;
    //var player1Coords = Tuple.Create(0, 0);
    //var player2Coords = Tuple.Create(8, 0);
    //var player3Coords = Tuple.Create(8, 8);
    //var player4Coords = Tuple.Create(0, 8);

    public static bool gameOver = false;
    //public Transform[] waypoints;

    // Use this for initialization
    void Start () {

        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");
        player3MoveText = GameObject.Find("Player3MoveText");
        player4MoveText = GameObject.Find("Player4MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
        player4 = GameObject.Find("Player4");

        player1.GetComponent<Player>().moveAllowed = false;
        player2.GetComponent<Player>().moveAllowed = false;
        player3.GetComponent<Player>().moveAllowed = false;
        player4.GetComponent<Player>().moveAllowed = false;

        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
        player3MoveText.gameObject.SetActive(false);
        player4MoveText.gameObject.SetActive(false);

        player1.GetComponent<Player>().x = 0;player1.GetComponent<Player>().y = 0;
        player2.GetComponent<Player>().x = 8;player2.GetComponent<Player>().y = 0;
        player3.GetComponent<Player>().x = 8;player3.GetComponent<Player>().y = 8;
        player4.GetComponent<Player>().x = 0;player4.GetComponent<Player>().y = 8;

        SpawnItemCollectable(Item.ItemType.Cheese);
        SpawnItemCollectable(Item.ItemType.Mushroom);
        SpawnItemCollectable(Item.ItemType.Pepperoni);
    }

    // Update is called once per frame
    void Update()
    {
        // Determine when to stop
        if (player1.GetComponent<Player>().done == true) {
            //player1.GetComponent<Player>().moveAllowed = false;
            player2.GetComponent<Player>().done = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
        }
        if (player2.GetComponent<Player>().done == true) {
            player3.GetComponent<Player>().done = false;
            //player2.GetComponent<Player>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player3MoveText.gameObject.SetActive(true);
        }
        if (player3.GetComponent<Player>().done == true) {
            player4.GetComponent<Player>().done = false;
            //player3.GetComponent<Player>().moveAllowed = false;
            player3MoveText.gameObject.SetActive(false);
            player4MoveText.gameObject.SetActive(true);
        }
        if (player4.GetComponent<Player>().done == true) {
            player1.GetComponent<Player>().done = false;
            player4.GetComponent<Player>().moveAllowed = false;
            player4MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
        }
    }

    /*private bool reachDest(int nextMoveIndex, int startIndex, int dist) {
        int currIndex = nextMoveIndex - 1;
        if (currIndex == -1) currIndex = waypoints.Length - 1;

        if (currIndex == (startIndex + dist) % waypoints.Length) return true;
        return false;
    } */

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player2.GetComponent<Player>().done = false;
                player1.GetComponent<Player>().moveAllowed = true;
                break;
            case 2:
                player3.GetComponent<Player>().done = false;
                player2.GetComponent<Player>().moveAllowed = true;
                break;
            case 3:
                player4.GetComponent<Player>().done = false;
                player3.GetComponent<Player>().moveAllowed = true;
                break;
            case 4:
                player1.GetComponent<Player>().done = false;
                player4.GetComponent<Player>().moveAllowed = true;
                break;
        }
    }

    public void SpawnItemCollectable(Item.ItemType type) {
       /* int itemX = Random.Range(0, 7);
        int itemY = Random.Range(0, 7);
        // Make Random Positions
        ItemCollectable.SpawnItemCollectable(player1.position, new Item { itemType = type, amount = 1 });
        */
    }
}
