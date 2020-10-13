using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using GameUI;

public class UIManager :
	MonoBehaviour,
	IGameManager
{
    public ManagerStatus Status { get; private set; }
    public Canvas Canvas;
    [SerializeField]
    [RequiredField]
    private Button _confirmingButton;
    private List<ConfirmingTask> _confirmingTasks;
    [RequiredField]
    public Transform AssociatedElements;
    [RequiredField]
    public Transform Inventory;
    [RequiredField]
    public Camera Camera;
    [RequiredField]
    public PopupText PopupTextPrefab;
    [RequiredField]
    public PopupIcon PopupIconPrefab;

    public void Startup()
    {
        Status = ManagerStatus.Initializing;
        Debug.Log("UI manager is starting...");
        _confirmingTasks = new List<ConfirmingTask>();
        Status = ManagerStatus.Started;
        Debug.Log("UI manager is started.");
	}
    public void Confirm(string message, Action func)
    {
        _confirmingTasks.Add(new ConfirmingTask{ Action = func, Message = message });
        if (_confirmingTasks.Count == 1)
        {
            ShowConfirming();
        }
    }
    public void CancelConfirming(Action func)
    {
        var index = _confirmingTasks.FindIndex(x => x.Action == func);
        if (index >= 0)
        {
            _confirmingTasks.RemoveAt(index);
        }
        if (index == 0)
        {
            ChangeConfirming();
        }
    }
    private void ShowConfirming()
    {
        var confirmTask = _confirmingTasks[0];
        _confirmingButton.onClick.AddListener(OnConfirmed);
        _confirmingButton.GetComponentInChildren<Text>().text = confirmTask.Message;
        _confirmingButton.gameObject.SetActive(true);
    }
    private void ChangeConfirming()
    {
        _confirmingButton.onClick.RemoveListener(OnConfirmed);
        if (_confirmingTasks.Count == 0)
        {
            _confirmingButton.gameObject.SetActive(false);
        } else
        {
            ShowConfirming();
        }
    }
    private void OnConfirmed()
    {
        var confirmTask = _confirmingTasks[0];
        _confirmingTasks.RemoveAt(0);
        ChangeConfirming();
        confirmTask.Action();
    }
    public void ShowPopupIcon(Sprite sprite, string text)
    {
        PopupIcon.CreateInstance(AssociatedElements.gameObject, sprite, text);
    }
}

public class ConfirmingTask
{
    public string Message;
    public Action Action;
}