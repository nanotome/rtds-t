using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;
    // Event to be fired off when a LivingEntity dies
    public event System.Action OnDeath;

    protected float health;
    protected bool dead = false;

    // virtual allows the Start method to be called in derived classes
    // For example base.Start()
    // This ensures behaviour common to all derived classes are implemented
    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        Destroy(gameObject);
    }

    // This method assumes the amount is positive
    public void UpdateHealth(int amount)
    {
        health += amount;
    }
}
