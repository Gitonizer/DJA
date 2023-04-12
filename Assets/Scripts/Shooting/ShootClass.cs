using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootClass : MonoBehaviour
{
    public Image CrossHair;
    public Bullet Bullet;
    public GameObject CameraHolder;
    public LayerMask LayerMask;

    private void Update()
    {
        CrossHair.color = Color.black;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, Mathf.Infinity, LayerMask))
        {
            CrossHair.color = Color.red;
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.black);
    }

    public IEnumerator ShootBullet(Action SetCooldownOver)
    {
        Bullet bullet = Instantiate(Bullet, transform.position + transform.forward + transform.up, Quaternion.identity);
        Vector3 direction = Quaternion.AngleAxis(-20f, CameraHolder.transform.forward) * Quaternion.AngleAxis(-5f, CameraHolder.transform.up) * CameraHolder.transform.forward;
        bullet.Initialize(direction * 40f);

        yield return new WaitForSeconds(0.5f);

        SetCooldownOver();
    }
}
