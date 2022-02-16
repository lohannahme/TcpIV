using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation levelGame = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while(levelGame.progress < 1)
        {
            yield return new WaitForSeconds(3); 
        }
    }
}
