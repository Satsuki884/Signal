using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PickUpTaskData", menuName = "Configs/PickUpTaskData")]
public class PickUpTaskData : ScriptableObject, ITaskData
{
    [SerializeField] private string taskId;
    public string TaskId => taskId;

    [SerializeField] private string description;
    public string Description => description;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private bool isCompleted;
    public bool IsCompleted
    {
        get => isCompleted;
        set => isCompleted = value;
    }

    [SerializeField] private int itemId;

}