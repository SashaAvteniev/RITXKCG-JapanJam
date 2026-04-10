using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 現在誰に入力情報を渡すか
/// </summary>
public enum InputHandler
{
    Player,
    UI
}

public sealed class InputSystemManager : SingletonMonoBehaviour<InputSystemManager>
{
    [SerializeField] 
    private InputActionAsset _asset;

    private InputActionMap _playerMap;
    private InputActionMap _uiMap;

    // 登録済みアクション
    private readonly Dictionary<string, InputAction> _subscribed = new();
    private Dictionary<InputAction, string> _eventDic = new();

    // ブロック条件（Fade中は止める等）
    private Func<bool> _isBlocked;

    public void Initialize()
    {
        _playerMap = _asset.FindActionMap("Player", throwIfNotFound: true);
        _uiMap = _asset.FindActionMap("UI", throwIfNotFound: true);

        SubscribeAllActions(_asset);

        // 初期はPlayerだけ有効、みたいな運用もOK
        _playerMap.Enable();
        _uiMap.Disable();
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
    }

    private void SubscribeAllActions(InputActionAsset asset)
    {
        UnsubscribeAll();
        if (asset == null) 
            return;

        foreach (var map in asset.actionMaps)
            foreach (var action in map.actions)
            {
                action.started += OnActionEvent;
                action.performed += OnActionEvent;
                action.canceled += OnActionEvent;
                var key = $"{map.name}/{action.name}";
                _subscribed.Add(key, action);
            }
    }

    private void UnsubscribeAll()
    {
        foreach (var map in _subscribed)
        {
            var a = map.Value;
            if (a == null)
                continue;

            a.started -= OnActionEvent;
            a.performed -= OnActionEvent;
            a.canceled -= OnActionEvent;
        }

        _subscribed.Clear();
        _eventDic.Clear();
    }

    private void OnActionEvent(InputAction.CallbackContext ctx)
    {
        if (_isBlocked != null && _isBlocked()) 
            return;

        var signal = InputSignal.From(ctx);

        if (!_eventDic.TryGetValue(ctx.action, out var eventName))
            return;
        EventDispatcher.Instance.Dispatch(eventName, signal);
    }

    public void BindActionEvent(string actionName, InputHandler handler, string eventName)
    {
        actionName = $"{GetMapNameFromHandler()}/{actionName}";

        if (!_subscribed.ContainsKey(actionName))
            return;

        _eventDic[_subscribed[actionName]] = eventName;

        string GetMapNameFromHandler()
        {
            return handler switch
            {
                InputHandler.Player => "Player",
                InputHandler.UI => "UI",
                _ => null
            };
        }

    }

    public void SetBlockTerm(Func<bool> blockTerm)
        => _isBlocked = blockTerm;

    public void ClearBlockTerm()
        => _isBlocked = null;

    // ハンドラー指定でアクティブ化
    public void SetHandler(InputHandler h)
    {
        switch (h)
        {
            case InputHandler.Player:
                _uiMap.Disable();
                _playerMap.Enable();
                break;
            case InputHandler.UI:
                _playerMap.Disable();
                _uiMap.Enable();
                break;
        }
    }
}

// -------------------- domain event --------------------

public readonly struct InputSignal
{
    public readonly int ActionId;
    public readonly string Map;
    public readonly string Action;
    public readonly InputActionPhase Phase;
    public readonly double Time;
    public readonly object Value;
    

    private InputSignal(int actionId, string map, string action, InputActionPhase phase, double time, object value)
    {
        ActionId = actionId;
        Map = map;
        Action = action;
        Phase = phase;
        Time = time;
        Value = value;
    }

    public static InputSignal From(InputAction.CallbackContext ctx)
    {
        string map = ctx.action.actionMap.name;
        string action = ctx.action.name;
        
        // 文字列をハッシュ化する
        int id = Animator.StringToHash($"{map}/{action}");

        object value = ReadValueBoxed(ctx);

        return new InputSignal(
            actionId: id,
            map: map,
            action: action,
            phase: ctx.phase,
            time: ctx.time,
            value: value
        );
    }

    private static object ReadValueBoxed(InputAction.CallbackContext ctx)
    {
        // よく使う型だけ分岐（必要なら追加）
        return ctx.action.expectedControlType switch
        {
            "Vector2" => ctx.ReadValue<Vector2>(),
            "Vector3" => ctx.ReadValue<Vector3>(),
            "Axis" => ctx.ReadValue<float>(),
            "Button" => ctx.ReadValue<float>(),
            _ => null
        };
    }
}