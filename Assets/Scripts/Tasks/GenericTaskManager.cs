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

    private UIUtils UIUtils;

    public TaskWindow Parent { get; set; }

    public GenericTask CurrentGenericTask = null;


    public Button CompleteBtn => throw new System.NotImplementedException();

    private void Start()
    {
        UIUtils = GetComponent<UIUtils>();
    }

    public void SetTask(GenericTask currentTask)
    {
        CurrentGenericTask = currentTask;
    }

    public void CheckGenericTaskAnswer(string asnwer) 
    {
        if (asnwer == CurrentGenericTask.CorrectAnswer)
        {
            _inputCode.text = "Correct";
            CompleteTask();
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
        Parent.CompleteTask();
    }

    public void SetParent(TaskWindow parent)
    {
        Parent = parent;
    }
}
