using Game.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIUtils))]
public class GenericTaskManager : MonoBehaviour, ITaskPrefab
{
    [SerializeField]
    protected Text _inputCode;

    [SerializeField]
    protected float resetTimeInSeconds = 0.5f;

    public string TaskName;

    private UIUtils UIUtils;

    public TaskWindow Parent { get; set; }

    public GenericTask CurrentGenericTask;


    public Button CompleteBtn => throw new System.NotImplementedException();

    private void Start()
    {
        UIUtils = GetComponent<UIUtils>();
    }

    public void SetTask(GenericTask currentTask)
    {
        CurrentGenericTask = currentTask;
        TaskName = CurrentGenericTask.GetTitle();
    }

    public void CheckGenericTaskAnswer(string asnwer) 
    {
        if (asnwer == CurrentGenericTask.CorrectAnswer)
        {
            _inputCode.text = "Correct";
            return;
        }
        
        _inputCode.text = "Failed";
        StartCoroutine(UIUtils.ResetCode(_inputCode,  resetTimeInSeconds));        
    }

    public bool IsReady()
    {
        throw new System.NotImplementedException();
    }

    public void CompleteTask()
    {
        throw new System.NotImplementedException();
    }

    public void SetParent(TaskWindow parent)
    {
        Parent = parent;
    }
}
