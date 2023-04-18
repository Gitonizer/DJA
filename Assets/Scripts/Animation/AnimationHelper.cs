using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationHelper
{
    public static IEnumerator AnimateButton(Animation animation, Action buttonCallBack)
    {
        animation.Play();

        yield return new WaitWhile(() => animation.isPlaying);

        buttonCallBack();
    }
}
