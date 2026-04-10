using UnityEngine;

/// <summary>
/// 外部ファイルのディレクトリをまとめたクラス
/// </summary>
public class SummarizeResourceDirectory
{
    // TextureDirectory : Addressables


    // UsingFont

    // 使うフォントが決まったらここにAddressablesのパスを突っ込む
    public const string FONT = "Assets/Font/MainFont.asset";

    // JsonFileDirectory

    public const string STAGEDATA_PATH_TEMPLATE = "/ParameterControll/JsonFiles/";

    // ScriptableObjectDirectory

    public const string STAGEDATA_ASSET_PATH = "Assets/ExternalResources/Stages/StageDatas.asset";

    ///CRIPath///

    public const string CRI_ACFFILE_PATH = "Assets/StreamingAssets/CRIAssets/TebasakiPanic.acf";
    public const string CRI_ACBFILE_PATH_TEMPLATE = "CRIAssets/";
}
