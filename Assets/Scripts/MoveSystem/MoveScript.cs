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

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        Destroy(this.gameObject);//нужно переписать скрипт и обращения к нему полностью
    }

    public IEnumerator Braking()
    {
        for (float q = 0f; q < _finalSpeed; q += .01f)
        {
            this.GetComponent<Player>().speed = q;
            yield return new WaitForSeconds(.006f);
        }
        this.GetComponent<Player>().speed = _finalSpeed;
    }
    public IEnumerator Discarding()
    {
        for (float q = -2.5f; q < _finalSpeed; q += 0.14f)
        {
            this.GetComponent<Player>().speed = q;
            yield return new WaitForSeconds(0.045f);
        }
        this.GetComponent<Player>().speed = _finalSpeed;
    }

    void Update()
    {
        _finalSpeed = _baseSpeed - (hand.GetComponent<Hand>().lootStorage.GetLootCount() + 1) / _decFactor;
   //     this.GetComponent<Player>().speed = _finalSpeed;
    }
}
