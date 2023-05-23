using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateTest : MonoBehaviour
{
    private Transform Player;
    private float rotationSpeed;
    private NavMeshAgent _agent;

    public GameObject[] Waypoints;

    public int CurrentWaypointIndex;

    private PatrolState _patrolState;

    private void Awake()
    {
        rotationSpeed = 60f;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        CurrentWaypointIndex = 0;
        _patrolState = PatrolState.Patrol;
    }

    private void Start()
    {
        _agent.SetDestination(Waypoints[CurrentWaypointIndex].transform.position);
    }
    private void Update()
    {
        WaypointPatrolling();
        RotateToNextWaypoint();
    }

    private void WaypointPatrolling()
    {
        if (_patrolState == PatrolState.Rotate)
            return;

        if (Vector3.Distance(transform.position, Waypoints[CurrentWaypointIndex].transform.position) >= 0.95f)
            return;

        CurrentWaypointIndex = CurrentWaypointIndex + 1 < Waypoints.Length ? CurrentWaypointIndex + 1 : 0;

        _patrolState = PatrolState.Rotate;
    }

    private void RotateToNextWaypoint()
    {
        if (_patrolState == PatrolState.Patrol)
            return;

        var TargetQuaternion = Quaternion.LookRotation(Waypoints[CurrentWaypointIndex].transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetQuaternion, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, TargetQuaternion) <= 6f)
        {
            _agent.SetDestination(Waypoints[CurrentWaypointIndex].transform.position);

            _patrolState = PatrolState.Patrol;
        }
    }
}

public enum PatrolState
{
    Patrol,
    Rotate
}
