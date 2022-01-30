using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region VARIABLES

    public Image fader;

    private static SceneController instance;


    #endregion

    #region UNITY_CALLS

    private void Awake()
    {
        OnInstance();
    }

    #endregion

    #region FUNCTIONS

    public static void LoadScene(string sceneName, float duration, float waitTime)
    {
        instance.StartCoroutine(instance.FaderScene(sceneName, duration, waitTime));
    }
    private IEnumerator FaderScene(string sceneName, float duration = 1, float waitTime = 0)
    {
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);


        yield return new WaitForSeconds(waitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);

    }
    private void OnInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
            fader.gameObject.SetActive(false);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}