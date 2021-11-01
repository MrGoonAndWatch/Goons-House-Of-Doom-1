using System.Linq;

public class GiveItemOnInspect : Inspectable
{
    public Item ItemToGive;
    public string[] AddedItemText;
    public string InventoryFullText = "I can't fit anything else in my inventory.";

    private PlayerInventory _playerInventory;
    private TextReader _textReader;

    void Start()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _textReader = FindObjectOfType<TextReader>();
    }

    public override bool CanInspect()
    {
        return TriggerCount == 0;
    }

    public override void OnUsed()
    {
        if (TriggerCount > 0)
            return;

        var remainingQty = _playerInventory.AddItem(ItemToGive);
        if (remainingQty == 0)
        {
            TriggerCount++;

            if (EventToTrigger != GlobalEvent.None)
            {
                var playerStatus = FindObjectOfType<PlayerStatus>();
                playerStatus.TriggeredEvent(EventToTrigger);
            }

            if(AddedItemText.Any())
                _textReader.QueueReadText(AddedItemText);
        }
        else
            _textReader.QueueReadText(new[] { InventoryFullText });
        
    }
}
