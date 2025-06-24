using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeyBindButton : MonoBehaviour
{
    public InputActionReference actionRef; // The input action to rebind
    public TMP_Text label;                 // UI label showing the binding

    private int xboxBindingIndex = -1;

    private void Start()
    {
        xboxBindingIndex = FindXboxBindingIndex();

        if (xboxBindingIndex != -1)
        {
            LoadBinding();
        }
        else
        {
            Debug.LogWarning($"No Xbox binding found on action: {actionRef.action.name}");
        }
    }

    public void StartRebind()
    {
        if (xboxBindingIndex == -1)
        {
            Debug.LogWarning("Cannot rebind — Xbox binding not found.");
            return;
        }

        actionRef.action.Disable();
        label.text = "Press Xbox button...";

        actionRef.action.PerformInteractiveRebinding(xboxBindingIndex)
            .WithControlsHavingToMatchPath("<XInputController>")
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionRef.action.Enable();

                UpdateLabel();
                SaveBinding();
            })
            .Start();
    }

    private int FindXboxBindingIndex()
    {
        var bindings = actionRef.action.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];
            if (binding.path != null && binding.path.Contains("XInputController"))
            {
                return i;
            }
        }

        return -1;
    }

    private void UpdateLabel()
    {
        if (xboxBindingIndex == -1) return;

        string readable = InputControlPath.ToHumanReadableString(
            actionRef.action.bindings[xboxBindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        label.text = readable;
    }

    private void SaveBinding()
    {
        if (xboxBindingIndex == -1) return;

        string key = actionRef.action.name + "_Xbox";
        string path = actionRef.action.bindings[xboxBindingIndex].effectivePath;
        PlayerPrefs.SetString(key, path);
    }

    private void LoadBinding()
    {
        string key = actionRef.action.name + "_Xbox";
        string path = PlayerPrefs.GetString(key, "");

        if (!string.IsNullOrEmpty(path) && xboxBindingIndex != -1)
        {
            actionRef.action.ApplyBindingOverride(xboxBindingIndex, path);
        }

        UpdateLabel();
    }
}
