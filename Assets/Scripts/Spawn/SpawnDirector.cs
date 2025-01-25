using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    public List<Spawnable> Spawnables;

    private Dictionary<int, List<GameObject>> _instances = new();
    private Player _player;

    void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _instances = Spawnables.ToDictionary(s => s.GetInstanceID(), s => new List<GameObject>());
        Spawnables.ForEach(s => StartCoroutine(NPCDirector(s)));
    }

    IEnumerator NPCDirector(Spawnable spawnable)
    {
        yield return new WaitForSeconds(spawnable.SpawnTickRate);

        if (Random.Range(0f, 1f) <= spawnable.SpawnChance)
        {
            if (_instances[spawnable.GetInstanceID()].Count() < spawnable.MaxInstances)
            {
                var enemiesToSpawn = Mathf.Min(
                    spawnable.MaxInstances - _instances.Count(),
                    spawnable.SpawnInstancesPerTick
                );
                for (var i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemy(spawnable);
                }
            }
        }

        yield return NPCDirector(spawnable);
    }

    void SpawnEnemy(Spawnable spawnable)
    {
        if (Player.Instance == null) return;

        var spawnPosition = SpawnUtil.GetSpawnPointAroundTarget(
            (Vector2)_player.transform.position,
            spawnable.SpawnMagnitude
        );

        var enemy = Instantiate(spawnable.InstancePrefab, spawnPosition, Quaternion.identity);
        var component = enemy.AddComponent<DirectorInstance>();
        component.SpawnDirector = this;
        component.Key = spawnable.GetInstanceID();
        _instances[spawnable.GetInstanceID()].Add(enemy);
    }

    public void Delete(int key, GameObject instance)
    {
        if (_instances.TryGetValue(key, out var instanceList))
        {
            instanceList.Remove(instance);
        }
    }
}
