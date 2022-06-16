using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isFinished = false;
    public LootStorage lootStorage;
    public float moveLimitX;
    private Input_Controls _input;
    private Camera _camera;
    private Vector3 _screenMousePosition = new Vector3();
    private Vector3 _InGameMousePosition = new Vector3();
    private float _onMousePress ;

    private void MoveHand()
    {
        _screenMousePosition = _input.Action_Map.TapPosition.ReadValue<Vector2>();
        _screenMousePosition.z = 7; //ðàññòîÿíèå îò êàìåðû äî ðóêè, ïîêà õàðäêîä, íî íóæíî èñïðàâèòü(íåò +_-)
        _InGameMousePosition = _camera.ScreenToWorldPoint(_screenMousePosition);

        _InGameMousePosition.y = gameObject.transform.position.y;
        _InGameMousePosition.z = gameObject.transform.position.z;

        _onMousePress = _input.Action_Map.Tap.ReadValue<float>();

        if (_onMousePress > 0)
        {
            if (_InGameMousePosition.x < moveLimitX && _InGameMousePosition.x > -moveLimitX)
            {
                gameObject.transform.position = _InGameMousePosition;
            }
            else if (_InGameMousePosition.x < -moveLimitX)
            {
                gameObject.transform.position = new Vector3(-moveLimitX, gameObject.transform.position.y,
                    gameObject.transform.position.z);
            }
            else if (_InGameMousePosition.x > moveLimitX)
            {
                gameObject.transform.position = new Vector3(moveLimitX, gameObject.transform.position.y,
                    gameObject.transform.position.z);
            }
        }
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
        MoveScript.instance.ChangePlayerSpeed();
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
            _InGameMousePosition.z = gameObject.transform.position.z; // åñëè ýòîé ñòðîêè íå áóäåò, ðóêà çàâèñàåò â îäíîì ìåñòå
            gameObject.transform.position = _InGameMousePosition;
        }
        MoveStone();
    }
}
