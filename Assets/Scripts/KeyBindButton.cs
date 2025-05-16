using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyBindButton : MonoBehaviour
{
    public InputActionReference actionRef; // The input action to rebind
    public int[] bindingIndices;           // Supports multiple bindings (e.g., keyboard + gamepad)
    public TMP_Text[] labels;              // Text fields for each binding button

    private void Start()
    {
        LoadAllBindings();
    }

    // Called by each UI button, passing the index you want to rebind
    public void StartRebind(int index)
    {
        if (index < 0 || index >= bindingIndices.Length)
        {
            Debug.LogError("Invalid binding index for rebind.");
            return;
        }

        // Disable the action first
        actionRef.action.Disable();

        labels[index].text = "Press a key...";

        actionRef.action.PerformInteractiveRebinding(bindingIndices[index])
            .WithControlsExcluding("Mouse") // Optional
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionRef.action.Enable();

                UpdateLabel(index);
                SaveBinding(index);
            })
            .Start();
    }

    void UpdateLabel(int index)
    {
        string displayString = actionRef.action.GetBindingDisplayString(bindingIndices[index]);
        labels[index].text = displayString;
    }

    void SaveBinding(int index)
    {
        string path = actionRef.action.bindings[bindingIndices[index]].effectivePath;
        PlayerPrefs.SetString(actionRef.action.name + "_binding_" + index, path);
    }

    void LoadBinding(int index)
    {
        string key = actionRef.action.name + "_binding_" + index;
        string path = PlayerPrefs.GetString(key, "");
        if (!string.IsNullOrEmpty(path))
        {
            actionRef.action.ApplyBindingOverride(bindingIndices[index], path);
        }

        UpdateLabel(index);
    }

    void LoadAllBindings()
    {
        for (int i = 0; i < bindingIndices.Length; i++)
        {
            LoadBinding(i);
        }
    }
}
