using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public GameObject Player;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _playerCurrentPosition;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerCurrentPosition = new Vector3();
    }

    private void Update()
    {
        if (_playerCurrentPosition == Player.transform.position)
            return;

        _navMeshAgent.SetDestination(Player.transform.position);
        _playerCurrentPosition = Player.transform.position;
    }
}
