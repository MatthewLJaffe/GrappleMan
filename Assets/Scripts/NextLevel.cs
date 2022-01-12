using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    private AudioSource _audioSource;
    private Coroutine _loadingLevel;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartNextLevel()
    {
        if (_loadingLevel == null)
            _loadingLevel = StartCoroutine(LoadNextLevel());
    }

    public void StartSceneLoad(int sceneIdx)
    {
        if (_loadingLevel == null)
            _loadingLevel = StartCoroutine(LoadSceneIndex(sceneIdx));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _loadingLevel == null)
            _loadingLevel = StartCoroutine(LoadNextLevel());
    }
    private IEnumerator LoadNextLevel()
    {
        _audioSource.Play();
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private IEnumerator LoadSceneIndex(int sceneIndex)
    {
        _audioSource.Play();
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        SceneManager.LoadScene(sceneIndex);
    }
}
