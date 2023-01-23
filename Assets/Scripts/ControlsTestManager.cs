using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsTestManager : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown("m")) {
            SceneManager.LoadScene("Start-Menu");
        }
    }
}
