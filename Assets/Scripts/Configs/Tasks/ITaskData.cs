using System.Collections.Generic;
using UnityEngine;

public interface ITaskData
{
    string TaskId { get; }
    string Description { get; }
    Sprite Icon { get; }
    bool IsCompleted { get; set; }
}