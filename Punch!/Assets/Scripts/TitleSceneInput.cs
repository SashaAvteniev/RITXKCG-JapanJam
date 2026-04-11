using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

//TitleSceneでの入力を受付て、シーンの遷移をするクラス
//A class that accepts input in TitleScene and handles scene transitions.

public class TitleSceneInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.onAnyButtonPress.CallOnce(ctrl=>OnClick());
    }

    private void OnClick()
    {
        //シーン遷移
        //Scene transition
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }
}
