using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowTarget : MonoBehaviour
{
    public Transform target;
    public GameObject map;
    [SerializeField] private Vector3 cameraSpeed = Vector3.zero; // Velocity used by SmoothDamp
    [SerializeField] private float smoothTime = 0.3f;    // Smooth time for the camera delay

    private Vector2 _maxBounds;
    private Vector2 _minBounds;
    private void Awake()
    {
        SetCameraBoundaries();
        PositionOnTarget();
    }
    private void SetCameraBoundaries()
    {
        if (!map)
        {
            _maxBounds = Vector2.positiveInfinity;
            _minBounds = Vector2.negativeInfinity;
            return;
        }

        // Calculate the camera's half dimensions
        Camera mainCamera = Camera.main;
        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = mainCamera.aspect * camHalfHeight;

        // Get the map bounds from its Renderer (e.g., SpriteRenderer or TilemapRenderer)
        Bounds mapBounds = map.GetComponent<Renderer>().bounds;
        _maxBounds = new Vector2(mapBounds.max.x - camHalfWidth, mapBounds.max.y - camHalfHeight);
        _minBounds = new Vector2(mapBounds.min.x + camHalfWidth, mapBounds.min.y + camHalfHeight + 2f); //+2f to keep the camera snug above the level bottom edge
    }
    private void PositionOnTarget()
    {
        if (!target)
            throw new SystemException("No target assigned");

        float horizontalPos = Math.Clamp(target.position.x, _minBounds.x, _maxBounds.x);
        float verticalPos = Math.Clamp(target.position.y, _minBounds.y, _maxBounds.y);
        transform.position = new Vector3(horizontalPos, verticalPos, -10f);
    }
    private void LateUpdate()
    {
        if (!target)
            throw new SystemException("No target assigned");

        float horizontalPos = Math.Clamp(target.position.x, _minBounds.x, _maxBounds.x);
        float verticalPos = Math.Clamp(target.position.y, _minBounds.y, _maxBounds.y);
        Vector3 position = new Vector3(horizontalPos, verticalPos, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref cameraSpeed, smoothTime);
    }
}