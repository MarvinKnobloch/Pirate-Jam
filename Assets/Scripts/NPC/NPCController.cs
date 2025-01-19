using System.Collections;
using System.Linq;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float MoveSpeed;
    public int GatherAmount = 1;
    public float GatherTime = 1f;

    private bool _isGathering = false;
    private GatherResource _targetResource;
    private Coroutine _gatherCoroutine;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<GatherResource>() != null)
        {
            _isGathering = true;
        }
    }

    IEnumerator DoGather()
    {
        if (_targetResource != null)
        {
            _targetResource.Gather(GatherAmount);
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
        var foundResources = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<GatherResource>().ToArray();
        var closestResourceDistance = float.MaxValue;
        GatherResource closestResource = null;

        foreach (var resource in foundResources)
        {
            var distance = Vector2.Distance(transform.position, resource.transform.position);
            if (distance < closestResourceDistance)
            {
                closestResourceDistance = distance;
                closestResource = resource;
            }
        }

        if (closestResource != null)
        {
            _targetResource = closestResource;
        }
    }
}
