using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;
    private float timer;

    void Start()
    {
        GameManager.instance.AddFood(this);    
    }

    void Update()
    {
        if (timer >= _lifeTime)
        {
            Destroy();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void Destroy()
    {
        GameManager.instance.RemoveFoodAtList(this);
        Destroy(gameObject);
    }
}
