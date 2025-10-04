using UnityEngine;

namespace Units
{
    public interface ITargetable
    {
        public void TakeDamage(int damage);
        
        public Faction GetFaction();
        Vector3 GetPosition();

        public Unit GetUnit();
        
        Object Handle { get; }
    }
}