using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Game.Managers.Controllers;

public class Minimap : MonoBehaviour
{
    public PlayerController localPlayerObject;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (localPlayerObject == null)
            localPlayerObject = FindObjectOfType<PlayerController>();
        if (localPlayerObject == null) return;

        Vector3 newPosition = localPlayerObject.gameObject.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, localPlayerObject.gameObject.transform.eulerAngles.y, 0F);
    }
}
