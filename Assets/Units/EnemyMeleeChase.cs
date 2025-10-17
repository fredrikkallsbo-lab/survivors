using Battlefield.GameMechanics.Combat.BattlefieldController;
using Units;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMeleeChase : MonoBehaviour
{
    BattlefieldController battlefieldController;
    public float speed = 25f;

    private Rigidbody2D rb;

    void Awake()
    {
        battlefieldController = GameObject.Find("GameCoordinator").GetComponent<BattlefieldController>();
        rb = GetComponent<Rigidbody2D>();
        // Good practice for physics-driven objects
        rb.freezeRotation = true;

    }

    void FixedUpdate()
    {
        Unit target = battlefieldController.GetBattlefieldUnitInterface()
            .GetClosestUnitOfFaction(Faction.Player, transform, 100, LayerMask.GetMask("Ally"));

        // Direction towards target
        Vector2 direction = (target.GetPosition() - transform.position).normalized;
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;

        // Physics-friendly move
        rb.MovePosition(newPos);
    }
}