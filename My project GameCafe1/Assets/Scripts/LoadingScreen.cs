using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public string sceneToLoad = "Main";
    private int dotCount = 0;
    private bool isLoading = false;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        isLoading = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {

            dotCount = (dotCount + 1) % 4; 
            loadingText.text = "Loading" + new string('.', dotCount);

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
}