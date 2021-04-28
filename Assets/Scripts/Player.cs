using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Sprite> shapes;
    private int currentShapeIndex;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ChangeShape();
        }
    }

    private void ChangeShape()
    {
        GetComponent<SpriteRenderer>().sprite = shapes[currentShapeIndex];
        currentShapeIndex++;
        if (currentShapeIndex >= shapes.Count)
        {
            currentShapeIndex = 0;
        }
    }
}
