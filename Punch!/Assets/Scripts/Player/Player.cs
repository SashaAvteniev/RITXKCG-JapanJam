using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum eCharacterType
{
    Sora,
    Nasu,
    Ichigo,
    Kaeru
}

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
    [SerializeField]
    private float _punchWait;
    private float _punchTimer;
    private float _distancePerJump;
    private float _currentLineLength;
    [SerializeField]
    private Transform _spawnPoint;

    private Vector2 _lastMoveInput;
    [SerializeField]
    private LayerMask _groundMask;
    private bool falling;
    private Vector3 _distanceGone;
    private bool _respawning;
    private float _respawnTimer;
    [SerializeField]
    public int playerNumber;
    [Header("Status")]
    [SerializeField]
    private int _life;
    public int PlayerID
    { get; set; }
    public int SelectedCharacter
    { get; set; }

    public void Initialize()
    {
        _inputListner.Initialzie();
        _inputListner.OnLStickInputCallBack = (input) => _lastMoveInput = input;
        _inputListner.OnPunchInputPressedCallBack = Punch;
        _timer = _bounceTimeDelay;
        _isGrounded = false;
        _punched = false;
        _verticalVelocity.y = 0;
        _punchLine.enabled = false;
        _punchingStarted = false;
        _timesPunched = 0;
        _distancePerJump = _maxLineLength / _numPunchesPerLine;
        _currentLineLength = _maxLineLength;
        this.transform.position = _spawnPoint.position;
        _punchTimer = 0;
        falling = false;
        _distanceGone = Vector3.zero;
        _respawnTimer = 0;
        _respawning = false;
        EventDispatcher.Instance.Dispatch($"SetPlayerImage{PlayerID}", SelectedCharacter);
        WinnerTracker.instance.playerList.Add(this.gameObject);
    }
    private void FixedUpdate()
    {
        if (_life > 0)
        {
            if (_respawning)
            {
                _respawnTimer += Time.fixedDeltaTime;
                if (_respawnTimer > 3)
                {
                    this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
                    _isGrounded = false;
                    _respawnTimer = 0;
                    _respawning = false;
                    _punchingStarted = false;
                    _timesPunched = 0;
                    _punchTimer = _punchWait;

                }
            }
            if (_timer > 0)
            {
                _timer -= Time.fixedDeltaTime;
            }
            if (_punched)
            {
                var hit = Physics.Raycast(
                    this.transform.position,
                    Vector3.down,
                    out RaycastHit hitObj,
                    3,
                    _groundMask.value
                );
                if (!hit)
                {
                    falling = true;

                }
                else
                {
                    falling = false;
                }
                if (falling && transform.position.y < -16)
                {
                    _respawning = true;
                    falling = false;
                    GetDamage();
                }

            }


            Move();
            Gravity();


            _velocity.y += _verticalVelocity.y * Time.fixedDeltaTime;

            _velocity.x = _horizontalVelocity.x;
            _velocity.z = _horizontalVelocity.z;
            if (!_respawning)
            {
                this.transform.position = _velocity;

                this.transform.forward = _forwardVector;
            }


            if (!_punchingStarted)
            {
                _punchTimer -= Time.fixedDeltaTime;
            }
        }
        
    }

    private void Move()
    {
        //Makes movement only up down left right
        if (_lastMoveInput.magnitude > 0 && !_punchingStarted && !_punched)
        {
            if (_lastMoveInput.x > 0 && _lastMoveInput.x > Mathf.Abs(_lastMoveInput.y))
            {
                _forwardVector = new Vector3 (1, 0, 0);

            }
            if (_lastMoveInput.x < 0 && Mathf.Abs(_lastMoveInput.x) > Mathf.Abs(_lastMoveInput.y))
            {
                _forwardVector = new Vector3(-1, 0, 0);

            }
            if (_lastMoveInput.y > 0 && _lastMoveInput.y > Mathf.Abs(_lastMoveInput.x))
            {
                _forwardVector = new Vector3(0, 0, 1);

            }
            if (_lastMoveInput.y < 0 && Mathf.Abs(_lastMoveInput.y) > Mathf.Abs(_lastMoveInput.x))
            {
                _forwardVector = new Vector3(0, 0, -1);

            }
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
            _horizontalVelocity.x += _forwardVector.x * _distancePerJump * Time.fixedDeltaTime;
            _horizontalVelocity.z += _forwardVector.z * _distancePerJump * Time.fixedDeltaTime;
        }
        else if(!_respawning && !_punched) 
        {
            Debug.Log(_horizontalVelocity.x);
            _horizontalVelocity.x = transform.position.x;
            _horizontalVelocity.z = transform.position.z;
        }

    }

    private void Punch()
    {
        if (!_punched && _punchTimer < 0)
        {
            if (_punchLine.enabled)
            {
                if (_timesPunched == 0)
                {
                    _punchingStarted = true;
                    _currentLineLength = _currentLineLength / 2;
                    _timesPunched++;


                    _verticalVelocity = Vector3.zero;
                    _verticalVelocity.y += _punchJumpSpeed;
                    _punched = true;
                    _isGrounded = false;
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
                        _punchTimer = _punchWait;
                    }
                }
            }
        }
    }

    private void Gravity()
    {
        if (!_isGrounded) 
        {
            _verticalVelocity.y -= _gravity*Time.fixedDeltaTime;
            if (_velocity.y < .2 && !falling)
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

    private void GetDamage()
    {
        _life--;
        if(_life <= 0)
        {
            WinnerTracker.instance.playerList.Remove(this.gameObject);
        }
        EventDispatcher.Instance.Dispatch($"OnLifeChanged{PlayerID}", _life);
    }
}
