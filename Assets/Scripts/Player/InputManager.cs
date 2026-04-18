using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager: MonoBehaviour
{
    public static InputManager Instance { get; private set;}
    public InputSystem_Actions actions;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        if (Instance != null) return;

        GameObject go = new GameObject("InputManager");
        Instance = go.AddComponent<InputManager>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        actions = new InputSystem_Actions();
        actions.Enable();
    }

    private void OnDestroy()
    {
        if(Instance == this){
            actions.Disable();
            actions = null;
            Instance = null;
        }
    }

    public void ChangeUnputMap(InputType type)
    {
        switch(type)
        {
            case InputType.Player:
                actions.UI.Disable();
                actions.Player.Enable();
                break;
            case InputType.UI:
                actions.Player.Disable();
                actions.UI.Enable();
                break;
        }
    }
}

public enum InputType
{
    Player, UI
}