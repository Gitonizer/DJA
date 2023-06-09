using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    [Header("Animation Sounds")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
    }

    // Animation Events (for the prefab that came with the starter pack)
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_rigidBody.centerOfMass), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_rigidBody.centerOfMass), FootstepAudioVolume);
        }
    }
}
