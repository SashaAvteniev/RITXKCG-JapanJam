using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private int currentPlayerCount = 0;

    [SerializeField]
    private TextWrapper _textPlayer1;
    [SerializeField]
    private TextWrapper _textPlayer2;


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
        _textPlayer1.SetText("WaiTing Connect Controller : Player1");
        _textPlayer2.SetText("WaiTing Connect Controller : Player2");
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
                _textPlayer1.SetText("Connect !! : Player1");
                break;
            case 1:
                _textPlayer2.SetText("Connect !! : Player2");
                break;
        }

        currentPlayerCount++;

        Debug.Log($"Player{currentPlayerCount}Joined");

        

        // プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            SceneManager.LoadScene("CharacterSelectScene");
        }
    }
}
