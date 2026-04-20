using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public static TaskUI Instance { get; private set; }

    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Image taskIconImage;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTask(TaskData task)
    {
        if (task == null) return;

        taskText.text = $"* {task.Description}";
        taskIconImage.sprite = task.Icon;
    }
}