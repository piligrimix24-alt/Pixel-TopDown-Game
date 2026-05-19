using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackForce = 0.5f;
    [SerializeField] private float knockBackMovingTimeMax = 0.2f;
    public bool isGettingKnockedBack { get; private set; }

    private float _knockBackMovingTime;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _knockBackMovingTime -= Time.deltaTime;
        if (_knockBackMovingTime < 0)
            StopKnockBackMovement();

    }
    public void GetKnockedBack(Transform damageSourse)
    {
        isGettingKnockedBack = true;
        _knockBackMovingTime = knockBackMovingTimeMax;
        Vector2 diff = (transform.position - damageSourse.position).normalized * knockBackForce / _rb.mass;
        _rb.AddForce(diff, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        isGettingKnockedBack = false;
        _rb.linearVelocity = Vector2.zero;   //у дяди rb.velocity = Vector2.zero; смотри чтоб работало
    }
}
