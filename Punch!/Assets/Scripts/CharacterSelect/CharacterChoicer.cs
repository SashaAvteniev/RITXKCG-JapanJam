using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterChoicer : MonoBehaviour
{
    [SerializeField]
    private ButtonMenuController _buttonController;

    [SerializeField]
    private ControllerInputListener _controllerListner;

    [SerializeField]
    private PlayerInfo _playerInfo;

    [SerializeField]
    private int _playerCount = 2;

    private int _chooseNumber = 0;
    private int _currentSelectPlayer = 0;

    private void Start()
    {
        _playerCount = PlayerJoinManager.currentPlayerCount;
        _buttonController.Initialize(CreateButtonFunc());

        _controllerListner.OnLStickInputCallBack = MoveSelect;
        _controllerListner.OnPunchInputPressedCallBack = SubmitSelect;

        Action[] CreateButtonFunc()
        {
            return new Action[]
                {
                    () => _chooseNumber = (int)eCharacterType.Sora,
                    () => _chooseNumber = (int)eCharacterType.Nasu,
                    () => _chooseNumber = (int)eCharacterType.Ichigo,
                    () => _chooseNumber = (int)eCharacterType.Kaeru,
                };
        }
    }

    private void MoveSelect(Vector2 input)
    {
        _buttonController.MoveButton(GetDirection());

        eDirection GetDirection()
        {
            return (Mathf.Abs(input.x), Mathf.Abs(input.y)) switch
            {
                ( < 0.5f, < 0.5f) => eDirection.None,
                ( > 0, < 0.5f) => input.x > 0 ? eDirection.Right : eDirection.Left,
                ( < 0.5f, > 0) => input.y > 0 ? eDirection.Up : eDirection.Down,
                _ => input.x > 0 ? input.y > 0 ? eDirection.UpRight : eDirection.DownRight : input.y > 0 ? eDirection.UpLeft : eDirection.DownLeft,
            };
        }
    }

    private void SubmitSelect()
    {
        _buttonController.SelectButton();

        switch (_currentSelectPlayer)
        {
            case 0:
                _playerInfo.PlayerDatas[0].SelectedCharacterID = _chooseNumber-1;
                EventDispatcher.Instance.Dispatch("OnSelected0", _chooseNumber-1);
                break;
            case 1:
                _playerInfo.PlayerDatas[1].SelectedCharacterID = _chooseNumber-1;
                EventDispatcher.Instance.Dispatch("OnSelected1", _chooseNumber-1);
                break;
            case 2:
                _playerInfo.PlayerDatas[2].SelectedCharacterID = _chooseNumber;
                EventDispatcher.Instance.Dispatch("OnSelected2", _chooseNumber);
                break;
            case 3:
                _playerInfo.PlayerDatas[3].SelectedCharacterID = _chooseNumber;
                EventDispatcher.Instance.Dispatch("OnSelected3", _chooseNumber);
                break;
        }

        ++_currentSelectPlayer;

        if (_currentSelectPlayer < _playerCount)
            return;

        SceneManager.LoadScene("PlayScene");
    }
}
