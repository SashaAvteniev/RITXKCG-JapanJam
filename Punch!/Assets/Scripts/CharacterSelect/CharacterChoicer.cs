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
    private CharacterSelectData _selectData;

    [SerializeField]
    private int _playerValue = 2;

    private int _chooseNumber = 0;
    private int _currentSelectPlayer = 0;

    private void Start()
    {
        _buttonController.Initialize(CreateButtonFunc());

        _controllerListner.OnLStickInputCallBack = MoveSelect;
        _controllerListner.OnPunchInputPressedCallBack = SubmitSelect;

        _selectData.Initialize();

        Action[] CreateButtonFunc()
        {
            return new Action[]
                {
                    () => _chooseNumber = 0,
                    () => _chooseNumber = 1,
                    () => _chooseNumber = 2,
                    () => _chooseNumber = 3,
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
                _selectData.Player1Character = _chooseNumber;
                EventDispatcher.Instance.Dispatch("OnSelected0", _chooseNumber);
                break;
            case 1:
                _selectData.Player1Character = _chooseNumber;
                EventDispatcher.Instance.Dispatch("OnSelected1", _chooseNumber);
                break;
        }

        ++_currentSelectPlayer;

        if (_currentSelectPlayer < _playerValue)
            return;

        SceneManager.LoadScene("PlayScene");
    }
}
