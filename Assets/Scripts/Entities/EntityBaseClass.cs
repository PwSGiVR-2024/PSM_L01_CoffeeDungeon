using UnityEngine;

public abstract class EntityBaseClass : MonoBehaviour
{
    [SerializeField] public int minAttack;
    [SerializeField] public int maxAttack;
    [SerializeField] public int health;
    [SerializeField] public int defence;
    public abstract void DealDamage();

    public abstract void TakeDamage();

    public virtual void Fallen()
    {
        print("");
    }
}
