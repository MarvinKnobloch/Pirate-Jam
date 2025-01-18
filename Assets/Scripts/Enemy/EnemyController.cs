using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Movement"), Range(1, 10)]
    public float MoveSpeed;

    [Header("Combat")]
    public float AttackDistance = 2f;
    public float AttackCooldown = 1f;
    public float Health = 10f;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private float _attackTimer = 0f;
    private bool _isAttacking = false;

    void OnEnable()
    {
        _attackTimer = AttackCooldown;

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        _collider.radius = AttackDistance;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Boig");
        if (other.collider.CompareTag("Player"))
        {
            _isAttacking = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            _isAttacking = false;
        }
    }

    void FixedUpdate()
    {
        if (!_isAttacking)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        var playerPosition = Player.Instance.transform.position;
        var direction = (playerPosition - transform.position).normalized;
        _rigidbody.MovePosition(transform.position + direction * MoveSpeed * Time.fixedDeltaTime);
    }
}
