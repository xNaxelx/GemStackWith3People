using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private LootStorage _ls;
    private uint victimNumberInStorage;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Loot>() != null)
        {
            _ls = collision.gameObject.GetComponent<Loot>().hand.GetComponent<Hand>().lootStorage;
            victimNumberInStorage = collision.gameObject.GetComponent<Loot>().numberInStorage;

            Destroy(_ls.GetLootGO(victimNumberInStorage));
            _ls.LoseElement(victimNumberInStorage);

            if(victimNumberInStorage != _ls.GetLastElementNumber())
            {
                for(uint v = victimNumberInStorage + 1, l = _ls.GetLastElementNumber(); v <= l; v++)
                {
                    _ls.GetLootGO(v).GetComponent<Loot>().TossTrap();
                    _ls.LoseElement(v);
                }                
            }
            _ls.CompressElements();
        }
    }
}
