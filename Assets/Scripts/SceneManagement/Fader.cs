using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
  public class Fader : MonoBehaviour
  {
    CanvasGroup canvasGroup;

    private void Start()
    {
      canvasGroup = GetComponent<CanvasGroup>();

    }

    public void FadeOutImmediate() {
      canvasGroup.alpha = 1;
    }
    
    public IEnumerator FadeOut(float time)
    {
      while (canvasGroup.alpha < 1f)
      {
        canvasGroup.alpha += Time.deltaTime / time;

        yield return null;
      }

      canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeIn(float time)
    {
      while (canvasGroup.alpha > 0f)
      {

        canvasGroup.alpha -= Time.deltaTime / time;

        yield return null;
      }

      canvasGroup.alpha = 0f;
    }
  }
}
