using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    public event EventHandler OnPlayerTakeHit;
    public event EventHandler OnPlayerDeath;

    [SerializeField] private float movingSpeed;
    private readonly float _baseSpeed = 5f;
    private readonly float _shiftSpeed = 10f;
    //private readonly float _dashSpeed = 15f;
    private readonly float _minSpeed = 0.1f;
    private int _directionRunning = 1;

    [SerializeField] private int maxHealth = 10;
    private int _currentHealth;
    private float _damageRecoveryTime = 0.5f;

    private bool _isRunning = false;
    private bool _isShiftRunning = false;
    private bool _isAttacking = false;
    public bool _allowedToMove = true;
    private bool _canTakeDamage = true;
    private bool _isAlive = true;

    private float _attackAnimationDuration = 0.1f;

    Vector2 inputVector;
    //=================================================================================================================
    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }
    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerShift += GameInput_OnPlayerShift;
        GameInput.Instance.OnPlayerShiftReleased += GameInput_OnPlayerShiftReleased;
        //GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
        movingSpeed = _baseSpeed;
        _currentHealth = maxHealth;
    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        if (_allowedToMove && !_knockBack.isGettingKnockedBack)
        {
            HandleMovement();
        }
    }
    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerShift -= GameInput_OnPlayerShift;
        GameInput.Instance.OnPlayerShiftReleased -= GameInput_OnPlayerShiftReleased;
    }
    //=================================================================================================================
    public Vector3 GetPlayerPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
    public int DirectionRunning() => _directionRunning;
    public bool IsRunning() => _isRunning;
    public bool IsShiftRunning() => _isShiftRunning;
    public bool IsAttacking() => _isAttacking;
    public bool IsAlive() => _isAlive;
    public void TakeDamage(Transform damageSourse, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            _knockBack.GetKnockedBack(damageSourse);
            StartCoroutine(DamageRecoveryRoutine());
            Debug.Log(_currentHealth);
            OnPlayerTakeHit?.Invoke(this, EventArgs.Empty);
        }
        DetectDeath();
    }
    //=================================================================================================================
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }
    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            GameInput.Instance.DisableMovement();
        }
    }
    public void ResetAttack()
    {
        _isAttacking = false;
        _allowedToMove = true;
    }
    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minSpeed || Mathf.Abs(inputVector.x) > _minSpeed)
        {
            _isRunning = true;
            if (inputVector.x > 0)
            {
                _directionRunning = 1;
            }
            else
            {
                _directionRunning = -1;
            }
        }
        else
        {
            _isRunning = false;
            _directionRunning = 0;
        }
    }
    //private void Dash()
    //{
    //    movingSpeed = _dashSpeed;
    //}
    //============================================================================
    private void GameInput_OnPlayerShift(object sender, System.EventArgs e)
    {
        if (_isRunning)
        {
            movingSpeed = _shiftSpeed;
            _isShiftRunning = true;
        }
        else
        {
            _isShiftRunning = false;
        }
    }
    private void GameInput_OnPlayerShiftReleased(object sender, System.EventArgs e)
    {
        movingSpeed = _baseSpeed;
        _isShiftRunning = false;
    }
    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
        _isAttacking = true;
        _allowedToMove = false;
        Invoke(nameof(ResetAttack), _attackAnimationDuration);
    }
    //private void GameInput_OnPlayerDash(object sender, EventArgs e)
    //{
    //    Dash();
    //}
}