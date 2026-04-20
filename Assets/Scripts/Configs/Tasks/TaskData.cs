using UnityEngine;

public enum TaskType
{
    Collect,
    Reach,
    Kill
}


[CreateAssetMenu(fileName = "TaskData", menuName = "Configs/TaskData")]
public class TaskData : ScriptableObject
{
    [SerializeField, ReadOnly] private string taskId;
    public string TaskId => taskId;

    [SerializeField] private string description;
    public string Description => description;

    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

#if UNITY_EDITOR
    private void OnValidate()
    {
        string assetName = name;

        if (taskId != assetName)
        {
            taskId = assetName;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}