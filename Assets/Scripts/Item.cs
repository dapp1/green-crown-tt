using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : Objects
{
    bool _combinedObject;
    bool _isColliding;
    bool _isDestroyed;

    private void Start()
    {
        if (_combinedObject)
        {
            foreach (BoxPoint point in checker.boxPoints)
            {
                if (point.objectID == id)
                {
                    transform.DOMove(point.transform.position, 0.3f);
                }
            }
        } else
        {
            FindNearestEmptyPosition(transform.position);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            int id = (int)collision.gameObject.GetComponent<Item>().nextObjects;

            if (id == (int)this.nextObjects)
            {
                _isColliding = true;
            }
        }

        if (collision.gameObject.CompareTag("rock")) 
        {
            Rock rock = collision.gameObject.GetComponent<Rock>();

            if((int)currentObject == (int)rock.removeObject)
            {
                _isDestroyed = true;
            }
        }

    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            _isColliding = false;
        }

        if(collision.gameObject.CompareTag("rock"))
        {
            _isDestroyed = false;
        }
    }

    protected override void OnMouseUp()
    {
        checker.NotifyObjectStatusChange(this);

        if (!_isColliding && !_isDestroyed)
        {
            base.OnMouseUp();
        }else
        {
            CombineObjects();
        }
    }

    void CombineObjects()
    {
        if (Input.GetMouseButtonUp(0) && _isColliding || Input.GetMouseButtonUp(0) && _isDestroyed)
        {
            Item objectA = gameObject.GetComponent<Item>();
            GameObject objectB;

            if (objectA != null)
            {
                objectB = objectA.GetCollidedObject();

                Vector2 position = objectB.transform.position;

                if (objectB != null && _isColliding)
                {
                    int id = objectB.GetComponent<Item>().id;

                    Destroy(objectA.gameObject);
                    Destroy(objectB);

                    CreateNewObject((int)nextObjects, position, id);
                }

                if(objectB != null && _isDestroyed)
                {
                    int id = objectB.GetComponent<Rock>().id;

                    foreach (BoxPoint point in checker.boxPoints)
                    {
                        if (point.objectID == id)
                        {
                            point.objectID = 0;
                            point.occupied = false;
                        }
                    }

                    Destroy(objectA.gameObject);
                    Destroy(objectB);
                }
            }
        }
    }

    private void CreateNewObject(int newId, Vector2 position, int id)
    {
        if (Checker.Instance.prefabDictionary.ContainsKey(newId))
        {
            GameObject nextObj = Instantiate(checker.items[newId]);
            nextObj.transform.position = position;
            nextObj.GetComponent<Item>()._combinedObject = true;
            nextObj.GetComponent<Item>().id = id;
        }
    }

    public GameObject GetCollidedObject()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject != gameObject)
                {
                    if (col.TryGetComponent<Item>(out var otherObject))
                    {
                        return otherObject.gameObject;
                    }
                    else if (col.TryGetComponent<Rock>(out var otherObject2))
                    {
                        return otherObject2.gameObject;
                    }
                }
            }
        }
        return null;
    }
}
