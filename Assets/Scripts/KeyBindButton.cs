using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyBindButton : MonoBehaviour
{
    public InputActionReference actionRef; // e.g. Jump or Move action
    public int bindingIndex; 
    public TMP_Text label;

    private void Start()
    {
        UpdateLabel();
    }

    public void StartRebind()
    {
        // Disable the action first
        actionRef.action.Disable();

        label.text = "Press a key...";

        actionRef.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // can be optional
            .OnComplete(operation =>
            {
                operation.Dispose();

                // Re-enable after rebind or else it will get error
                actionRef.action.Enable();

                UpdateLabel();
                SaveBinding();
            })
            .Start();
    }

    void UpdateLabel()
    {
        var displayString = actionRef.action.GetBindingDisplayString(bindingIndex);
        label.text = displayString;
    }

    void SaveBinding()
    {
        var path = actionRef.action.bindings[bindingIndex].effectivePath;
        PlayerPrefs.SetString(actionRef.action.name + bindingIndex, path);
    }

    public void LoadBinding()
    {
        string path = PlayerPrefs.GetString(actionRef.action.name + bindingIndex, "");
        if (!string.IsNullOrEmpty(path))
        {
            actionRef.action.ApplyBindingOverride(bindingIndex, path);
            UpdateLabel();
        }
    }
}
