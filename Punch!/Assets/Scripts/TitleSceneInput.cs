using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

//TitleSceneでの入力を受付て、シーンの遷移をするクラス
//A class that accepts input in TitleScene and handles scene transitions.

public class TitleSceneInput : MonoBehaviour
{
    [SerializeField] private float delayTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    InputSystem.onAnyButtonPress.CallOnce(ctrl=>OnClick());
    //}

    public void OnClick()
    {
        SceneManager.LoadScene("TitleOverRayScene", LoadSceneMode.Additive);
        //PlauSceneに遷移する前に少し待つ
        StartCoroutine(DelayAndLoadScene());
    }

    private IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(delayTime);
        //シーン遷移
        //Scene transition
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }
}
