using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 target;
    private bool selected;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        gameManager.moveableObjects.Add(this);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    public void OnMouseDown()
    {
        SelectObject();
    }

    void SelectObject()
    {
        selected = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        foreach (MoveToMouse obj in gameManager.moveableObjects)
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