using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [SerializeField] private TaskPool taskPool;

    private List<TaskData> tasks;
    private int currentTaskIndex = -1;

    private TaskData CurrentTask =>
        (currentTaskIndex >= 0 && currentTaskIndex < tasks.Count)
        ? tasks[currentTaskIndex]
        : null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitTasks();
        StartFirstTask();
    }

    private void InitTasks()
    {
        tasks = new List<TaskData>(taskPool.GetTaskDatas());
    }

    private void StartFirstTask()
    {
        if (tasks.Count == 0)
        {
            Debug.LogWarning("No tasks in pool!");
            return;
        }

        currentTaskIndex = 0;
        Debug.Log($"Start task: {CurrentTask.Description}");

        // якщо є UI
        TaskUI.Instance?.ShowTask(CurrentTask);
    }

    public void CompleteCurrentTask()
    {
        if (CurrentTask == null) return;

        Debug.Log($"Completed: {CurrentTask.Description}");

        currentTaskIndex++;

        // якщо це була остання таска
        if (currentTaskIndex >= tasks.Count)
        {
            Debug.Log("🎉 ГРА ЗАКІНЧЕНА");
            return;
        }

        // наступна таска
        Debug.Log($"Next task: {CurrentTask.Description}");
        TaskUI.Instance?.ShowTask(CurrentTask);
    }

    public string GetCurrentTaskId()
    {
        return CurrentTask?.TaskId;
    }
}