using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Connection : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;

    void Start()
    {
        if (Application.isBatchMode)
        {
            _networkManager.StartClient();
        }
    }

    public void JoinClient()
    {
        _networkManager.networkAddress = "localhost";
        _networkManager.StartClient();
    }
}
