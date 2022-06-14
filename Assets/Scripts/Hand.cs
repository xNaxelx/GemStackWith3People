using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isFinished = false;
    public LootStorage lootStorage;
    private Input_Controls _input;
    private Camera _camera;
    private Vector3 _screenMousePosition = new Vector3();
    private Vector3 _InGameMousePosition = new Vector3();

    private void MoveHand()
    {
        _screenMousePosition = _input.Action_Map.TapPosition.ReadValue<Vector2>();
        _screenMousePosition.z = 7; //расстояние от камеры до руки, пока хардкод, но нужно исправить
        _InGameMousePosition = _camera.ScreenToWorldPoint(_screenMousePosition);

        _InGameMousePosition.y = gameObject.transform.position.y;
        _InGameMousePosition.z = gameObject.transform.position.z;

        gameObject.transform.position = _InGameMousePosition;
    }

    public void Grab(GameObject Loot)
    {
        for(uint i = 0; i < lootStorage.GetLootCount(); i++)
        {
            if(Loot == lootStorage.GetLootGO(i))
            {
                return;
            }
        }
        lootStorage.SetLoot(Loot);

        Loot.GetComponent<Loot>().hand = gameObject;
        ScoreManager.GetInstance().IndicateScoreChange(Loot.transform.position,(int)Loot.GetComponent<Loot>().cost,true);
    }

    private void MoveStone()
    {
        for(uint i = 0; i < lootStorage.GetLootCount(); i++)
        {
            if(i == 0)
            {
                lootStorage.GetLootGO(i).GetComponent<Loot>().Follow(gameObject);
            }
            else
            {
                lootStorage.GetLootGO(i).GetComponent<Loot>().Follow(lootStorage.GetLootGO(i - 1));
            }
        }
    }

    private void Awake()
    {
        _input = new Input_Controls();
        _camera = Camera.main;
        lootStorage = new LootStorage(100);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Loot>() != null)
        {
            Grab(collision.gameObject);
        }
    }

    void FixedUpdate()
    {
        if(isFinished == false)
        {
            MoveHand();
        }
        else
        {
            _InGameMousePosition.x = 0;
            _InGameMousePosition.z = gameObject.transform.position.z; // если этой строки не будет, рука зависает в одном месте
            gameObject.transform.position = _InGameMousePosition;
        }
        MoveStone();
    }
}
