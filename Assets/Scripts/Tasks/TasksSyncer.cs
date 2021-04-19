using Game.Managers;
using Game.Tasks;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksSyncer : NetworkBehaviour
{
    public SyncList<Task> Tasks = new SyncList<Task>();

    private void OnConnectedToServer()
    {
        TaskManager.Tasks = new List<Task>(Tasks);
        PlayerManager.AssignRandomTasks(PlayerManager.LocalPlayer);
    }
}
