using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PatrolState : StateMachineBehaviour
{

    float timer;
    public float PatrolTime = 10f;

    //reference to player and the Navmesh
    Transform player;
    NavMeshAgent agent;

    //detection area and the patrol speed
    public float detectionRadius = 10f;
    public float patrolSpeed = 2f;

    //make waypoints to patrol 
    List<Transform> waypoints = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpeed;
        timer = 0;

        //moving through waypoints
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypoints.Add(t);
        }
        Vector3 nextPos = waypoints[Random.Range(0, waypoints.Count)].position;
        agent.SetDestination(nextPos);  
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        //check if enemy has reached the waypoint
        if(agent.remainingDistance <= agent.stoppingDistance) 
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        }

        //back to idle state
        timer += Time.deltaTime;
        if (timer > PatrolTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        //chase player
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        agent.SetDestination(agent.transform.position);
    }

}
