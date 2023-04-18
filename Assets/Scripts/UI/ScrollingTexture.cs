using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingTexture : MonoBehaviour
{
    private RawImage _image;

    public float SpeedX;
    public float SpeedY;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    private void Start()
    {
        StartCoroutine(ChangeScroll());
    }

    private void Update()
    {
        _image.uvRect = new Rect(_image.uvRect.position + new Vector2(SpeedX, SpeedY) * Time.deltaTime, _image.uvRect.size);
    }

    private IEnumerator ChangeScroll()
    {
        yield return new WaitForSeconds(4);

        SpeedX = Random.Range(-3, 3) / 10f;
        SpeedY = Random.Range(-3, 3) / 10f;

        StartCoroutine(ChangeScroll());
    }
}
