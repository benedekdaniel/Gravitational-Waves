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

        [Tooltip("Trigger Zone Prefab")]
        public GameObject Zone;

        [Scene]
        [Tooltip("Add all sub-scenes to this list")]
        public string[] subScenes;

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (MySceneManager.GetSceneArgument<bool>("GameView", "IsHost"))
            {
                var genericTaskPrefab = AssetManager.Prefab("GenericTaskObjective");

                var genericTaskGameObject = Instantiate(genericTaskPrefab, new Vector3(-18.22f, 0.5f, -31.53f), Quaternion.identity);

                var interactiveTask = genericTaskGameObject.GetComponent<InteractiveTask>();

                interactiveTask.SetTask(TaskManager.Tasks.FirstOrDefault(gt => gt is GenericTask) as GenericTask);
                NetworkServer.Spawn(genericTaskGameObject);

                // Assign tasks for host
                PlayerManager.AssignRandomTasks(PlayerManager.LocalPlayer);
            }

        }

        public override void Start() {

            StartCoroutine(LoadSubScenes());


            var isHost = MySceneManager.GetSceneArgument<bool>("GameView", "IsHost");
            if (isHost)
            {
                StartHosting();
            }

            base.Start();
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
            logger.Log("Loading Scenes");

            foreach (string sceneName in subScenes)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                if (logger.LogEnabled()) logger.Log($"Loaded {sceneName}");
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
