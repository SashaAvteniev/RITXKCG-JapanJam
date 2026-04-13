using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class MainGameManager : SingletonMonoBehaviour<MainGameManager>
{
    [SerializeField]
    private PlayerInput[] _playerPrefab;

    [SerializeField]
    private GameObject[] _playerLives;

    [SerializeField]
    private PlayerInfo _playerInfo;

    [SerializeField]
    private Vector3[] _playerPoses;

    public void Initialize()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        for (int i = 0; i < _playerInfo.PlayerDatas.Count; ++i)
        {
            // PlayerInputを所持した仮想のプレイヤーをインスタンス化
            // ※Join要求元のデバイス情報を紐づけてインスタンスを生成する
            var player = PlayerInput.Instantiate(
                prefab: _playerPrefab[_playerInfo.PlayerDatas[i].SelectedCharacterID].gameObject,
                playerIndex: i,
                pairWithDevice: _playerInfo.PlayerDatas[i].PairWithDevice
                );

            player.transform.position = _playerPoses[i];

            var playerInstance = player.GetComponent<Player>();
            playerInstance.PlayerID = i;
            playerInstance.SelectedCharacter = _playerInfo.PlayerDatas[i].SelectedCharacterID;
            playerInstance.Initialize();

            _playerLives[i].GetComponent<UnityEngine.UI.Image>().enabled = true;
            _playerLives[i].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }
}
