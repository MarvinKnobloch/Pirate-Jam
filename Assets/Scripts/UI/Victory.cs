using UnityEngine;

public class Victory : MonoBehaviour
{
    private void OnDestroy()
    {
        PlayerUI.Instance.Victory();
    }
}

