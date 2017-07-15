using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent( typeof(NavMeshAgent))]
public class Enemy : LivingEntity, IItemCase {

    public float rangeOfSight; // how far the Enemy can see
    public float rangeOfAttack; // how far the Enemy can attack
    public LayerMask viewMask; // layer on which obstacles to sight exist
    public Transform attackSpawn;
    public Projectile attackType;

    public ParticleSystem deathEffect;
    // List of items that may be spawned when the Enemy dies
    public List<Pack> deathItems;

    public float msBetweenAttacks;
    float nextAttackTime;
    float attackSpeed = 0f;
    float attackLifeTime = .2f;

    enum State { Idle, Chasing, Attacking };
    State currentState;

    private NavMeshAgent navMeshAgent;
    Transform target;
    LivingEntity targetEntity;
    bool hasTarget;

    float collisionRadius;
    float targetCollisionRadius;

    Color flashColor = Color.white;
    float flashDuration = .15f;
    Material skinMaterial;
    Color originalColor;

    System.Random prng = new System.Random();

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().sharedMaterial;
        originalColor = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            // Get a reference to the Player
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            // The Player and Enemy should collide on their surfaces and not their centres.
            collisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    protected override void Start()
    {
        base.Start();

        if (hasTarget)
        {
            currentState = State.Idle;

            // Listen for the Player's death event
            targetEntity.OnDeath += OnTargetDeath;

            // Cast a ray from the Enemy to the Player
            StartCoroutine(UpdatePath());
        }
    }

    private void Update()
    {
        if (hasTarget)
        {
            if (CanAttackPlayer())
            {
                Attack();
            }

            if (CanSeePlayer())
            {
                transform.LookAt(target.transform.position);
            }
        }
    }

    // check if the Enemy can see the Player
    bool CanSeePlayer()
    {
        // 1. Check the distance between the Enemy and the Player
        if (Vector3.Distance(transform.position, target.position) < rangeOfSight)
        {
            // 2. Cast a ray from the Enemy to the Player. If it doesn't hit anything, the Enemy can see the Player
            if (!Physics.Linecast(transform.position, target.position, viewMask))
            {
                return true;
            }
        }

        return false;
    }

    // check if the Enemy can attack the Player
    bool CanAttackPlayer()
    {
        // 1. Check the distance between the Enemy and the Player
        if (Vector3.Distance(transform.position, target.position) < rangeOfAttack)
        {
            return true;
        }

        return false;
    }

    // Calculate a new path to the target four times a second.
    // This is more performant than calculating the path every frame
    // (if this was in the update method).
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while(hasTarget)
        {
            if(CanSeePlayer())
            {
                // When a LivingEntity dies, its transform remains but its
                // navMeshAgent does not.
                if(!dead)
                {
                    navMeshAgent.enabled = true;
                    currentState = State.Chasing;

                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPos = target.position - dirToTarget * (collisionRadius + targetCollisionRadius);

                    navMeshAgent.SetDestination(targetPos);
                }  
            } else
            {
                if (!dead)
                {
                    navMeshAgent.enabled = false; // this stops the Enemy from chasing the Player
                }
            }
            
            yield return new WaitForSeconds(refreshRate);
        }
    }

    void Attack()
    {
        if (Time.time > nextAttackTime)
        {
            // The Enemy might not have been chasing the Player before attacking
            // so we store the previous state in order to return to it when
            // the Enemy is done attacking.
            State prevState = currentState;
            currentState = State.Attacking;
            // The Enemy should not be chasing the Player while attacking.
            navMeshAgent.enabled = false;

            nextAttackTime = Time.time + msBetweenAttacks / 1000;
            Projectile newProjectile = Instantiate(attackType, attackSpawn.position, attackSpawn.rotation);
            newProjectile.SetLifeTime(attackLifeTime);
            newProjectile.SetSpeed(attackSpeed);

            currentState = prevState;
            navMeshAgent.enabled = true;
        }
    }

    IEnumerator DamageFlash()
    {
        float duration = 0f;
        skinMaterial.color = flashColor;

        while(duration < flashDuration)
        {
            duration += Time.deltaTime;
            yield return null;
        }

        skinMaterial.color = originalColor;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // Enemy should flash when it takes damage
        StartCoroutine(DamageFlash());
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    public void Explode(ParticleSystem deathEffect, Vector3 hitPoint)
    {
        Destroy(Instantiate(deathEffect.gameObject, hitPoint, transform.rotation));
    }

    public void SpawnItems(List<Pack> items)
    {
        // pick a random item from the list of items and spawn it
        Pack itemToSpawn = deathItems[prng.Next(items.Count)];
        Instantiate(itemToSpawn, transform.position, transform.rotation);
    }

    public override void Die()
    {
        Explode(deathEffect, transform.position);
        SpawnItems(deathItems);
        base.Die();
    }
}
