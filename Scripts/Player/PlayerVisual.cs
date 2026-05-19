using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private FlashBlink _flashBlink;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_SHIFT_RUNNING = "IsShiftRunning";
    private const string IS_ATTACKING = "IsAttacking";
    //private const string IS_HURT = "IsHurt";
    private const string IS_DEAD = "IsDead";
    //=================================================================================================================
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();
    }
    private void Start()
    {
        //Player.Instance.OnPlayerTakeHit += Player_OnPlayerTakeHit;
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }
    private void Update()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        _animator.SetBool(IS_SHIFT_RUNNING, Player.Instance.IsShiftRunning());
        _animator.SetBool(IS_ATTACKING, Player.Instance.IsAttacking());
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
    }
    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
    //=================================================================================================================
    private void AdjustPlayerFacingDirection()
    {
        int directionRunning = Player.Instance.DirectionRunning();
        if (directionRunning < 0) { _spriteRenderer.flipX = true; }
        else if (directionRunning > 0) { _spriteRenderer.flipX = false; }
        else
        {
            Vector3 mousePosition = GameInput.Instance.GetMousePosition();
            Vector3 playerPosition = Player.Instance.GetPlayerPosition();
            if (mousePosition.x < playerPosition.x) { _spriteRenderer.flipX = true; }
            else { _spriteRenderer.flipX = false; }
        }
    }
    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        _animator.SetTrigger(IS_DEAD);
        _flashBlink.StopBlinking();
    }
    //       Анимация удара (с задержкой почему то)
    //private void Player_OnPlayerTakeHit(object sender, System.EventArgs e)
    //{
    //    //_animator.SetTrigger(IS_HURT);
    //}
    //public void TriggerEndAttackAnimation()
    //{
    //    sword.AttackColliderTurnOff();
    //}
}
