using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float boundHeight;
    public float boundWidth;

    public Hunter hunter;
    public List<Boid> allBoids = new List<Boid>();
    public List<Food> allFood = new List<Food>();

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddBoid(Boid b)
    {
        if (allBoids.Contains(b)) return;

        allBoids.Add(b);
    }

    public void AddFood(Food f)
    {
        if (allFood.Contains(f)) return;
        else
            allFood.Add(f);
    }

    public int AllFoodCount()
    {
        return allFood.Count;
    }

    public void RemoveFoodAtList(Food f)
    {
        if (allFood.Contains(f)) allFood.Remove(f);
    }

    public Vector3 ApplyBounds(Vector3 objectPosition)
    {
        if (objectPosition.x > boundWidth / 2) objectPosition.x = -boundWidth / 2;
        if (objectPosition.x < -boundWidth / 2) objectPosition.x = boundWidth / 2;
        if (objectPosition.z < -boundHeight / 2) objectPosition.z = boundHeight / 2;
        if (objectPosition.z > boundHeight / 2) objectPosition.z = -boundHeight / 2;

        return objectPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 topLeft = new Vector3(-boundWidth / 2, 0, boundHeight / 2);
        Vector3 topRight = new Vector3(boundWidth / 2, 0, boundHeight / 2);
        Vector3 botRight = new Vector3(boundWidth / 2, 0, -boundHeight / 2);
        Vector3 botLeft = new Vector3(-boundWidth / 2, 0, -boundHeight / 2);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
        Gizmos.DrawLine(botRight, botLeft);
        Gizmos.DrawLine(botLeft, topLeft);
    }
}