using System;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SlimeVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    private Animator _animator;
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiloier";
    private const string ATTACK = "Attack";
    private const string HURT = "Hurt";
    private const string IS_DEAD = "IsDead";
    SpriteRenderer _spriteRenderer;
    //=================================================================================================================
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }
    private void Update()
    {
        _animator.SetBool(IS_RUNNING, enemyAI.IsRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, enemyAI.GetRoamingAnimationSpeed());
    }
    public void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= _enemyEntity_OnTakeHit;
        enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }
    //=================================================================================================================
    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }
    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }
    //=================================================================================================================
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(HURT);
    }
    private void _enemyEntity_OnDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IS_DEAD, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }
}
