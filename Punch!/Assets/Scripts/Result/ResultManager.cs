using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
    [SerializeField]
    public ResultInfo _resultInfo;

    [SerializeField]
    public List<Sprite> winSprites;

    [SerializeField]
    public GameObject winImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetImage();
    }

    private void SetImage()
    {
        switch (_resultInfo.WinnerPlayerID)
        {
            case 0:
                winImage.GetComponent<UnityEngine.UI.Image>().sprite = winSprites[0];
                break;
            case 1:
                winImage.GetComponent<UnityEngine.UI.Image>().sprite = winSprites[1];
                break;
            case 2:
                winImage.GetComponent<UnityEngine.UI.Image>().sprite = winSprites[2];
                break;
            case 3:
                winImage.GetComponent<UnityEngine.UI.Image>().sprite = winSprites[3];
                break;
        }

        winImage.SetActive(true);
    }
}
