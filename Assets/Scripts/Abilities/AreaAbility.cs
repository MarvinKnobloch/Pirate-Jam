using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class AreaAbility : MonoBehaviour
{
    [SerializeField] Vector2 areaSize;
    [SerializeField] private AreaType areaType;
    [SerializeField] private float lifeTime;
    [SerializeField] private float tickInterval;
    [SerializeField] private int damage;
    [SerializeField] private int heal;
    [Range(0f, 1f)]
    [SerializeField] private float speedReduction;
    [SerializeField] private float speedReductionTime;
    [SerializeField] private LayerMask hitLayer;

    private enum AreaType
    {
        Circle,
        Box,
    }
    private void Awake()
    {
        transform.localScale = areaSize;
    }
    private void Start()
    {
        CancelInvoke();
        Destroy(gameObject, lifeTime);
        InvokeRepeating("ExecuteAbility", 0.1f, tickInterval);
    }
    private void ExecuteAbility()
    {
        switch (areaType)
        {
            case AreaType.Circle:
                CheckInCircle();
                break;
        }
    }
    private void CheckInCircle()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 0.5f, hitLayer);

        foreach (Collider2D obj in cols)
        {
            if (damage != 0)
            {
                if (obj.gameObject.transform.parent.TryGetComponent(out Health parentHealth))
                {
                    parentHealth.TakeDamage(damage);
                }
            }
            if (heal != 0)
            {

            }
            if (speedReduction != 0)
            {
                if (obj.gameObject.transform.parent.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.DoSlow(speedReduction, speedReductionTime);
                }
            }
        }
    }
}
