using UnityEngine;

public abstract class EntityBaseClass : MonoBehaviour
{
    protected int minAttack;
    protected int maxAttack;
    protected int health;
    protected int defence;
    protected abstract void DealDamage();

    protected abstract void TakeDamage();

    protected virtual void Fallen()
    {
        print("");
    }

}
