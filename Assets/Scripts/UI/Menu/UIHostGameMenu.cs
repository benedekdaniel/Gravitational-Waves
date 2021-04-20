using Game.Managers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHostGameMenu : MonoBehaviour
{
    public void HostGame()
    {
        var newNetworkDiscovery = FindObjectOfType<NewNetworkDiscovery>();
        StartCoroutine(LoadGamViewScene());
        NetworkManager.singleton.StartHost();
        newNetworkDiscovery.AdvertiseServer();

    }

    IEnumerator LoadGamViewScene()
    {
        yield return SceneManager.LoadSceneAsync("GameView");
    }
}
