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

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _healthBar = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        CurrentHealth = MAX_HEALTH;
    }

    public void Damage()
    {
        _healthBar.value = CurrentHealth -= 1f;
        _audioSource.Play();

        if (CurrentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
