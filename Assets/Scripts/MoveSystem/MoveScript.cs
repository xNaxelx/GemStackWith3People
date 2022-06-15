using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _decFactor;
    private float _finalSpeed;

    public IEnumerator Braking()
    {
        for (float q = 0f; q < _finalSpeed; q += .1f)
        {
            this.GetComponent<Player>().speed = q;
            yield return new WaitForSeconds(.03f);
        }
        this.GetComponent<Player>().speed = _finalSpeed;
    }

    void Update()
    {
        _finalSpeed = _baseSpeed - (hand.GetComponent<Hand>().lootStorage.GetLootCount() + 1) / _decFactor;
        this.GetComponent<Player>().speed = _finalSpeed;
    }
}
