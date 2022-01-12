using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    [SerializeField] private float loadWait;
    private AudioSource _audioSource;
    private Coroutine _restartLevel;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _restartLevel == null)
            _restartLevel = StartCoroutine(RestartLevel());   
    }

    private IEnumerator RestartLevel()
    {
        _audioSource.Play();
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
