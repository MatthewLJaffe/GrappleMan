using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuitMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuParent;
    [SerializeField] private float delayTime;
    private Coroutine _menuCloseRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuParent.SetActive(!menuParent.activeInHierarchy);
        }
    }

    public void StartDelayedMenuClose()
    {
        if (_menuCloseRoutine == null)
            _menuCloseRoutine = StartCoroutine(DelayedMenuClose());
    }

    private IEnumerator DelayedMenuClose()
    {
        yield return new WaitForSeconds(delayTime);
        menuParent.SetActive(false);
        _menuCloseRoutine = null;
    }
}
