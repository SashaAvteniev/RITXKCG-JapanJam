using System.Runtime.CompilerServices;
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

    private Vector3 _forwardVector;
    [SerializeField]
    private LineRenderer _punchLine;
    [SerializeField]
    private float _maxLineLength;
    [SerializeField]
    private int _numPunchesPerLine;
    private float _timeInJump;

    private Vector3 _verticalVelocity;
    private Vector3 _horizontalVelocity;

    [SerializeField]
    private float _bounceTimeDelay;
    private float _timer;
    private bool _isGrounded;
    private bool _punchingStarted;
    private bool _punched;
    private int _timesPunched;

    private Vector2 _lastMoveInput;

    private void Start()
    {
        _inputListner.Initialzie();
        _inputListner.OnLStickInputCallBack = (input) => _lastMoveInput = input;
        _inputListner.OnPunchInputPressedCallBack = Punch;
        _timer = _bounceTimeDelay;
        _isGrounded = true;
        _punched = false;
        _verticalVelocity.y = 0;
        _punchLine.enabled = false;
        _punchingStarted = false;
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

        if (_punched)
        {
            _timeInJump += Time.fixedDeltaTime;
        }

        _velocity += _verticalVelocity * Time.fixedDeltaTime;

        if(_horizontalVelocity.x > 0 || _horizontalVelocity.z > 0)
        {
            _velocity += _horizontalVelocity * Time.fixedDeltaTime;
        }

        this.transform.position = _velocity;

        this.transform.forward = _forwardVector;


    }

    private void Move()
    {
        /*
        Vector3 pos = this.transform.position;
        pos.x += _lastMoveInput.x * _moveSpeed * Time.fixedDeltaTime;
        pos.z += _lastMoveInput.y * _moveSpeed * Time.fixedDeltaTime;
        _velocity.x = pos.x;
        _velocity.z = pos.z;
        */
        if (_lastMoveInput.magnitude > 0 && !_punchingStarted)
        {
            _forwardVector.x = _lastMoveInput.x;
            _forwardVector.z = _lastMoveInput.y;
            _forwardVector.y = 0;

            _punchLine.enabled = true;
        }
        else if (_lastMoveInput.magnitude == 0 &&  !_punchingStarted) 
        {
            _punchLine.enabled = false;
        }
        _punchLine.SetPosition(1, transform.position + _forwardVector * _maxLineLength);

        float distancePerJump = _maxLineLength / _numPunchesPerLine;
        Vector3 nextPoint = transform.position + _forwardVector * distancePerJump;
        if (_punched)
        {
            // _velocity = this.transform.position;
            _horizontalVelocity.x += nextPoint.x*Time.fixedDeltaTime;
            _horizontalVelocity.z += nextPoint.z*Time.fixedDeltaTime;
        }
        else
        {
            _horizontalVelocity = Vector3.zero;
        }


    }

    private void Punch()
    {
        float distancePerJump = _maxLineLength / _numPunchesPerLine;

        if (_timesPunched == 0)
        {
            _punchingStarted = true;

        }
        _timesPunched++;
        if (_timesPunched > _numPunchesPerLine)
        {
            _punchingStarted = false;
        }
        if (_punchingStarted)
        {
            if (_punchLine.enabled)
            {
                if (!_punched)
                {
                    _verticalVelocity = Vector3.zero;
                    _verticalVelocity.y += _punchJumpSpeed;
                    _punched = true;
                    _puncher.OnPunch();
                }
            }
        }


    }

    private void Jump() 
    {
        if (_isGrounded && _timer < 0)
        {
             _verticalVelocity.y = _baseJumpSpeed;
            _isGrounded = false;
        }
    }

    private void Gravity()
    {
        if (!_isGrounded) 
        {
            _verticalVelocity.y -= _gravity;
            if (_velocity.y < -.2)
            {
                _velocity.y = 0;
                _timer = _bounceTimeDelay;
                _isGrounded = true;
                _verticalVelocity = Vector3.zero;
                _punched = false;
                _timeInJump = 0;
            }
        }
    }
}
