using Game.Managers;
using Game.Tasks;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(InteractionListener))]
public class InteractiveTask : NetworkBehaviour {

    [SyncVar]
    public string TaskName;

    public Task Task;

    public InteractionListener InteractionListener { get; private set; }

    public void SetTask(Task task)
    {
        Task = task;
        TaskName = task.GetTitle();
    }
    private void Start() {
        InteractionListener = gameObject.GetComponent<InteractionListener>();
        InteractionListener.OnInteraction.AddListener(Trigger);

        if (!string.IsNullOrEmpty(TaskName))
            Task = TaskManager.Task(TaskName);
    }

    private void OnDestroy() {
        InteractionListener.OnInteraction.RemoveListener(Trigger);
    }

    public void Trigger() {
        TaskManager.TriggeredTask(Task);
    }
}
