using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public List<PlayerData> PlayerDatas;

    public void Initialize()
    {
        PlayerDatas ??= new();
        PlayerDatas.Clear();
    }
}

[System.Serializable]
public class PlayerData
{
    public InputDevice PairWithDevice { get; private set; } = default;
    public int SelectedCharacterID = default;

    public PlayerData(InputDevice pairWithDevice, int selectedCharacterID)
    {
        PairWithDevice = pairWithDevice;
        SelectedCharacterID = selectedCharacterID;
    }
}