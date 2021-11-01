public class SpawnBoulderOnConfirm : Inspectable
{
    public Boulder Boulder;

    public override void OnUsed()
    {
        base.OnUsed();

        if(TriggerCount == 1)
            Boulder.Activate();
    }
}
