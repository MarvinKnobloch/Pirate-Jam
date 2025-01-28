using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Movement"), Range(0, 10)]
    public float MoveSpeed;

    [Header("Combat")]
    public float AttackDistance = 2f;
    public float AttackCooldown = 1f;
    public int AttackDamage = 1;

    [Header("Exp")]
    public int experienceGain;

    [NonSerialized] public Health health;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private float targetUpdateTime;
    private GameObject nearestTarget;
    private float _attackTimer = 0f;
    private bool _isAttacking = false;
    private float _maxMovementSpeed;
    private EnemyTargetDetector _targetDetector;
    private GameObject _attackTarget;
    private float _slowPercentage;
    private float _slowDuration;
    private bool _isStunned = false;
    private float _stunDuration;

    void OnEnable()
    {
        _attackTimer = AttackCooldown;
        _maxMovementSpeed = MoveSpeed;

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        _collider.radius = AttackDistance;

        _targetDetector = GetComponentInChildren<EnemyTargetDetector>();

        health = GetComponent<Health>();
        if (health != null) health.dieEvent.AddListener(OnDeath);
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
            _rigidbody.linearVelocity = Vector2.zero;
            HandleAttack();
        }
    }

    public void DoStun(float stunDuration)
    {
        StopCoroutine("Stun");

        _stunDuration = stunDuration;
        StartCoroutine("Stun");
    }

    IEnumerator Stun()
    {
        //Debug.Log(_stunDuration);
        _isStunned = true;
        health.HealthBarImage.color = Color.gray;
        yield return new WaitForSeconds(_stunDuration);
        if (MoveSpeed != _maxMovementSpeed) health.HealthBarImage.color = Color.blue;
        else health.HealthBarImage.color = Color.red;
        _isStunned = false;
    }

    public void DoSlow(float slowPercentage, float slowDuration)
    {
        if (MoveSpeed >= (_maxMovementSpeed * slowPercentage))          //if new Slow is worse do nothing
        {
            StopCoroutine("Slow");

            MoveSpeed = _maxMovementSpeed;
            this._slowPercentage = slowPercentage;
            this._slowDuration = slowDuration;

            StartCoroutine("Slow");
        }
    }

    IEnumerator Slow()
    {
        MoveSpeed *= _slowPercentage;
        health.HealthBarImage.color = Color.blue;
        yield return new WaitForSeconds(_slowDuration);
        health.HealthBarImage.color = Color.red;
        MoveSpeed = _maxMovementSpeed;
    }

    private void HandleAttack()
    {
        if (_isStunned) return;
        if (Player.Instance == null) return;

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
        if (_isStunned || Player.Instance == null)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        targetUpdateTime += Time.fixedDeltaTime;
        if (targetUpdateTime > 0.1f)
        {
            targetUpdateTime = 0;
            nearestTarget = _targetDetector.Targets.ElementAtOrDefault(0);
        }

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
    public void OnDeath()
    {
        PlayerUI.Instance.expController.PlayerGainExp(experienceGain);
        PlayerUI.Instance.KillCountUpdate();
    }
}
