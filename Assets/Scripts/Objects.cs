using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public abstract class Objects : MonoBehaviour
{
    public int id;
    [SerializeField] protected bool draggable;

    protected Checker checker;
    protected Vector2 touchPosition;

    protected bool isInputActive;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    public GameItems nextObjects;
    public GameItems currentObject;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        checker = FindObjectOfType<Checker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected bool check;

    protected Collider2D lastCollider;

    protected virtual void OnMouseDrag()
    {
        if (draggable)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isInputActive = IsTouchingObject(touchPosition);

            if (isInputActive)
            {
                boxCollider.isTrigger = true;
                spriteRenderer.sortingOrder = 1;
                transform.position = touchPosition;
            }
        }
    }

    protected virtual void OnMouseUp()
    {
        lastCollider = null;
        checker.isActive = false;
        isInputActive = false;
        FindNearestEmptyPosition(transform.position);
    }

    bool IsTouchingObject(Vector2 touchPos)
    {
        if (lastCollider == null && !checker.isActive)
        {
            Collider2D collider = Physics2D.OverlapPoint(touchPos);

            if (collider?.gameObject == gameObject)
            {
                lastCollider = collider;
                checker.isActive = true;
            }
        }

        return lastCollider == null ? false : true;
    }

    //When Item spawned
    protected void FindNearestEmptyPosition(Vector2 fromPosition)
    {
        BoxPoint nearestEmptyPoint = checker.boxPoints
        .Where(point => !point.occupied)
        .OrderBy(point => Vector2.Distance(fromPosition, point.transform.position))
        .FirstOrDefault();

        isInputActive = false;
        boxCollider.isTrigger = false;

        if (nearestEmptyPoint != null)
        {
            nearestEmptyPoint.occupied = true;
            nearestEmptyPoint.objectID = id;
        }

        float duration = 0.3f;

        transform.DOMove(nearestEmptyPoint.transform.position, duration);

        StartCoroutine(Delay(duration + 1));
    }

    private void OnDisable()
    {
        lastCollider = null;
        checker.isActive = false;
    }

    IEnumerator Delay(float duration)
    {
        yield return new WaitForSeconds(duration + 0.1f);
        spriteRenderer.sortingOrder = 0;
    }
}

[Serializable]
public class ObjectsToGenerate
{
    public Vector2 chance;
    public GameObject objects;
}