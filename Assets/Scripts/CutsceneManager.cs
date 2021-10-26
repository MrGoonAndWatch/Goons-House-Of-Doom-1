using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GameObject Vignette;
    
    void Start()
    {
        if (PlayerStatus == null)
            PlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    public void PlaySelfDestructCutscene()
    {
        StartCutscene();

        // TODO: DO CUTSCENE.

        EndCutscene();
    }

    private void StartCutscene()
    {
        PlayerStatus.LockMovement = true;
        Vignette.SetActive(true);
    }

    private void EndCutscene()
    {
        PlayerStatus.LockMovement = false;
        Vignette.SetActive(false);
    }
}
