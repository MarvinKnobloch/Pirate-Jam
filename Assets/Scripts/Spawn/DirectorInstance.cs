using UnityEngine;

public class DirectorInstance : MonoBehaviour
{
    public SpawnDirector SpawnDirector;
    public int Key;

    private void OnDestroy()
    {
        SpawnDirector.Delete(Key, gameObject);
    }
}
