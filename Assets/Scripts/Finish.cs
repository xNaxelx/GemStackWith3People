using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private uint gettedValuse = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Loot>() != null)
        {
            collision.gameObject.GetComponent<Loot>().hand.GetComponent<Hand>().isFinished = true;
        }
        else if(collision.gameObject.GetComponent<Hand>() != null)
        {
            collision.gameObject.GetComponent<Hand>().isFinished = true;
            //сюда вставить после решения бага, отредактированный под подсчёт денег код ловушки
        }

        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().IsFinished = true;
            Debug.Log("Your getted valuse: " + gettedValuse);
            ScoreManager.GetInstance().ChangePlayerBalance((int)gettedValuse);
        }
    }
}
