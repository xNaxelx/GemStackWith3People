using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public static MoveScript instance { get; private set; }
    [SerializeField] private GameObject hand;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _decFactor;
    private float _finalSpeed;
    private bool _cr_running;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public IEnumerator Braking()
    {
        _cr_running = true;
        for (float q = 0f; q < _finalSpeed; q += .01f)
        {
            this.GetComponent<Player>().speed = q;
            yield return new WaitForSeconds(.006f);
        }
        this.GetComponent<Player>().speed = _finalSpeed;
        _cr_running = false;
    }
    public IEnumerator Discarding()
    {
        _cr_running = true;
        for (float q = -2.5f; q < _finalSpeed; q += 0.14f)
        {
            this.GetComponent<Player>().speed = q;
            yield return new WaitForSeconds(0.045f);
        }
        this.GetComponent<Player>().speed = _finalSpeed;
        _cr_running = false;
    }

    public void Update()
    {
        _finalSpeed = _baseSpeed - (hand.GetComponent<Hand>().lootStorage.GetLootCount() + 1) / _decFactor;

    }

    public void ChangePlayerSpeed()
    {
        if (_cr_running == false)
        {
            this.GetComponent<Player>().speed = _finalSpeed;
        }
    }
}
