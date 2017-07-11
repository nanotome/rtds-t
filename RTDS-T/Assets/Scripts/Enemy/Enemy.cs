using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent( typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    Transform target;
    GameObject targetEntity;
    bool hasTarget;
    public float rangeOfSight; // how far the Enemy can see
    public LayerMask viewMask; // layer on which obstacles to sight exist

    float collisionRadius;
    float targetCollisionRadius;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            // Get a reference to the Player
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.gameObject;

            // The Player and Enemy should collide on their surfaces and not their centres.
            collisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    private void Start()
    {
        if (hasTarget)
        {
            // Cast a ray from the Enemy to the Player
            StartCoroutine(UpdatePath());
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
                navMeshAgent.enabled = true;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPos = target.position - dirToTarget * (collisionRadius + targetCollisionRadius);

                navMeshAgent.SetDestination(targetPos);
            } else
            {
                navMeshAgent.enabled = false; // this stops the Enemy from chasing the Player
            }
            
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
