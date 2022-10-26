using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;
    private static GameObject player1Points, player2Points;
    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;
    public Transform[] waypoints;

    // Use this for initialization
    void Start () {

        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        GameObject parent1 = GameObject.Find("Player1Points");
        player1Points = parent1.transform.GetChild(0).gameObject;
        player1.GetComponent<Player>().textBox = player1Points.GetComponent<Text>();
        GameObject parent2 = GameObject.Find("Player2Points");
        player2Points = parent2.transform.GetChild(0).gameObject;
        player2.GetComponent<Player>().textBox = player2Points.GetComponent<Text>();

        player1.GetComponent<Player>().moveAllowed = false;
        player2.GetComponent<Player>().moveAllowed = false;

        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);

        SpawnItemCollectable(Item.ItemType.Cheese);
        SpawnItemCollectable(Item.ItemType.Mushroom);
        SpawnItemCollectable(Item.ItemType.Pepperoni);
    }

    // Update is called once per frame
    void Update()
    {
        // Determine when to stop
        if (reachDest(player1.GetComponent<Player>().waypointIndex,
            player1StartWaypoint, diceSideThrown))
        {
            player1.GetComponent<Player>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<Player>().waypointIndex - 1;
            if (player1StartWaypoint == -1) player1StartWaypoint = waypoints.Length - 1;
        }

        if (reachDest(player2.GetComponent<Player>().waypointIndex,
            player2StartWaypoint, diceSideThrown))
        {
            player2.GetComponent<Player>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<Player>().waypointIndex - 1;
            if (player2StartWaypoint == -1) player2StartWaypoint = waypoints.Length - 1;
        }

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

    private bool reachDest(int nextMoveIndex, int startIndex, int dist) {
        int currIndex = nextMoveIndex - 1;
        if (currIndex == -1) currIndex = waypoints.Length - 1;

        if (currIndex == (startIndex + dist) % waypoints.Length) return true;
        return false;
    }

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
    }

    public void SpawnItemCollectable(Item.ItemType type) {
        int waypointIndex = Random.Range(0, waypoints.Length);
        ItemCollectable.SpawnItemCollectable(waypoints[waypointIndex].position, new Item { itemType = type, amount = 1 });
    }

    public void UpdatePlayerPoints(Player player, int points) {

        player1Points.GetComponent<Text>().text = "" + points;
    }
}
