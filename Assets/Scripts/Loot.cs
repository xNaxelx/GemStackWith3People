using Assets.Scripts;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] public float followSpeed = 5;
    [HideInInspector] public GameObject hand = null;
    [HideInInspector] public bool isKeaped = false;
    [SerializeField] public uint numberInStorage;
    [HideInInspector] public uint cost = 10;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Material[] _material;
    private uint _stage = 0;
    private float _forceX;
    private Vector3 _forceVector3 = new Vector3();
    private Vector3 _transformBuffer = new Vector3();
    

    public void Follow(GameObject previousGrabedElement)
    {
        _transformBuffer.x = transform.position.x;
        _transformBuffer.y = previousGrabedElement.transform.position.y; ;
        _transformBuffer.z = previousGrabedElement.transform.position.z + 1;
        transform.position = _transformBuffer;

        _forceX = previousGrabedElement.transform.position.x - transform.position.x;
        _forceVector3.x = _forceX * followSpeed;
        _rb.velocity = _forceVector3;
    }

    public void IncreaseStage(uint sourceStage)
    {
        if (_stage == sourceStage)
        {
            _stage++;
            gameObject.GetComponent<MeshRenderer>().material = _material[_stage];
            cost += 10;
            ScoreManager.GetInstance().IndicateScoreChange(transform.position,10);
        }
    }

    public void TossTrap()
    {
        _rb.AddForce(Random.Range(0.5f, 2f), Random.Range(1f, 2.5f), Random.Range(-0.5f, 0.5f), ForceMode.VelocityChange);
        isKeaped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.GetComponent<Loot>() != null) & isKeaped)
        {
            hand.GetComponent<Hand>().Grab(collision.gameObject);
        }
    }

}
