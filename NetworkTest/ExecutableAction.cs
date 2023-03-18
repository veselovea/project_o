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
    }

    public List<ExecutableAction> Tasks { get; private set; }
    public bool CanExecuteTasks { get; private set; } = true;
    public bool IsTaskExecuting { get; set; } = false;

    protected void AddNewTask(ExecutableAction action)
    {
        while (true)
        {
            if (IsTaskExecuting)
                continue;
            CanExecuteTasks = false;
            Tasks.RemoveAll(item => item.IsExecuted);
            Tasks.Add(action);
            CanExecuteTasks = true;
            break;
        }
    }
}
