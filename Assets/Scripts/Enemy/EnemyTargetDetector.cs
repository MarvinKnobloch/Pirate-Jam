using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    public List<GameObject> Targets = new();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            Targets.Add(other.gameObject);
            SortTargetsByDistance();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            Targets.Remove(other.gameObject);
            SortTargetsByDistance();
        }
    }

    private void SortTargetsByDistance()
    {
        Targets.Sort((a, b) =>
        {
            var distanceA = Vector2.Distance(transform.position, a.transform.position);
            var distanceB = Vector2.Distance(transform.position, b.transform.position);

            return distanceA.CompareTo(distanceB);
        });
    }
}
