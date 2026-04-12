using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerConnectManager : MonoBehaviour
{
    [SerializeField]
    private int _playerValue;
    [SerializeField]
    private PlayerInfo _playerInfo;

    private int _playerCount = default;

    private void Start()
    {
        _playerInfo.Initialize();
        _playerCount = 0;
    }

    // プレイヤー入室時に受け取る通知
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //_playerInfo.PlayerDatas.Add(new(playerInput.devices[0]))
    }

    // プレイヤー退室時に受け取る通知
    public void OnPlayerLeft(PlayerInput playerInput)
    {

    }
}
