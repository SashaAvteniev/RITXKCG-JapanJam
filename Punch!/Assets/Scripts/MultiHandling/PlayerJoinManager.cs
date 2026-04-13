using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design.Serialization;

/// <summary>
/// プレイヤーの入退室の管理クラス（アウトゲーム）
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    // プレイヤーがゲームにJoinするためのInputAction
    [SerializeField] 
    private InputAction playerJoinInputAction = default;
    // PlayerInputがアタッチされているプレイヤーオブジェクト
    [SerializeField] 
    private PlayerInput playerPrefab = default;
    // 最大参加人数
    [SerializeField] 
    private int maxPlayerCount = default;

    [SerializeField]
    private PlayerInfo _playerInfo;

    // Join済みのデバイス情報
    private InputDevice[] joinedDevices = default;
    // 現在のプレイヤー数
    public static int currentPlayerCount = 0;

    [SerializeField]
    private TextWrapper _textPlayer1;
    [SerializeField]
    private TextWrapper _textPlayer2;

    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private GameObject playerTextPrefab;

    public List<GameObject> players;
    public List<Sprite> deviceSprites;

    private void Awake()
    {
        // 最大参加可能数で配列を初期化
        joinedDevices = new InputDevice[maxPlayerCount];

        // InputActionを有効化し、コールバックを設定
        playerJoinInputAction.Enable();
        playerJoinInputAction.performed += OnJoin;

        _playerInfo.Initialize();

        _textPlayer1.Initialize();
        _textPlayer2.Initialize();
        _textPlayer1.SetText("Waiting Connect Controller : Player 1");
        _textPlayer2.SetText("Waiting Connect Controller : Player 2");

        // Initialize player list and add player 1
        players = new List<GameObject>();
        AddPlayerText(players.Count);
    }

    private void OnDestroy()
    {
        playerJoinInputAction.Dispose();
    }

    /// <summary>
    /// デバイスによってJoin要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            return;
        }

        // Join要求元のデバイスが既に参加済みのとき、処理を終了
        foreach (var device in joinedDevices)
        {
            if (context.control.device == device)
            {

                return;
            }
        }

        // PlayerInputを所持した仮想のプレイヤーをインスタンス化
        // ※Join要求元のデバイス情報を紐づけてインスタンスを生成する
        PlayerInput.Instantiate(
            prefab: playerPrefab.gameObject,
            playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device
            );

        // Joinしたデバイス情報を保存
        joinedDevices[currentPlayerCount] = context.control.device;

        _playerInfo.PlayerDatas.Add(new PlayerData(context.control.device, 0));

        switch (currentPlayerCount)
        {
            case 0:
                _textPlayer1.SetText("Connect !! : Player 1");
                break;
            case 1:
                _textPlayer2.SetText("Connect !! : Player 2");
                break;
        }

        // If keyboard connected
        if (context.control.device is Keyboard)
        {
            UnityEngine.Debug.Log("Keyboard joined.");
            UnityEngine.UI.Image deviceImage = players[currentPlayerCount].GetComponentInChildren<UnityEngine.UI.Image>();
            deviceImage.sprite = deviceSprites[0];
            RectTransform deviceImageRect = deviceImage.GetComponent<RectTransform>();
            deviceImageRect.sizeDelta = new Vector2(1506f, 433f);
            deviceImageRect.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            deviceImage.enabled = true;
        }
        // If controller connected
        else
        {
            UnityEngine.Debug.Log("Controller joined.");
            UnityEngine.UI.Image deviceImage = players[currentPlayerCount].GetComponentInChildren<UnityEngine.UI.Image>();
            deviceImage.sprite = deviceSprites[1];
            RectTransform deviceImageRect = deviceImage.GetComponent<RectTransform>();
            deviceImageRect.sizeDelta = new Vector2(1179f, 845f);
            deviceImageRect.localScale = new Vector3(0.10f, 0.10f, 0.10f);
            deviceImage.enabled = true;
        }

        // Increment player count and add a new player
        currentPlayerCount++;
        AddPlayerText(currentPlayerCount);

        UnityEngine.Debug.Log($"Player{currentPlayerCount}Joined");

        // プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            SceneManager.LoadScene("CharacterSelectScene");
        }
    }

    private void AddPlayerText(int playerCount)
    {
        GameObject playerTextInstance = Instantiate(playerTextPrefab, canvas);
        RectTransform rect = playerTextInstance.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        switch (playerCount)
        {
            case 0:
                rect.anchoredPosition = new Vector2(-500f, 200f);
                break;
            case 1:
                rect.anchoredPosition = new Vector2(500f, 200f);
                break;
            case 2:
                rect.anchoredPosition = new Vector2(-500f, -150f);
                break;
            case 3:
                rect.anchoredPosition = new Vector2(500f, -150f);
                break;
        }

        playerTextInstance.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + (playerCount + 1);

        players.Add(playerTextInstance);
    }
}
