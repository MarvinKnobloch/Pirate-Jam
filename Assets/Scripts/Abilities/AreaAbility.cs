using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class AreaAbility : MonoBehaviour
{
    [SerializeField] Vector2 areaSize;
    [SerializeField] private AreaType areaCollider;
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
        InvokeRepeating("SetAreaCollider", 0.1f, tickInterval);
    }
    private void SetAreaCollider()
    {
        switch (areaCollider)
        {
            case AreaType.Circle:
                CheckInCircle();
                break;
            case AreaType.Box:
                CheckInBox();
                break;
        }
    }
    private void CheckInCircle()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 0.5f, hitLayer);

        ExecuteAbility(cols);
    }
    private void CheckInBox()
    {
        float angle = transform.eulerAngles.z;
        if(angle > 180)
        {
            float difference = angle - 180;
            angle = (180 - difference) * -1;
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, areaSize, angle, hitLayer);

        ExecuteAbility(cols);
    }
    private void ExecuteAbility(Collider2D[] cols)
    {

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
