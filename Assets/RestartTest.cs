using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTest : MonoBehaviour
{
    [SerializeField] private int _timeToWait = 10;

    void Start(){
        StartCoroutine(restartCoroutine());
    }

    IEnumerator restartCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        yield return new WaitForSeconds(_timeToWait);

        //yield on a new YieldInstruction that waits for 5 seconds.
        LadderLevelManager.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
