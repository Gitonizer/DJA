using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float CurrentHealth;
    private AudioSource _audioSource;

    private const float MAX_HEALTH = 10f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        CurrentHealth = MAX_HEALTH;
    }

    public void Damage()
    {
        CurrentHealth -= 1f;
        _audioSource.Play();

        if (CurrentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
