using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextWrapper _text;
    [SerializeField]
    private ResultInfo _resultInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.Initialize();
        SetText();
    }

    private void SetText()
    {
        switch (_resultInfo.WinnerPlayerID)
        {
            case 0:
                _text.SetText("Player1Win!!");
                break;
            case 1:
                _text.SetText("Player2Win!!");
                break;
        }
    }
}
