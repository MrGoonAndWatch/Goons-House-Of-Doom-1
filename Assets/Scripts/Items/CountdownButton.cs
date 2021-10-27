public class CountdownButton : Inspectable
{
    public TextReader TextReader;
    public CutsceneManager CutsceneManager;

    private const string TimerReducedText = "Self destruct sequence expedited.";
    private const string ButtonStuckText = "The button is stuck, I can't press it any more.";

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
        
        if (TriggerCount == 1)
        {
            CutsceneManager.PlaySelfDestructCutscene();
        }
        else
        {
            var countdownDisplay = FindObjectOfType<CountdownDisplay>();
            var result = countdownDisplay.StartCountdown();
            if (result)
            {
                TextReader.QueueReadText(new [] { TimerReducedText });
            }
            else
            {
                TextReader.QueueReadText(new[] { ButtonStuckText });
            }
        }
    }
}
