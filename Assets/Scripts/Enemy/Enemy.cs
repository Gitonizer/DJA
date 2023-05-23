using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public float CurrentHealth;

    private AudioSource _audioSource;
    private const float MAX_HEALTH = 10f;
    private Slider _healthBar;
    private Camera _camera;
    private EnemyManager _enemyManager;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _healthBar = GetComponentInChildren<Slider>();
        _camera = Camera.main;
    }

    void Start()
    {
        _healthBar.minValue = 0f;
        _healthBar.maxValue = CurrentHealth = MAX_HEALTH;
    }

    private void Update()
    {
        _healthBar.transform.LookAt(_camera.transform);
    }

    public void Initialize(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void Damage()
    {
        _healthBar.value = CurrentHealth -= 1f;
        _audioSource.Play();

        if (CurrentHealth <= 0f)
        {
            _enemyManager.HandleEnemyDeath();
            Destroy(gameObject);
        }
    }
}
