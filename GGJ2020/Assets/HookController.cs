using UnityEngine;

public class HookController : MonoBehaviour
{
    private Transform _spawnPoint;
    private Level _level;
    private Robot _robot;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;
        _level = GameObject.FindWithTag("LevelManager").GetComponent<Level>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.right * 2 * Time.fixedDeltaTime;
    }

    public void SetRobot(Robot r)
    {
        _robot = r;
        r.transform.position = transform.position;
        r.gameObject.SetActive(true);
        var hook = GetComponentInChildren<DistanceJoint2D>();
        hook.connectedBody = r.GetHeadRigidBody();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_robot)
        {
            _level.GetRobotsQueue().Enqueue(_robot);
            _robot.gameObject.SetActive(false);
            _robot = null;
        }
        transform.position = _spawnPoint.position;
        SetRobot(_level.GetRobotsQueue().Dequeue());
    }
}
