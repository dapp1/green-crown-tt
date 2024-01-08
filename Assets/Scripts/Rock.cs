using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Rock : MonoBehaviour
{
    public GameItems removeObject;
    public int id;

    private void Start()
    {
        List<BoxPoint> list = FindObjectOfType<Checker>().boxPoints;
        CheckFor(list);
    }
    void CheckFor(List<BoxPoint> list)
    {
        BoxPoint nearestEmptyPoint = list
        .Where(point => !point.occupied)
        .OrderBy(point => Vector2.Distance(transform.position, point.transform.position))
        .FirstOrDefault();

        nearestEmptyPoint.occupied = true;
        nearestEmptyPoint.objectID = id;
    }
}
