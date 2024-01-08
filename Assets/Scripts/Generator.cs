using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickHandler))]
[RequireComponent(typeof(BoxCollider2D))]
public class Generator : Objects
{
    private ClickHandler _clickHandler;
    [SerializeField] private List<ObjectsToGenerate> objectsToGenerate;

    private int count;

    protected override void Awake()
    {
        _clickHandler = GetComponent<ClickHandler>();
        _clickHandler.clicked += Generate;
        base.Awake();
    }
    protected override void OnMouseDrag()
    {

    }
    protected override void OnMouseUp()
    {
        
    }
    public void Generate()
    {
        if (!HasFreeSpace()) return;

        int randomChance = UnityEngine.Random.Range(0, 100);

        foreach (ObjectsToGenerate obj in objectsToGenerate)
        {
            if (randomChance >= obj.chance.x && randomChance < obj.chance.y)
            {
                GameObject nextObj = Instantiate(obj.objects);
                nextObj.transform.position = transform.position;
                nextObj.GetComponent<Item>().id = count + 1;
                count++;
            }
        }
    }

    bool HasFreeSpace()
    {
        bool hasSpace = false;

        for (int i = 0; i < checker.boxPoints.Count; i++)
        {
            if (!checker.boxPoints[i].occupied)
            {
                hasSpace = true;
            }
        }

        return hasSpace;
    }
}
