using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UpgradeSystem;

public class NPCController : MonoBehaviour
{
    public float MoveSpeed;
    public int GatherAmount = 1;
    public float GatherTime = 1;
    private float lookGatherTimer;

    private bool _isGathering = false;
    private GatherResource _targetResource;
    private Coroutine _gatherCoroutine;

    [NonSerialized] public Health health;

    [SerializeField] private LayerMask gatherLayer;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        GatherTime = GatherTime - Upgrades.Instance.GetUpgradeStat(Upgrades.UpgradeType.GatherSpeed);
    }
    void FixedUpdate()
    {
        if (!_isGathering)
        {
            if (_targetResource == null)
            {
                FindClosestResource();
            }
            else
            {
                MoveToResource();
            }
        }
        else
        {
            if (_targetResource == null)
            {
                _isGathering = false;
            }
            else
            {
                if (_gatherCoroutine == null)
                {
                    _gatherCoroutine = StartCoroutine(DoGather());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & gatherLayer) != 0)
        {
            _isGathering = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & gatherLayer) != 0)
        {
            _isGathering = false;
        }
    }


    IEnumerator DoGather()
    {
        if (_targetResource != null)
        {
            var collected = _targetResource.Gather(GatherAmount);
            Player.Instance.AddResource(_targetResource.Type, collected);
        }

        yield return new WaitForSeconds(GatherTime);
        _gatherCoroutine = null;
    }

    void MoveToResource()
    {
        var direction = _targetResource.transform.position - transform.position;
        transform.position += MoveSpeed * Time.fixedDeltaTime * direction.normalized;
    }

    void FindClosestResource()
    {
        lookGatherTimer += Time.fixedDeltaTime;

        if (lookGatherTimer > 0.1f)
        {
            if (PlayerUI.Instance.allressources.Count != 0)
            {
                int randomNumber = UnityEngine.Random.Range(0, PlayerUI.Instance.allressources.Count);
                _targetResource = PlayerUI.Instance.allressources[randomNumber];
            }
            else _targetResource = null;
        }
        else
        {
            _targetResource = null;
        }

        //foreach (var resource in foundResources)
        //{
        //    var distance = Vector2.Distance(transform.position, resource.transform.position);
        //    if (distance < closestResourceDistance)
        //    {
        //        closestResourceDistance = distance;
        //        closestResource = resource;
        //    }
        //}
    }
}
