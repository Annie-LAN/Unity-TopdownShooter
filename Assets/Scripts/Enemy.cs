using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    NavMeshAgent pathFinder;
    Transform target;

    // override the base class Start method
    protected override void Start()
    {
        // call the base class(LivingEntity)'s Start method
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
    }
    
    void Update()
    {
        
    }

    // put 'SetDestination' this way so that it's updating 4 time per second, compared to updating every frame(could be 60 times per second) if putting it in Update method. This improves game performance.
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        while(target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            if (!dead)
            {
                pathFinder.SetDestination(targetPosition);
            }            
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
