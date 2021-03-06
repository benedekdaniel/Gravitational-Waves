using Game.Managers;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHostGameMenu : MonoBehaviour
{

    public TMP_InputField GameCodeUIElement;

    private NewNetworkDiscovery newNetworkDiscovery;


    private void Awake()
    {
        newNetworkDiscovery = FindObjectOfType<NewNetworkDiscovery>();

        SetGameCodeOnUI(newNetworkDiscovery.GameCode);
    }

    

    public void HostGame()
    {
        SceneManager.LoadScene("GameView", LoadSceneMode.Single);
        NetworkManager.singleton.StartHost();
        newNetworkDiscovery.AdvertiseServer();
    }

    private void SetGameCodeOnUI(string gameCode)
    {
        if (GameCodeUIElement == null) 
            throw new MissingReferenceException("Cannot set game code on the UI. No Game Code UI Element provided.");

        GameCodeUIElement.text = gameCode;
    }
}
