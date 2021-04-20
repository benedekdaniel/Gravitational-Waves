using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
using Game.Tasks;
using UnityEngine.UI;
using System.Linq;

public class UIPlayerTaskList : MonoBehaviour {
    private GameObject taskListRowPrefab;

    private ScrollRect taskListScrollView;
    private GameObject taskListContainer;
    

    private List<UITaskRow> displayedTasks;

    private void OnEnable() {
        taskListRowPrefab = AssetManager.Prefab(SettingsManager.taskListRowPrefab);
        taskListScrollView = gameObject.GetComponentInChildren<ScrollRect>();
        taskListContainer = taskListScrollView.content.gameObject;
        displayedTasks = new List<UITaskRow>();
        PlayerManager.OnPlayerUpdate.AddListener(DisplayPlayerTasks);
    }

    private void DisplayPlayerTasks() {
        foreach(Task task in PlayerManager.LocalPlayer.AssignedTasks) {
            if(!displayedTasks.Any(x => x.Task == task)) {
                GameObject isntance = Instantiate(taskListRowPrefab, taskListContainer.transform);
                UITaskRow taskRow = isntance.GetComponent<UITaskRow>();
                taskRow.AssignTask(task);
                displayedTasks.Add(taskRow);
            }
        }
    }
}
