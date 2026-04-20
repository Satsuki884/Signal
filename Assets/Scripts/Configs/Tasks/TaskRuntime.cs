public class TaskRuntime
{
    public TaskData Data { get; private set; }
    public bool IsCompleted { get; private set; }

    public TaskRuntime(TaskData data)
    {
        Data = data;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}