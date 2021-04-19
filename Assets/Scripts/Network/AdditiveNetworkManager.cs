using Game.Managers;
using Game.Tasks;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Additive
{
    [AddComponentMenu("")]
    public class AdditiveNetworkManager : NetworkManager
    {
        static readonly ILogger logger = LogFactory.GetLogger(typeof(AdditiveNetworkManager));
        private static System.Random random = new System.Random();

        [Tooltip("Trigger Zone Prefab")]
        public GameObject Zone;

        [Scene]
        [Tooltip("Add all sub-scenes to this list")]
        public string[] subScenes;

        public override void Start() {
            base.Start();

            StartCoroutine(LoadSubScenes());


            var isHost = MySceneManager.GetSceneArgument<bool>("GameView", "IsHost");
            if (isHost)
            {
                StartHosting();
                var spawnpoints = GameObject.FindGameObjectsWithTag("GenericTaskSpawnpoint").ToList();

            foreach (var task in TaskManager.Tasks)
            {
                if (task is GenericTask genericTask)
                {
                    var spawnpoint = spawnpoints[random.Next(spawnpoints.Count)];
                    spawnpoints.Remove(spawnpoint);

                    var genericTaskPrefab = AssetManager.Prefab("GenericTaskObjective");

                    var genericTaskGameObject = Instantiate(genericTaskPrefab, spawnpoint.transform.position, Quaternion.identity);
                    var genericTaskTitleObject = genericTaskGameObject.GetComponentInChildren<TMPro.TMP_Text>();

                    var interactiveTaskComponent = genericTaskGameObject.GetComponent<InteractiveTask>();
                    genericTaskTitleObject.text = genericTask.GetTitle();

                    interactiveTaskComponent.SetTask(genericTask);
                    NetworkServer.Spawn(genericTaskGameObject);
                }
            }


            // Assign tasks for host
            PlayerManager.AssignRandomTasks(PlayerManager.LocalPlayer);
            }

        }

        public void StartHosting()
        {
            if (!isNetworkActive)
            {
                StartHost();
            }

            // Instantiate Zone Handler on server only
            Instantiate(Zone);
        }

        IEnumerator LoadSubScenes()
        {
            foreach (string sceneName in subScenes)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }

        public override void OnStopServer()
        {
            StartCoroutine(UnloadScenes());
        }

        public override void OnStopClient()
        {
            StartCoroutine(UnloadScenes());
        }

        IEnumerator UnloadScenes()
        {
            logger.Log("Unloading Subscenes");

            foreach (string sceneName in subScenes)
                if (SceneManager.GetSceneByName(sceneName).IsValid() || SceneManager.GetSceneByPath(sceneName).IsValid())
                {
                    yield return SceneManager.UnloadSceneAsync(sceneName);
                    if (logger.LogEnabled()) logger.Log($"Unloaded {sceneName}");
                }

            yield return Resources.UnloadUnusedAssets();
        }

        private void OnConnectedToServer()
        {
            // Assign tasks for clients
            PlayerManager.AssignRandomTasks(PlayerManager.LocalPlayer);

        }
    }
}
