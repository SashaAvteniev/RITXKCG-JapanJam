using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ControllerInputListener _inputListner;
    [SerializeField]
    private GroundPuncher _puncher;

    
    [SerializeField]
    private float _moveSpeed; // for test]
    [SerializeField]
    private float _baseJumpSpeed;
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _punchJumpSpeed;
    private Vector3 _velocity;
    private float _verticalVelocity;

    [SerializeField]
    private float _bounceTimeDelay;
    private float _timer;
    private bool _isGrounded;
    private bool _punched;

    private Vector2 _lastMoveInput;

    private void Start()
    {
        _inputListner.Initialzie();
        _inputListner.OnLStickInputCallBack = (input) => _lastMoveInput = input;
        _inputListner.OnPunchInputPressedCallBack = Punch;
        _timer = _bounceTimeDelay;
        _isGrounded = true;
        _punched = false;
        _verticalVelocity = 0;
    }
    private void FixedUpdate()
    {
        if(_timer > 0)
        {
            _timer -= Time.fixedDeltaTime;
        }
        
        Jump();
        Move();
        Gravity();

        _velocity.y += _verticalVelocity * Time.deltaTime;
        this.transform.position = _velocity;

    }

    private void Move()
    {
        Vector3 pos = this.transform.position;
        pos.x += _lastMoveInput.x * _moveSpeed * Time.fixedDeltaTime;
        pos.z += _lastMoveInput.y * _moveSpeed * Time.fixedDeltaTime;
        _velocity.x = pos.x;
        _velocity.z = pos.z;
    }

    private void Punch()
    {
        Debug.Log(_verticalVelocity);
        if (!_punched)
        {
            _verticalVelocity = 0;
            _verticalVelocity += _punchJumpSpeed;
            _punched = true;
            _puncher.OnPunch();
        }
    }

    private void Jump() 
    {
        if (_isGrounded && _timer < 0)
        {
             _verticalVelocity = _baseJumpSpeed;
            _isGrounded = false;
        }
    }

    private void Gravity()
    {
        if (!_isGrounded) 
        {
            _verticalVelocity -= _gravity;
            if (_velocity.y < -.2)
            {
                _velocity.y = 0;
                _timer = _bounceTimeDelay;
                _isGrounded = true;
                _verticalVelocity = 0;
                _punched = false;
            }
        }
    }
}
