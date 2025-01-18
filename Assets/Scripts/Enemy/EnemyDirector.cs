using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public static EnemyDirector Instance;
    public GameObject EnemyPrefab;
    public int MaxEnemies;
    public int SpawnEnemiesPerTick;
    public int SpawnMagnitude;

    private List<EnemyController> _enemies = new();
    private Player _player;


    IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _player = FindFirstObjectByType<Player>();
        yield return StartCoroutine(NPCDirector());
    }

    IEnumerator NPCDirector()
    {
        yield return new WaitForSeconds(1f);

        if (_enemies.Count() < MaxEnemies)
        {
            var enemiesToSpawn = Mathf.Min(MaxEnemies - _enemies.Count(), SpawnEnemiesPerTick);
            for (var i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }
        }


        yield return NPCDirector();
    }

    void SpawnEnemy()
    {
        var playerPosition = _player.transform.position;
        var playerPosition2D = (Vector2)playerPosition;
        var unitCircle = Random.insideUnitCircle;
        var unitCircleMagnitude = unitCircle.magnitude;
        var spawnUnitCircle = Vector2.zero;

        spawnUnitCircle.x = unitCircle.x * SpawnMagnitude / unitCircleMagnitude;
        spawnUnitCircle.y = unitCircle.y * SpawnMagnitude / unitCircleMagnitude;

        var spawnPosition = playerPosition2D + spawnUnitCircle;

        var enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        var enemyController = enemy.GetComponent<EnemyController>();
        _enemies.Add(enemyController);
    }
}
