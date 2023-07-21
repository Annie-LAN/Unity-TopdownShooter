using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State {Idle, Chasing, Attacking};
    State currentState;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    // override the base class Start method
    protected override void Start()
    {
        // call the base class(LivingEntity)'s Start method
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }


    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                // not use 'Distance' method cause it's expensive
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }        
    }

    IEnumerator Attack()
    {
        // disable the pathfinding so that it doesn't interfere with the attack animation
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);        

        float attackSpeed = 3;
        // set this for the attack animation
        float percent = 0;

        skinMaterial.color = Color.red;

        while(percent < 1)
        {
            percent += Time.deltaTime * attackSpeed;
            // make the enemy go to the attackPosition, then back to original Position
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            // skip a frame between each step in the while loop
            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled=true;
    }

    // put 'SetDestination' this way so that it's updating 4 time per second, compared to updating every frame(could be 60 times per second) if putting it in Update method. This improves game performance.
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        while(hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }            
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
