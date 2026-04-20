using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TaskPool", menuName = "Configs/TaskPool")]
public class TaskPool : ScriptableObject
{
    [SerializeField] private List<TaskData> taskDatas = new List<TaskData>();
    public List<TaskData> GetTaskDatas() => taskDatas;
}