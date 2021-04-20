using Mirror;
using Mirror.Examples.Additive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Network/NewNetworkDiscoveryHUD")]
[RequireComponent(typeof(NewNetworkDiscovery))]
[RequireComponent(typeof(AdditiveNetworkManager))]
public class NewNetworkDiscoveryHUD : MonoBehaviour
{
    Vector2 scrollViewPos = Vector2.zero;

    public NewNetworkDiscovery newNetworkDiscovery;
    public NetworkManager networkManager;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (newNetworkDiscovery == null)
        {
            newNetworkDiscovery = GetComponent<NewNetworkDiscovery>();
            networkManager = GetComponent<AdditiveNetworkManager>();

            UnityEditor.Events.UnityEventTools.AddPersistentListener(newNetworkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, newNetworkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    void OnGUI()
    {
        if (NetworkManager.singleton == null)
            return;

        if (NetworkServer.active || NetworkClient.active)
            return;

        if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
            DrawGUI();
    }

    void DrawGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Find Servers"))
        {
            ServerListManager.DiscoveredServers.Clear();
            newNetworkDiscovery.StartDiscovery();
        }

        GUILayout.EndHorizontal();

        // show list of found server

        GUILayout.Label($"Discovered Servers [{ServerListManager.DiscoveredServers.Count}]:");

        // servers
        scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);

        foreach (NetworkResponseMessage info in ServerListManager.DiscoveredServers.Values)
            if (GUILayout.Button(info.gameCode))
                Connect(info);

        GUILayout.EndScrollView();
    }

    void Connect(NetworkResponseMessage info)
    {
        // NetworkManager.singleton.StartClient(info.uri);
        networkManager.StartClient(info.uri);
    }

    public void OnDiscoveredServer(NetworkResponseMessage info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        ServerListManager.AddServer(info);
    }
}

