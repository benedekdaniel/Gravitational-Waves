using Game.Managers;
using Game.Managers.Controllers;
using Game.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionMethod
{
    Mouse,
    Key
}

[RequireComponent(typeof(SphereCollider))]
public class InteractionListener : MonoBehaviour, IInteractible {

    [Tooltip("Toggle whether this Game Object can be interacted with.")]
    public bool IsInteractionAllowed;

    public float triggerRadius = 1f;

    [Tooltip("Whether the interaction can be triggered by mouse click.")]
    public bool CanTriggerByMouse = true;

    [Tooltip("Whether the interaction can be triggered by key press.")]
    public bool CanTriggerByKeys = true;

    [Tooltip("The keys by which the interaction can be triggered.")]
    public List<KeyCode> TriggerKeys = new List<KeyCode> { KeyCode.F };

    public UnityEvent OnInteraction = new UnityEvent(); 

    private PlayerController localPlayerController;
    private SphereCollider triggerSphere;

    private void Start()
    {
        localPlayerController = FindObjectOfType<PlayerController>();
        triggerSphere = GetComponent<SphereCollider>();
        triggerSphere.radius = triggerRadius;
        triggerSphere.isTrigger = true;
    }

    public void Interact() {
        OnInteraction.Invoke();
    }

    public void Update() {

        if (localPlayerController == null)
        {
            localPlayerController = FindObjectOfType<PlayerController>();
        }
        if (localPlayerController == null) return;
        
        if (IsInteractableBy(InteractionMethod.Key))
        {
            foreach (var keyCode in GetTriggerKeys())
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Interact();
                    break;
                }
            }
        }
    }

    public void OnMouseDown()
    {
        if (localPlayerController == null) return;

        if (IsInteractableBy(InteractionMethod.Mouse))
        {
            if (Vector3.Distance(transform.position, localPlayerController.gameObject.transform.position) <= triggerRadius)
            {
                Interact();
            }
        }
    }

    /// <summary>
    /// Returns the list of the keys by which the interaction can be triggered with.
    /// </summary>
    /// <returns>The list of keys.</returns>
    public List<KeyCode> GetTriggerKeys()
    {
        return TriggerKeys;
    }

    /// <summary>
    /// Determines whether the object is interactable by the specified method.
    /// </summary>
    /// <param name="interactionMethod">The specified interaction method.</param>
    /// <returns>True - if the object can be interacted with by the specified method | False - otherwise.</returns>
    public bool IsInteractableBy(InteractionMethod interactionMethod)
    {
        if (!IsInteractionAllowed) return false;

        return interactionMethod switch
        {
            InteractionMethod.Mouse => CanTriggerByMouse,
            InteractionMethod.Key => CanTriggerByKeys,
            _ => false,
        };
    }

    /// <summary>
    /// When another object with a collider enters the trigger.
    /// </summary>
    /// <param name="other">The other object's collider compontent.</param>
    private void OnTriggerEnter(Collider other)
    {
        IsInteractionAllowed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsInteractionAllowed = false;
    }
}
