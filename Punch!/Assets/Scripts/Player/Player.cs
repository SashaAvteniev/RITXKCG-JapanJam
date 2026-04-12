using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Experimental.GraphView;
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
    [SerializeField]
    private float _timer;
    private bool _isGrounded;
    private bool _punchingStarted;
    private bool _punched;
    private int _timesPunched;
    private float _distancePerJump;
    private float _currentLineLength;
    [SerializeField]
    private Transform _spawnPoint;

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
        _timesPunched = 0;
        _distancePerJump = _maxLineLength / _numPunchesPerLine;
        _currentLineLength = _maxLineLength;
        this.transform.position = _spawnPoint.position;
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


        _velocity.y += _verticalVelocity.y * Time.fixedDeltaTime;

        _velocity.x = _horizontalVelocity.x;
        _velocity.z = _horizontalVelocity.z;

        this.transform.position = _velocity;

        this.transform.forward = _forwardVector;


    }

    private void Move()
    {
        if (_lastMoveInput.magnitude > 0 && !_punchingStarted && !_punched)
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
        _punchLine.SetPosition(0,new Vector3(this.transform.position.x, this.transform.position.y-.4f, this.transform.position.z));
        _punchLine.SetPosition(1, new Vector3((transform.position + _forwardVector.normalized * _currentLineLength).x, this.transform.position.y - .4f, (transform.position + _forwardVector.normalized * _currentLineLength).z));
        if (_punched)
        {
            _horizontalVelocity.x += _forwardVector.normalized.x * _distancePerJump * Time.fixedDeltaTime;
            _horizontalVelocity.z += _forwardVector.normalized.z * _distancePerJump * Time.fixedDeltaTime;
        }
        else
        {
            _horizontalVelocity.x = transform.position.x;
            _horizontalVelocity.z = transform.position.z;
        }

    }

    private void Punch()
    {
        if (!_punched)
        {
            if (_punchLine.enabled)
            {
                if (_timesPunched == 0)
                {
                    Debug.Log(_punched);
                    _punchingStarted = true;
                    _currentLineLength = _currentLineLength / 2;
                    _timesPunched++;


                    _verticalVelocity = Vector3.zero;
                    _verticalVelocity.y += _punchJumpSpeed;
                    _punched = true;
                    _puncher.OnPunch();
                    
                    if (_currentLineLength < _distancePerJump)
                    {
                        _currentLineLength = 0;
                    }
                    
                }
                else
                {
                    if (_punchingStarted)
                    {
                        _timesPunched++;

                        _verticalVelocity = Vector3.zero;
                        _verticalVelocity.y += _punchJumpSpeed;
                        _punched = true;
                        _isGrounded = false;
                        _puncher.OnPunch();
                        _currentLineLength /= 2;
                        if (_currentLineLength < _distancePerJump)
                        {
                            _currentLineLength = 0;
                        }
                        

                    }
                    if (_timesPunched > _numPunchesPerLine - 1)
                    {
                        _punchingStarted = false;
                        _timesPunched = 0;
                    }
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
            if (_velocity.y < 0)
            {
                _velocity.y = 0;
                _timer = _bounceTimeDelay;
                _isGrounded = true;
                _verticalVelocity = Vector3.zero;
                _punched = false;
                if (!_punchingStarted)
                {
                    _currentLineLength = _maxLineLength;
                    //_horizontalVelocity = Vector3.zero;
                }
            }
        }
    }
}
