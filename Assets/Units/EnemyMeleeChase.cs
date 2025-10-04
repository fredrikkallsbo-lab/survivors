using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMeleeChase : MonoBehaviour
{
    public Transform target;   // Assign in Inspector
    public float speed = 25f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Good practice for physics-driven objects
        rb.freezeRotation = true;
        target = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Direction towards target
        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;

        // Physics-friendly move
        rb.MovePosition(newPos);
    }
}