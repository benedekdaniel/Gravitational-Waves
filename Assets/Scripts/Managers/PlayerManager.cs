﻿using UnityEngine;
using System.Collections;
using System;
using Game.Players;
using UnityEngine.Events;

namespace Game.Managers {
    public static class PlayerManager {

        public static bool Ready { get; private set; }

        public static UnityEvent onPlayerUpdate { get; private set; }
        public static Player LocalPlayer { get; private set; }

        static PlayerManager() {
            Debug.Log("Loading PlayerManager");
            LocalPlayer = new Player("Test Player");
            Ready = true;
        }

        public static void Load() { }

        public static void AssignRandomTasks(Player player) {
            for (int i = 0; i < player.numberOfTasks; i++) {
                player.AssignTask(TaskManager.GetRandomTask().Clone());
            }
        }

        public static void AddPlayerUpdateListener(UnityAction action) {
            onPlayerUpdate.AddListener(action);
        }

        public static void RemovePlayerUpdateListener(UnityAction action) {
            onPlayerUpdate.RemoveListener(action);
        }
    }
}