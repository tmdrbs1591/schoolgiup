using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private static PlayerInput _input = new ();

    public void Enable() {
        _input.Player.Movement.Enable();
        _input.Player.Jump.Enable();
        _input.Player.Attack.Enable();
        _input.Player.Interact.Enable();
        _input.Player.Switch.Enable();
    }

    public float Horizontal() {
        return _input.Player.Movement.ReadValue<float>();
    }

    public event Action<InputAction.CallbackContext> JumpPerformed {
        add {
            _input.Player.Jump.performed += value;
        }
        remove {
            _input.Player.Jump.performed -= value;
        }
    }
    
    public event Action<InputAction.CallbackContext> AttackPerformed {
        add {
            _input.Player.Attack.performed += value;
        }
        remove {
            _input.Player.Attack.performed -= value;
        }
    }
    
    public bool Attacking() {
        return _input.Player.Attack.ReadValue<bool>();
    }
    
    public event Action<InputAction.CallbackContext> InteractPerformed {
        add {
            _input.Player.Interact.performed += value;
        }
        remove {
            _input.Player.Interact.performed -= value;
        }
    }
    
    public event Action<InputAction.CallbackContext> SwitchPerformed {
        add {
            _input.Player.Switch.performed += value;
        }
        remove {
            _input.Player.Switch.performed -= value;
        }
    }

}
