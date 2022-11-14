using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour {

    private static GameObject dice;
    private Dictionary<Item.ItemType, int[]> ingredientsDict;

    public static int diceSideThrown = 0;
    public static List<Player> playerList = new List<Player>();
    public static int[,] playerStartWaypoint = new int[4, 2] { { 0, 0 }, { 0, 9 }, { 7, 0 }, { 7, 9 } };

    public static GameObject whosTurn;
    public static bool waitForDice = false;
    public static bool gameOver = false;
    public Transform[][] waypoints;
    public Transform winningPanel;
    public Transform canvas;
    public ChefManager selectedChefs;

    private void InitPlayer() {
        Transform playerInfoUI = canvas.Find("UI_PlayerInfo").transform;
        Transform playerInfoTemplate = playerInfoUI.Find("PlayerInfoSlotContainer");
        for (int i=0; i<2; i++) {
            Transform playerInfo = Instantiate(playerInfoTemplate, playerInfoUI);
            playerInfo.gameObject.SetActive(true);
            playerInfo.transform.Find("PlayerMoveText").gameObject.SetActive(false);
            playerInfo.Find("PlayerIcon").GetComponent<Image>().sprite = ChefManager.GetSprite(selectedChefs.chefs[i]);
            RectTransform playerInfoRect = playerInfo.GetComponent<RectTransform>();
            switch (i) {
                case 0:
                    playerInfoRect.anchoredPosition = new Vector2(67, -111);
                    playerInfoRect.anchorMin = new Vector2(0, 1);
                    playerInfoRect.anchorMax = new Vector2(0, 1);
                    playerInfoRect.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case 1:
                    playerInfoRect.anchoredPosition = new Vector2(-67, -111);
                    playerInfoRect.anchorMin = new Vector2(1, 1);
                    playerInfoRect.anchorMax = new Vector2(1, 1);
                    playerInfoRect.pivot = new Vector2(0.5f, 0.5f);
                    break;
            }

            GameObject player = Instantiate(ImageAsset.Instance.pfPlayer, GameObject.Find("Players").transform).gameObject;
            player.GetComponent<SpriteRenderer>().sprite = ChefManager.GetSprite(selectedChefs.chefs[i]);
            player.GetComponent<Player>().currIndex[0] = playerStartWaypoint[i, 0];
            player.GetComponent<Player>().currIndex[1] = playerStartWaypoint[i, 1];
            player.GetComponent<Player>().uiInfo = playerInfo;
            player.GetComponent<Player>().FinishSetUpPlayer();

            playerList.Add(player.GetComponent<Player>());
        }
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
        whosTurn = playerList[0].gameObject;
        whosTurn.GetComponent<Player>().moveText.SetActive(true);

        dice = GameObject.Find("Dice");
        dice.GetComponent<Dice>().RefreshDiceNumber(playerList[0].maxDice);
        waitForDice = true;

        ingredientsDict = new Dictionary<Item.ItemType, int[]>();
        SpawnItemCollectable();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForDice) SpawnItemCollectable();
        if (diceSideThrown == 0 && !waitForDice) {
            waitForDice = true;
            if (playerList[0].moveText.activeSelf) {
                playerList[0].moveAllowed = false;
                playerList[0].moveText.gameObject.SetActive(false);
                playerList[1].moveText.gameObject.SetActive(true);
                dice.GetComponent<Dice>().RefreshDiceNumber(playerList[1].maxDice);
            } else {
                playerList[1].moveAllowed = false;
                playerList[1].moveText.gameObject.SetActive(false);
                playerList[0].moveText.gameObject.SetActive(true);
                dice.GetComponent<Dice>().RefreshDiceNumber(playerList[0].maxDice);
            }
        }

        // Determind if the game ends
        if (playerList[0].points >= 5) {
            endGame(playerList[0].gameObject);
        }
        else if (playerList[1].points >= 5) {
            endGame(playerList[1].gameObject);
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
        for (int i=0; i<2; i++) {
            playerList[i].gameObject.SetActive(false);
            playerList[i].moveText.gameObject.SetActive(false);
        }
        Invoke("Restart", 4f);
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                whosTurn = playerList[0].gameObject;
                playerList[0].moveAllowed = true;
                break;
            case 2:
                whosTurn = playerList[1].gameObject;
                playerList[1].moveAllowed = true;
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
        for (int i=0; i<2; i++) {
            int[] playerPos = playerList[i].currIndex;
            if (x == playerPos[0] && y == playerPos[1]) return false;
        }

        foreach (int[] pos in ingredientsDict.Values) {
            if (x == pos[0] && y == pos[1]) return false;
        }
        return true;
    }
}
