using UnityEngine;

[CreateAssetMenu(fileName = "Spawnable", menuName = "ScriptableObjects/Spawnable")]
public class Spawnable : ScriptableObject
{
    public GameObject InstancePrefab;
    public int MaxInstances;
    public float SpawnTickRate;
    public int SpawnInstancesPerTick;
    public int SpawnMinMagnitude;
    public int SpawnMaxMagnitude;
    public float SpawnChance;

    public bool spawnOnGameStart;
}
