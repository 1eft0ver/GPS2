using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour {

    public void uiToAR()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("AR");
    }

    // 從 AR Scene 跳到 SetLabel Scene
    public void arToSetLabel()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("SetLabel");
    }

    // 從 SetLabel Scene 跳到 AR Scene
    public void setLabelToAR()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("AR");
    }

    // 從 SetLabel Scene 跳到 Map Scene
    public void setLabelToMap()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("GoogleMap");
    }

    // 從 SetLabel Scene 跳到 Map Scene
    public void mapToSetLabel()
    {
        Debug.Log("Click Button");
        SceneManager.LoadScene("SetLabel");
    }
}
