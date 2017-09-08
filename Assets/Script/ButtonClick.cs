using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour {

    public void OnBtn()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("AR");
    }
}
