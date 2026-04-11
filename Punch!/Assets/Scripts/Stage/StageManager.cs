using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    [SerializeField]
    private StageBlock _blockPrefab;

    [SerializeField]
    private Vector2Int _stageScale;

    private List<StageBlock> _activeBlocks;

    private void Start()
    {
        _activeBlocks = new();
        CreateStage();
    }

    private void CreateStage()
    {
        for (int x = 0; x < _stageScale.x; ++x)
            for (int z = 0; z < _stageScale.y; ++z)
            {
                var pos = new Vector3(x, -1.3f, z);
                var block = Instantiate(_blockPrefab, pos, Quaternion.identity);
                _activeBlocks.Add(block);
            }
    }
}
