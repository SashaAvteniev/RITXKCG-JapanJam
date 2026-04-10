using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// The class catch the input event
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class ControllerInputListener : MonoBehaviour
{
    // On value changed
    public Action<Vector2> OnLStickInputCallBack
    { get; set; }
    // Called by GetkeyDown
    public Action OnPunchInputPressedCallBack
    { get; set; }

    /// <summary>
    /// You need a this function when Use this lisnter
    /// </summary>
    public void Initialzie()
    {
        var playerInput = this.GetComponent<PlayerInput>();
        bool isConnected = playerInput.devices.Any(d => d.added);

        if (!isConnected)
            ConnenctDevice();
    }

    private void ConnenctDevice()
    {
        var playerInput = this.GetComponent<PlayerInput>();

        var devices = Gamepad.all;

        foreach (var device in devices)
        {
            if (!PlayerInput.all.Any(p => p.devices.Contains(device)))
            {
                InputUser.PerformPairingWithDevice(device, playerInput.user);
            }
        }
    }

    public void OnPunch(InputValue value)
    {
        OnPunchInputPressedCallBack?.Invoke();
    }

    public void OnMove(InputValue value)
    {
        // MoveAction‚Ì“ü—Í’l‚ğæ“¾
        var axis = value.Get<Vector2>();

        // ˆÚ“®‘¬“x‚ğ•Û
        OnLStickInputCallBack?.Invoke(axis);
    }
}