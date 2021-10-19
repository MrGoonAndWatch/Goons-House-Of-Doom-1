using UnityEngine;

public class Shambler : Enemy
{
    public float Speed = 10.0f;
    [Tooltip("How frequently (in seconds) the enemy should look for a new position to move towards.")]
    public float TargetUpdateFrequency = 1.0f;
    public float MaxWanderDistance = 10.0f;
    public float AttackAtDistance = 2.0f;
    public float AttackDuration = 1.5f;
    public GameObject AttackHitbox;
    [Tooltip("How close to consider the enemy to be 'at' its targeted position (will stop moving).")]
    public float AtTargetThreshold = 5.0f;

    private Vector3 _origin;
    private Vector3 _targetPosition;

    private float _timeSinceTargetUpdate;
    private float _timeSinceAttackStart;

    private bool _chasingPlayer;
    private bool _attacking;

    private float _minWanderX;
    private float _maxWanderX;
    private float _minWanderZ;
    private float _maxWanderZ;

    protected override void Start()
    {
        base.Start();

        _origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        _minWanderX = _origin.x - MaxWanderDistance;
        _maxWanderX = _origin.x + MaxWanderDistance;
        _minWanderZ = _origin.z - MaxWanderDistance;
        _maxWanderZ = _origin.z + MaxWanderDistance;

        _targetPosition = transform.position;

        AttackHitbox.SetActive(false);
    }

    void Update()
    {
        _timeSinceTargetUpdate += Time.deltaTime;
        if (_attacking)
            _timeSinceAttackStart += Time.deltaTime;

        if (_timeSinceTargetUpdate > TargetUpdateFrequency)
            UpdateTarget();

        if (!_attacking && _chasingPlayer && DistanceToPlayer() <= AttackAtDistance)
            Attack();

        if (_attacking && _timeSinceAttackStart > AttackDuration)
            StopAttack();

        if (!_attacking)
            Move();
    }

    void UpdateTarget()
    {
        if (CanSeePlayer())
        {
            _targetPosition = new Vector3(PlayerStatus.transform.position.x, transform.position.y, PlayerStatus.transform.position.z);
            _chasingPlayer = true;
        }
        else if(IsAtDestination())
        {
            var targetX = Random.Range(_minWanderX, _maxWanderX);
            var targetZ = Random.Range(_minWanderZ, _maxWanderZ);
            _targetPosition = new Vector3(targetX, transform.position.y, targetZ);
            _chasingPlayer = false;
        }
        else
        {
            _chasingPlayer = false;
        }
        transform.LookAt(_targetPosition, Vector3.up);
        _timeSinceTargetUpdate = 0;
    }

    void Attack()
    {
        AttackHitbox.SetActive(true);
        _attacking = true;
        _timeSinceAttackStart = 0;
    }

    void StopAttack()
    {
        AttackHitbox.SetActive(false);
        _attacking = false;
    }

    void Move()
    {
        if(!IsAtDestination())
            transform.localPosition += transform.forward * Speed * Time.deltaTime;
    }

    bool IsAtDestination()
    {
        return Vector3.Distance(transform.position, _targetPosition) <= AtTargetThreshold;
    }
}
