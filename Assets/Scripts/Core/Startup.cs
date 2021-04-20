using UnityEngine;
using Game.Managers;
using System.Threading.Tasks;

namespace Game.Core
{

    //[InitializeOnLoad]
    public static class Startup {
        [RuntimeInitializeOnLoadMethod]
        static async Task OnRuntimeMethodLoad() {
            Debug.Log("Starting Manager Loading");
            await TaskManager.LoadTasksAsync();

            SettingsManager.Load();
            AssetManager.Load();
            InstanceManager.Load();
            
            PlayerManager.Load();

            UIManager.Load();

            MySceneManager.Load();
            CameraManager.Load();
            InputManager.Load();

            ActionManager.Load();
            

            TeamManager.Load();

            Debug.Log("Finished Manager Loading");
        }
    }
}