using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    private AudioSource clip;

    private void Awake()
    {
        clip = GetComponent<AudioSource>();
    }
    public void OnAnimate()
    {
        clip.Play();
    }
}
