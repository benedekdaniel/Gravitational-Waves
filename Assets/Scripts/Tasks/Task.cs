using Game.Managers;
using Game.Players;
using System;
using UnityEngine;

namespace Game.Tasks
{
    [Serializable]
    public class Task : ITask {
        public bool IsInProgress { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public string Id { get; protected set; }
        public string Title;
        public string Description;
        public string Prefab;
        public string Reward;
        public Task Parent;
        public Player Owner;

        public Task(string id, string title, string description, string prefab, string reward) {
            Id = id;
            Title = title;
            Description = description;
            Prefab = prefab;
            Reward = reward;
        }

        public Task()
        {

        }

        public void Complete(bool value = true) {
            IsCompleted = value;
            if (IsCompleted) {
                Debug.LogFormat("Task {0} Completed!", Title);
                IsInProgress = false;
                ScoreManager.AddScore(Owner, Reward);
                TaskManager.TaskUpdated();
            } else {
                Debug.LogFormat("Task {0} reset", Title);
            }
        }

        internal void ToggleCompleted() {
            Complete(!IsCompleted);
        }

        internal void Started(bool value = true) {
            if (value) {
                Debug.LogFormat("Task {0} started!", Title);
            } else {
                Debug.LogFormat("Task {0} no longer started", Title);
            }
            IsInProgress = value;
        }

        public string GetID() => Id;

        public string GetDescription() => Description;

        public string GetTitle() => Title;

        public void SetOwner(Player player) {
            Owner = player;
        }

        public Task GetOrigin() {
            return Parent == null ? this : Parent.GetOrigin();
        }
    }
}
