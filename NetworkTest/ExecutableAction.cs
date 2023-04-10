using System;
using System.Collections.Generic;

public class ExecutableAction
{
    public bool IsExecuted { get; set; } = false;
    public Action Execute { get; set; }
}

public class ExecuteTasksInMainThread
{
    private List<ExecutableAction> _tasksQueue;

    public ExecuteTasksInMainThread()
    {
        Tasks = new List<ExecutableAction>();
        _tasksQueue = new List<ExecutableAction>();
    }

    public List<ExecutableAction> Tasks { get; private set; }
    public bool CanExecuteTasks { get; private set; } = true;
    public bool IsTaskExecuting { get; set; } = false;

    public void ExecuteTasks()
    {
        if (Tasks.Count == 0
            || !CanExecuteTasks)
            return;
        IsTaskExecuting = true;
        foreach (var item in Tasks)
        {
            if (item is null)
                continue;
            if (!item.IsExecuted)
                item.Execute();
            item.IsExecuted = true;
        }
        IsTaskExecuting = false;
        AddTasksFromQueue();
    }

    public void AddTasksFromQueue()
    {
        CanExecuteTasks = false;
        Tasks.RemoveAll(item =>
        {
            if (item is null)
                return false;
            return item.IsExecuted;
        });
        Tasks.AddRange(_tasksQueue);
        CanExecuteTasks = true;
    }

    protected void AddTaskToQueue(ExecutableAction action)
    {
        _tasksQueue.RemoveAll(item => item.IsExecuted);
        _tasksQueue.Add(action);
        if (Tasks.Count == 0)
            Tasks.AddRange(_tasksQueue);
    }
}
