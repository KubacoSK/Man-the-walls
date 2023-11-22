using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public static List<MoveToMouse> moveableObjects = new List<MoveToMouse>();
    public float speed = 5f;
    private Vector3 target;
    private bool selected;

    void Start()
    {
        moveableObjects.Add(this);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        SelectObject();
    }

    void SelectObject()
    {
        selected = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        foreach (MoveToMouse obj in moveableObjects)
        {
            if (obj != this)
            {
                obj.Deselect();
            }
        }
    }

    void Deselect()
    {
        selected = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}