using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick() {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// #if y #end if son variables que maneja unity
    /// </summary>
    public void OnExitClick() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
