using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Movement"), Range(1, 10)]
    public float MoveSpeed;

    [Header("Combat")]
    public float AttackDistance = 2f;
    public float AttackCooldown = 1f;
    public int AttackDamage = 1;

    [NonSerialized] public Health Health;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private float _attackTimer = 0f;
    private bool _isAttacking = false;
    private float _maxMovementSpeed;
    private EnemyTargetDetector _targetDetector;
    private GameObject _attackTarget;

    private float slowPercentage;
    private float slowDuration;

    void OnEnable()
    {
        _attackTimer = AttackCooldown;
        _maxMovementSpeed = MoveSpeed;

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        _collider.radius = AttackDistance;

        Health = GetComponent<Health>();
        _targetDetector = GetComponentInChildren<EnemyTargetDetector>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("NPC"))
        {
            _isAttacking = true;
            _attackTarget = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("NPC"))
        {
            _isAttacking = false;
            _attackTarget = null;
        }
    }

    void FixedUpdate()
    {
        if (!_isAttacking)
        {
            MoveToPlayerOrTarget();
        }
        else
        {
            HandleAttack();
        }
    }

    public void DoSlow(float _slowPercentage, float _slowDuration)
    {
        if (MoveSpeed >= (_maxMovementSpeed * _slowPercentage))          //if new Slow is worse do nothing
        {
            StopCoroutine("Slow");

            MoveSpeed = _maxMovementSpeed;
            slowPercentage = _slowPercentage;
            slowDuration = _slowDuration;

            StartCoroutine("Slow");
        }
    }

    IEnumerator Slow()
    {
        MoveSpeed *= slowPercentage;
        yield return new WaitForSeconds(slowDuration);
        Debug.Log("slow");
        MoveSpeed = _maxMovementSpeed;
    }

    private void HandleAttack()
    {
        if (_attackTimer <= 0)
        {
            if (_attackTarget != null)
            {
                _attackTarget.GetComponent<Health>().TakeDamage(AttackDamage);
            }
            _attackTimer = AttackCooldown;
        }
        else
        {
            _attackTimer -= Time.fixedDeltaTime;
        }
    }

    private void MoveToPlayerOrTarget()
    {
        var nearestTarget = _targetDetector.Targets.ElementAtOrDefault(0);
        Vector3 targetPosition;

        if (nearestTarget != null)
        {
            targetPosition = nearestTarget.transform.position;
        }
        else
        {
            targetPosition = Player.Instance.transform.position;

        }

        var direction = (targetPosition - transform.position).normalized;
        _rigidbody.MovePosition(transform.position + MoveSpeed * Time.fixedDeltaTime * direction);
    }
}
