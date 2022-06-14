using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootStorage : ScriptableObject
{
    private GameObject[] _GOStorage;
    private Rigidbody[] _RBStorage;
    private uint _size;
    private uint _lootCount = 0;
    private uint _lastElementNumber;

    public void SetLoot(GameObject Loot)
    {
        if (Loot.GetComponent<Loot>() == null) return;
        if (_lootCount >= _size) return;

        _lootCount++;
        _lastElementNumber = _lootCount - 1;

        _GOStorage[_lastElementNumber] = Loot;
        _RBStorage[_lastElementNumber] = Loot.GetComponent<Rigidbody>(); 

        _GOStorage[_lastElementNumber].GetComponent<Loot>().numberInStorage = _lastElementNumber;
        _GOStorage[_lastElementNumber].GetComponent<Loot>().isKeaped = true;        
    }
    
    public void CompressElements() // сдвигает элементы к началу, смещ€€ элементы на место null'ов
    {
        uint Searcher(uint iterator) 
        {
            iterator++;
            if (_GOStorage[iterator] != null)
            {
                return iterator;
            }
            else
            {
                return Searcher(iterator);
            }
        }

        for(uint i = 0; i < _lootCount; i++)
        {
            if(_GOStorage[i] == null)
            {
                uint s = Searcher(i);
                _GOStorage[i] = _GOStorage[s];
                _RBStorage[i] = _RBStorage[s];

                _GOStorage[s] = null;
                _RBStorage[s] = null;
            }

            if(i + 1 == _lootCount)
            {
                _lastElementNumber = i;
            }
        }
    }
    
    /*public void LoseLoot(uint numberInMassive)
    {
        _GOStorage[numberInMassive].GetComponent<Loot>().isKeaped = false;
        _GOStorage[numberInMassive].GetComponent<Loot>().hand = null;
        _GOStorage[numberInMassive] = null;
        _RBStorage[numberInMassive] = null;
        if (numberInMassive == _lootCount - 1) // если лут последний в массиве
        {
            _GOStorage[numberInMassive] = null;
            _RBStorage[numberInMassive] = null;
            _lootCount--;
        }
        else // если лут в середине массива или первый
        {
            for(uint i = numberInMassive; i < _lootCount; i++)
            {
                _GOStorage[i] = _GOStorage[i + 1];
                _RBStorage[i] = _RBStorage[i + 1];
            }
            _GOStorage[_lootCount - 1] = null;
            _RBStorage[_lootCount - 1] = null;  
            _lootCount--;
        }
        
    }*/

    public GameObject GetLootGO(uint numberInMassive)
    {
        return _GOStorage[numberInMassive];
    }

    public Rigidbody GetLootRB(uint numberInMassive)
    {
        return _RBStorage[numberInMassive];
    }

    public uint GetLootCount()
    {
        return _lootCount;
    }
    
    public uint GetLastElementNumber()
    {
        return _lastElementNumber;
    }

    public void LoseElement(uint numberInStorage) // использовать в св€зке с CompressElements()!!!
    {
        _GOStorage[numberInStorage] = null;
        _RBStorage[numberInStorage] = null;
        _lootCount--;
    }

    public LootStorage(uint size)
    {
        _GOStorage = new GameObject[size];
        _RBStorage = new Rigidbody[size];
        _size = size;
    }
}
