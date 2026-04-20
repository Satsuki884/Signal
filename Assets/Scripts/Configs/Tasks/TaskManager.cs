using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private TaskPool taskPool;

    private List<TaskRuntime> tasks = new List<TaskRuntime>();
    private int currentTaskIndex = -1;

    private TaskRuntime CurrentTask =>
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

    // ---------------- INIT ----------------

    private void InitTasks()
    {
        tasks.Clear();

        foreach (var taskData in taskPool.GetTaskDatas())
        {
            tasks.Add(new TaskRuntime(taskData));
        }
    }

    private void StartFirstTask()
    {
        if (tasks.Count == 0)
        {
            Debug.LogWarning("No tasks in pool!");
            return;
        }

        currentTaskIndex = 0;

        SkipCompletedTasks();
    }

    // ---------------- COMPLETE ----------------

    public void CompleteCurrentTask()
    {
        if (CurrentTask == null) return;

        Debug.Log($"Completed: {CurrentTask.Data.Description}");

        CurrentTask.Complete();

        currentTaskIndex++;

        SkipCompletedTasks();
    }

    public void CompleteTaskById(string taskId)
    {
        if (tasks.Count == 0) return;

        var task = tasks.Find(t => t.Data.TaskId == taskId);

        if (task == null)
        {
            Debug.LogWarning($"Task with id {taskId} not found");
            return;
        }

        if (task.IsCompleted) return;

        task.Complete();

        int index = tasks.IndexOf(task);

        if (index == currentTaskIndex)
        {
            currentTaskIndex++;
            SkipCompletedTasks();
        }
    }

    // ---------------- FLOW ----------------

    private void SkipCompletedTasks()
    {
        while (currentTaskIndex < tasks.Count &&
               tasks[currentTaskIndex].IsCompleted)
        {
            currentTaskIndex++;
        }

        if (currentTaskIndex >= tasks.Count)
        {
            Debug.Log("🎉 ГРА ЗАКІНЧЕНА");
            return;
        }

        Debug.Log($"Current task: {CurrentTask.Data.Description}");

        TaskUI.Instance?.ShowTask(CurrentTask.Data);
        PausePanelController.Instance?.ShowTask(CurrentTask.Data);
    }

    // ---------------- GETTERS ----------------

    public string GetCurrentTaskId()
    {
        return CurrentTask?.Data.TaskId;
    }

    public TaskData GetCurrentTask()
    {
        return CurrentTask?.Data;
    }

    public bool IsTaskCompleted(string taskId)
    {
        var task = tasks.Find(t => t.Data.TaskId == taskId);
        return task != null && task.IsCompleted;
    }
}