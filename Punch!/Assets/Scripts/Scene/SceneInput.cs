using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

//Sceneでの入力を受付て、シーンの遷移をするクラス
//A class that accepts input in Scene and handles scene transitions.

public class SceneInput : MonoBehaviour
{
    private enum direction
    {
        LEFT,
        UP,
    }

    //ホバー画像の動く方向
    [SerializeField]
    private direction hoverDirection;

    // ホバー画像の動く方向をベクトルで表したもの
    private Vector3 moveDirection;

    //次のシーンの名前
    [SerializeField]
    private string nextSceneName;

    //スタートボタンの動く画像
    //Moving image of the start button
    [SerializeField]
    private Image hoverImage;

    [SerializeField]
    private float fadeTime;

    [SerializeField] 
    private float transitionTime = 1.0f;

    //ホバー画像が現在地からどのくらい動くのか
    //How much the hover image moves from its current position
    [SerializeField]
    private float targetVal;

    //ホバー画像の目的座標
    private Vector3 targetPos;

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

        //ホバー画像の目的地を決める
        switch(hoverDirection)
        {
            case direction.LEFT:
                //動く方向を決める
                moveDirection = Vector3.left;
                //動く方向に動く距離をかける
                moveDirection.x *= targetVal;
                //目的座標の決定
                Debug.Log(moveDirection);
                targetPos = hoverImage.transform.position + moveDirection;
                break;
            case direction.UP:
                moveDirection = Vector3.up;
                moveDirection.y *= targetVal;
                Debug.Log(moveDirection);
                targetPos = hoverImage.transform.position + moveDirection;
                break;
        }

        Debug.Log(targetPos);
    }

    void FixedUpdate()
    {
        timer.Update();    
    }
    private void OnClick()
    {
        //クリックされた時にスタートボタンの動く画像を動かす
        //Move the moving image of the start button when clicked
        var pos = hoverImage.rectTransform.position;
        hoverImage.rectTransform.DOMove(targetPos, moveTime)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(TransitionScene);
    }

    private void TransitionScene()
    {
        //暗転の画像を出す
        //Display the darkening image
        timer.CreateTask(()=>fadePanel.SetActive(true),fadeTime);
        timer.CreateTask(()=>SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single),transitionTime);
    }
}
