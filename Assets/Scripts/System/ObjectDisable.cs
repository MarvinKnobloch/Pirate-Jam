using System.Collections;
using UnityEngine;

public class ObjectDisable : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisabelObj());
    }
    IEnumerator DisabelObj()
    {
        yield return null;
        gameObject.SetActive(false);
    }
}
