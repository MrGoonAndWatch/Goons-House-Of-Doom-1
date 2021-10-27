using UnityEngine;

public class Inspectable : MonoBehaviour
{
    public GlobalEvent EventToTrigger;

    public string[] InspectText;
    public string[] Choices;

    protected int TriggerCount;

    public virtual void OnUsed()
    {
        TriggerCount++;

        if (TriggerCount > 1)
            return;

        if (EventToTrigger != GlobalEvent.None)
        {
            var playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.TriggeredEvent(EventToTrigger);
        }
    }

    public virtual void SetTriggered()
    {
        TriggerCount = 1;
    }
}
