using UnityEngine;

[DefaultExecutionOrder(-1000)] 
public class GameTime : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool respectTimeScale = true;

    bool paused;
    double now;

    public double Now => now;                         // gameplay time (seconds)
    public bool IsPaused => paused || (respectTimeScale && Time.timeScale == 0f);

    public float Delta
    {
        get
        {
            if (IsPaused) return 0f;
            float baseDt = respectTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            return baseDt * Mathf.Max(0f, speed);
        }
    }

    void Update()
    {
        now += Delta;
    }

    public void SetPaused(bool value) => paused = value;
    public void SetSpeed(float value) => speed = Mathf.Max(0f, value);
    public void SetRespectTimeScale(bool value) => respectTimeScale = value;
}