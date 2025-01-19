using UnityEngine;

public class SpawnUtil
{
    public static Vector2 GetSpawnPointAroundTarget(Vector2 target, float spawnMagnitude)
    {
        var unitCircle = Random.insideUnitCircle;
        var unitCircleMagnitude = unitCircle.magnitude;
        var spawnUnitCircle = Vector2.zero;

        spawnUnitCircle.x = unitCircle.x * spawnMagnitude / unitCircleMagnitude;
        spawnUnitCircle.y = unitCircle.y * spawnMagnitude / unitCircleMagnitude;

        return target + spawnUnitCircle;
    }
}
