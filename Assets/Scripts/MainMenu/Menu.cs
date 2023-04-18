using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GoToScene(Animation animation)
    {
        StartCoroutine(AnimationHelper.AnimateButton(animation, () => SceneManager.LoadScene("Level1", LoadSceneMode.Single)));
    }

    public void ExitGame(Animation animation)
    {
        StartCoroutine(AnimationHelper.AnimateButton(animation, Application.Quit));
    }
}
