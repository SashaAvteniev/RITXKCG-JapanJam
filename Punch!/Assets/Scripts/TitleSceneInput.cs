using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

//TitleSceneでの入力を受付て、シーンの遷移をするクラス
//A class that accepts input in TitleScene and handles scene transitions.

public class TitleSceneInput : MonoBehaviour
{
    //スタートボタンの動く画像
    //Moving image of the start button
    [SerializeField]
    private Image HoverImage;

    [SerializeField] 
    private float delayTime = 1.0f;

    //スタートボタンの動く画像が動く位置のY座標
    //Y-coordinate of the position where the moving image of the start button moves
    [SerializeField]
    private float targetPosY;

    //スタートボタンの動く画像が動く時間
    //Time for the moving image of the start button to move
    [SerializeField]
    private float moveTime = 0.5f;

    //暗転用のパネル
    //Panel for darkening the screen
    [SerializeField]
    private GameObject fadePanel;

    //タイマークラスのインスタンス
    private Timer timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //タイマークラスのインスタンスを作成
        timer = new Timer();

        //暗転用パネルのSetActiveをfalseにする
        fadePanel.SetActive(false);

        InputSystem.onAnyButtonPress.CallOnce(ctrl=>OnClick());
    }

    void FixedUpdate()
    {
        timer.Update();    
    }
    private void OnClick()
    {
        //クリックされた時にスタートボタンの動く画像を動かす
        //Move the moving image of the start button when clicked
        var pos = HoverImage.rectTransform.position;
        HoverImage.rectTransform.DOMove(new Vector3(pos.x, targetPosY, pos.z), moveTime)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(TransitionScene);
    }

    private void TransitionScene()
    {
        //暗転の画像を出す
        //Display the darkening image
        fadePanel.SetActive(true);
        timer.CreateTask(()=>SceneManager.LoadScene("PlayScene",LoadSceneMode.Single),delayTime);
    }
}
