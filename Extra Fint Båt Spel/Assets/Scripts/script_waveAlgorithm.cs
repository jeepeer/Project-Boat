using UnityEngine;

public class script_waveAlgorithm : MonoBehaviour
{
    public static script_waveAlgorithm instance;

    [SerializeField] private float amplitude = 0.3f;
    [SerializeField] private float length = 2f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 0f;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
        if (offset >= 180f || offset <= -180f) offset -= 180f;
    }

    public float GetWaveHeight(float x)
    {
        
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
