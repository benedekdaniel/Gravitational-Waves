using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(InteractiveTask))]
public class GenericTaskTitleUpdater : MonoBehaviour
{
    public TMP_Text titleTextComponent;
    private InteractiveTask interactiveTaskComponent;

    void Start()
    {
        interactiveTaskComponent = GetComponent<InteractiveTask>();
        titleTextComponent ??= GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(interactiveTaskComponent.Task.Title))
        {
            titleTextComponent.text = interactiveTaskComponent.Task.Title;
        }
    }
}
