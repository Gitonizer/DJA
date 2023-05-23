using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    //spawn enemies, count enemies, inform level is over
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private int _maxCurrentEnemies = 3;
    [SerializeField] private int _currentEnemies;
    [SerializeField] private int _totalEnemies;

    private float enemyHeight = 1.31f;

    public string NextLevel;
    public Enemy enemyPrefab;

    private void Awake()
    {
        _currentEnemies = 0;
        _totalEnemies = 0;
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawEnemy();
        }
    }

    private void Update()
    {
        if (_currentEnemies >= _maxCurrentEnemies)
            return;

        SpawEnemy();
    }

    private void SpawEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, transform);
        Vector3 randomPosition = new Vector3(Random.Range(-100, 100), enemyHeight, Random.Range(-100, 100));
        NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);
        enemy.transform.position = hit.position;
        enemy.Initialize(this);
        _currentEnemies++;
        _totalEnemies++;
    }

    public void HandleEnemyDeath()
    {
        _currentEnemies--;

        if (_totalEnemies - 1 >= _maxEnemies)
            SceneManager.LoadScene(NextLevel);
    }
}
