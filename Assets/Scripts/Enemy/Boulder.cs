using UnityEngine;

public class Boulder : MonoBehaviour
{
    public Vector3 Destination;
    public float Speed = 10.0f;

    private bool _active;
    private Vector3 _direction;

    private PlayerStatus _playerStatus;

    public void Activate()
    {
        gameObject.SetActive(true);

        var audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();

        _direction = (Destination - gameObject.transform.position).normalized;
        _active = true;

        _playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        if (!_active || _playerStatus.Paused)
            return;

        gameObject.transform.position += _direction * Speed * Time.deltaTime;
        if (Vector3.Distance(gameObject.transform.position, Destination) < 1)
            Deactivate();
    }

    private void Deactivate()
    {
        _active = false;
        var audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
