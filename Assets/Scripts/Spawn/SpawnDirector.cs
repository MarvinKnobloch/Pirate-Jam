using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    public GameObject InstancePrefab;
    public int MaxInstances;
    public float SpawnTickRate;
    public int SpawnInstancesPerTick;
    public int SpawnMagnitude;

    private List<GameObject> _instances = new();
    private Player _player;


    IEnumerator Start()
    {
        _player = FindFirstObjectByType<Player>();
        yield return StartCoroutine(NPCDirector());
    }

    IEnumerator NPCDirector()
    {
        yield return new WaitForSeconds(SpawnTickRate);

        if (_instances.Count() < MaxInstances)
        {
            var enemiesToSpawn = Mathf.Min(MaxInstances - _instances.Count(), SpawnInstancesPerTick);
            for (var i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }
        }


        yield return NPCDirector();
    }

    void SpawnEnemy()
    {
        var spawnPosition = SpawnUtil.GetSpawnPointAroundTarget((Vector2)_player.transform.position, SpawnMagnitude);

        var enemy = Instantiate(InstancePrefab, spawnPosition, Quaternion.identity);
        _instances.Add(enemy);
    }
}
