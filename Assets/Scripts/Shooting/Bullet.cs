using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public LayerMask LayerMask;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Initialize(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Impulse);

        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(collision.gameObject.layer)) == LayerMask)
        {
            if (collision.gameObject.GetComponent<ParticleSystem>())
                collision.gameObject.GetComponent<ParticleSystem>().Play();

            if (collision.gameObject.GetComponent<IDamageable>() != null)
                collision.gameObject.GetComponent<IDamageable>().Damage();
        }
    }
}
