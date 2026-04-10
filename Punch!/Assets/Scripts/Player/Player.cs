using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ControllerInputListener _inputListner;

    [SerializeField]
    private float _moveSpeed; // for test]

    private Vector2 _lastMoveInput;

    private void Start()
    {
        _inputListner.Initialzie();
        _inputListner.OnLStickInputCallBack = (input) => _lastMoveInput = input;
        _inputListner.OnPunchInputPressedCallBack = Punch;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var pos = this.transform.position;
        pos.x += _lastMoveInput.x * _moveSpeed * Time.fixedDeltaTime;
        pos.z += _lastMoveInput.y * _moveSpeed * Time.fixedDeltaTime;
        this.transform.position = pos;
    }

    private void Punch()
    {
        // TODO:Punch process write here
        Debug.Log("Punch!");
    }
}
