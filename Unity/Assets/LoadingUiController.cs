using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUiController : MonoBehaviour
{
    public float PauseTime = 2;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(PauseTime);
        SceneManager.LoadScene(UnityContract.SceneStartMenu, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
