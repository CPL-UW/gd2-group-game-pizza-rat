using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;
    private static GameObject dice;
    private Dictionary<Item.ItemType, int[]> ingredientsDict;

    public static int diceSideThrown = 0;
    public static GameObject player1, player2;
    public static int[] player1StartWaypoint = new int[] { 0, 0 };
    public static int[] player2StartWaypoint = new int[] { 0, 9 };

    public static GameObject whosTurn = player1;
    public static bool waitForDice = false;
    public static bool gameOver = false;
    public Transform[][] waypoints;
    public Transform winningPanel;
    public Transform canvas;
    public ChefManager selectedChefs;

    private void InitPlayer() {
        player1 = Instantiate(ImageAsset.Instance.pfPlayer, GameObject.Find("Players").transform).gameObject;
        player1.GetComponent<Player>().currIndex = player1StartWaypoint;

        Transform playerInfoUI = canvas.Find("UI_PlayerInfo").transform;
        Transform playerInfoTemplate = playerInfoUI.Find("PlayerInfoSlotContainer");
        Transform playerInfo = Instantiate(playerInfoTemplate, playerInfoUI);
        RectTransform playerInfoRect = playerInfo.GetComponent<RectTransform>();
        playerInfoRect.anchoredPosition = new Vector2(67, -111);
        playerInfoRect.anchorMin = new Vector2(0, 1);
        playerInfoRect.anchorMax = new Vector2(0, 1);
        playerInfoRect.pivot = new Vector2(0.5f, 0.5f);
        playerInfo.gameObject.SetActive(true);
        playerInfo.Find("PlayerIcon").GetComponent<Image>().sprite = ChefManager.GetSprite(selectedChefs.chefs[0]);

        player1.GetComponent<Player>().uiInfo = playerInfo;
        player1.GetComponent<SpriteRenderer>().sprite = ChefManager.GetSprite(selectedChefs.chefs[0]);
        player1MoveText = playerInfo.transform.Find("PlayerMoveText").gameObject;
        player1MoveText.gameObject.SetActive(true);

        player2 = Instantiate(ImageAsset.Instance.pfPlayer, GameObject.Find("Players").transform).gameObject;
        player2.GetComponent<Player>().currIndex = player2StartWaypoint;

        Transform playerInfo2 = Instantiate(playerInfoTemplate, playerInfoUI);
        RectTransform playerInfoRect2 = playerInfo2.GetComponent<RectTransform>();
        playerInfoRect2.anchoredPosition = new Vector2(-67, -111);
        playerInfoRect2.anchorMin = new Vector2(1, 1);
        playerInfoRect2.anchorMax = new Vector2(1, 1);
        playerInfoRect2.pivot = new Vector2(0.5f, 0.5f);
        playerInfo2.gameObject.SetActive(true);
        playerInfo2.Find("PlayerIcon").GetComponent<Image>().sprite = ChefManager.GetSprite(selectedChefs.chefs[1]);

        player2.GetComponent<Player>().uiInfo = playerInfo2;
        player2.GetComponent<SpriteRenderer>().sprite = ChefManager.GetSprite(selectedChefs.chefs[1]);
        player2MoveText = playerInfo2.transform.Find("PlayerMoveText").gameObject;
        player2MoveText.gameObject.SetActive(false);
    }

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


        InitPlayer();

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

        // Determind if the game ends
        if (player1.GetComponent<Player>().points >= 5) {
            endGame(player1);
        }
        else if (player2.GetComponent<Player>().points >= 5) {
            endGame(player2);
        }
    }

    void Restart() {
        SceneManager.LoadScene("Menu");
    }

    private void endGame(GameObject winner) {
        gameOver = true;
        winningPanel.gameObject.SetActive(true);
        winningPanel.Find("Text").GetComponent<TextMeshProUGUI>().text = "Congrats!\n" + winner.name + " Won!";
        winningPanel.Find("Image").GetComponent<Image>().sprite = winner.GetComponent<SpriteRenderer>().sprite;
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(false);
        player2MoveText.gameObject.SetActive(false);
        Invoke("Restart", 4f);
    }

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
