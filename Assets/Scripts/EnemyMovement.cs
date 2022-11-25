using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// created using: https://www.youtube.com/watch?v=aHFSDcEQuzQ


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent Agent;

    // https://youtu.be/5uO0dXYbL-s?list=PLllNmP7eq6TSkwDN8OO0E8S6CWybSE_xC&t=443
    // private Coroutine TargetCoroutine;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Agent.destination = Target.position;
    }
}