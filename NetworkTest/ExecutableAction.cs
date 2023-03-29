using System;
using System.Collections.Generic;

public class ExecutableAction
{
    public bool IsExecuted { get; set; } = false;
    public Action Execute { get; set; }
}

public class ExecuteTasksInMainThread
{
    public ExecuteTasksInMainThread()
    {
        Tasks = new List<ExecutableAction>();
        TaskQueue = new List<ExecutableAction>();
    }

    private List<ExecutableAction> TaskQueue { get; set; }
    public List<ExecutableAction> Tasks { get; private set; }
    public bool CanExecuteTasks { get; private set; } = true;
    public bool IsTaskExecuting { get; set; } = false;

    public void AddTasksFromQueue()
    {
        CanExecuteTasks = false;
        Tasks.RemoveAll(item => item.IsExecuted);
        Tasks.AddRange(TaskQueue);
        CanExecuteTasks = true;
    }

    protected void AddTaskToQueue(ExecutableAction action)
    {
        TaskQueue.RemoveAll(item => item.IsExecuted);
        TaskQueue.Add(action);
    }
}
