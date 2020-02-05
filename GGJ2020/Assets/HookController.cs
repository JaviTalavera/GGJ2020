using UnityEngine;

public class HookController : MonoBehaviour
{
    private Transform _spawnPoint;
    private Level _level;
    private Robot _robot;
    public float _speed;

    public bool _started = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;
        _level = GameObject.FindWithTag("LevelManager").GetComponent<Level>();
    }

    public void Initialize()
    {
        this._started = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_started)
        transform.position += Vector3.right * _speed * Time.fixedDeltaTime;
    }

    public void SetRobot(Robot r)
    {
        _robot = r;
        r.transform.position = transform.position;
        r.gameObject.SetActive(true);
        var hook = GetComponentInChildren<DistanceJoint2D>();
        hook.connectedBody = r.GetHeadRigidBody();
        r.SetHook(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_robot)
        {
            if (!_robot.IsRepaired())
                _level.GetRobotsQueue().Enqueue(_robot);
            _robot.gameObject.SetActive(false);
            _robot = null;
        }
        transform.position = _spawnPoint.position;
        if (_level.GetRobotsQueue().Count > 0)
            SetRobot(_level.GetRobotsQueue().Dequeue());
    }
}
