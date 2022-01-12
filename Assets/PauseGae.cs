using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGae : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Break();
        }
    }
}
