using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private string taskId; // який таск виконує цей тригер
    [SerializeField] private bool destroyAfterTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // перевіряємо чи це поточна таска
        if (TaskManager.Instance.GetCurrentTaskId() != taskId) return;

        Debug.Log($"Trigger activated for task: {taskId}");

        TaskManager.Instance.CompleteTaskById(taskId);

        if (destroyAfterTrigger)
            Destroy(gameObject);
    }
}