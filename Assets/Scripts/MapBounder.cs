using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBounder : MonoBehaviour
{
    private Vector2 _maxBounds;
    private Vector2 _minBounds;

    private void Awake()
    {
        Bounds mapBounds = GetComponent<Renderer>().bounds;
        _maxBounds = new Vector2(mapBounds.max.x, mapBounds.max.y);
        //+2f to keep the camera snug above the level bottom edge
        _minBounds = new Vector2(mapBounds.min.x, mapBounds.min.y + 2f);
    }

    public Vector2 GetMaxBounds()
    {
        return _maxBounds;
    }

    public Vector2 GetMinBounds()
    {
        return _minBounds;
    }
}