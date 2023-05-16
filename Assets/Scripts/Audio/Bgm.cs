using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgm : MonoBehaviour
{
    private AudioSource _audioSource;
    public static int _currentTime;
    [SerializeField] private bool _playing;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playing = true;
    }

    private void Start()
    {
        _audioSource.timeSamples = _currentTime;
    }

    private void Update()
    {
        if (_playing) _currentTime = _audioSource.timeSamples;

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_playing)
                StopSound();
            else
                PlaySound();
        }
    }

    private void PlaySound()
    {
        _audioSource.timeSamples = _currentTime;
        _audioSource.Play();
        _playing = true;
    }

    private void StopSound()
    {
        _currentTime = _audioSource.timeSamples;
        _audioSource.Stop();
        _playing = false;
    }
}
