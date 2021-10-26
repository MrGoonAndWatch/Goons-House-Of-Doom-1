public class CountdownButton : Inspectable
{
    public TextReader TextReader;
    public CutsceneManager CutsceneManager;

    private const string ButtonStuckText = "The button is stuck, I can't press it any more.";

    private const int CountdownBaseTime = 10;

    void Awake()
    {
        if (TextReader == null)
            TextReader = FindObjectOfType<TextReader>();
        if (CutsceneManager == null)
            CutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    public override void OnUsed()
    {
        base.OnUsed();

        if (TriggerCount == 0)
            return;

        var countdownTime = CountdownBaseTime / TriggerCount;
        if (countdownTime == 0)
        {
            TextReader.ReadText(new []{ButtonStuckText});
        }
        else
        {
            CutsceneManager.PlaySelfDestructCutscene();
        }
    }
}
