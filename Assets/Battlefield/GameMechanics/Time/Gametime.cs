using UnityEngine;

[DefaultExecutionOrder(-1000)] // tick before most things
public class GameTime : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    bool paused;
    double now;

    public double Now => now;                       // gameplay time (seconds)
    public float Delta => paused ? 0f : Time.unscaledDeltaTime * speed;
    public bool IsPaused => paused;

    void Update() => now += Delta;

    public void SetPaused(bool value) => paused = value;
    public void SetSpeed(float value) => speed = Mathf.Max(0f, value);
}