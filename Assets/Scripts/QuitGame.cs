using System.Collections;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private float quitDelay;
    private Coroutine _quitRoutine;
    public void StartQuit()
    {
        if (_quitRoutine == null)
            _quitRoutine = StartCoroutine(Quit());
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(quitDelay);
        Application.Quit();
    }
}
