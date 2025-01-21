using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    void Start()
    {
        var targetFps = PlayerPrefs.GetInt("FPS", 60);
        Application.targetFrameRate = targetFps;
    }
}
