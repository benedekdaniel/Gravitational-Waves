﻿using UnityEngine;
using Game.Tasks;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using Mirror;

namespace Game.Managers
{
    public static class TaskManager {

        public static List<Task> Tasks = new List<Task>();
        public static UnityEvent OnTaskUpdate  = new UnityEvent();

        internal static void TriggeredTask(Task task) {
            if (PlayerManager.LocalPlayer.HasTaskOpen) return; // Player already has a task open. Avoiding opening tasks multiple times

            Debug.LogFormat("Task {0} triggered", task.Title);

            var playerTask = PlayerManager.LocalPlayer.AssignedTasks.FirstOrDefault(x => x.GetOrigin().Id == task.Id && !x.IsCompleted);

            if (playerTask != null) {
                GameObject taskUI = InstanceManager.DisplayFullscreen("Task UI Framework");
                TaskWindow taskWindow = taskUI.GetComponent<TaskWindow>();
                taskWindow.SetTask(playerTask);
                PlayerManager.LocalPlayer.HasTaskOpen = true;
            }
        }

        public static async System.Threading.Tasks.Task LoadTasksAsync() {

            Tasks = new List<Task>() {
                new StandardTask("MirrorCleaning", "Mirror Cleaning", "Clean those mirrors, fool!", "Mirror Cleaning Task", "Reward1"),
                new StandardTask("WaveformFitter", "Waveform Fitter", "Fit da Waveform", "Waveform Fitter Task", "Reward2"),
                new StandardTask("Range", "Range", "Get the waveform in the correct area?", "Range Task", "Reward3"),
            };

            var genericTasks = await GenericTaskReader.ReadTasksFromDiskAsync();
            Tasks = Tasks.Concat(genericTasks).ToList();

            
        }

        public static void AddTaskUpdateListener(UnityAction action) {
            OnTaskUpdate.AddListener(action);
        }

        public static void RemoveTaskUpdateListener(UnityAction action) {
            OnTaskUpdate.RemoveListener(action);
        }
        public static Task Task(string id) {
            return Tasks.FirstOrDefault(x => x.GetID().ToLower() == id.ToLower());
        }

        public static void TaskUpdated() {
            OnTaskUpdate?.Invoke();
            PlayerManager.NotifyPlayerUpdate();
            TeamManager.TeamUpdated();
        }

    }
}