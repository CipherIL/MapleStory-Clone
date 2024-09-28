using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowTarget : MonoBehaviour
{
    public Transform target;
    public MapBounder mapBounder;
    [SerializeField] private Vector3 cameraSpeed = Vector3.zero; // Velocity used by SmoothDamp
    [SerializeField] private float smoothTime = 0.3f; // Smooth time for the camera delay

    private float _camHalfHeight;
    private float _camHalfWidth;

    private void Awake()
    {
        Camera mainCamera = Camera.main;
        _camHalfHeight = mainCamera.orthographicSize;
        _camHalfWidth = mainCamera.aspect * _camHalfHeight;
        PositionOnTarget();
    }

    private Vector2 GetMaxBounds()
    {
        if (!mapBounder)
            return Vector2.positiveInfinity;

        Vector2 maxBounds = mapBounder.GetMaxBounds();
        return new Vector2(maxBounds.x - _camHalfWidth, maxBounds.y - _camHalfHeight);
    }

    private Vector2 GetMinBounds()
    {
        if (!mapBounder)
            return Vector2.negativeInfinity;

        Vector2 maxBounds = mapBounder.GetMinBounds();
        return new Vector2(maxBounds.x + _camHalfWidth, maxBounds.y + _camHalfHeight);
    }

    private void PositionOnTarget()
    {
        if (!target)
            throw new SystemException("No target assigned");

        Vector2 maxBounds = GetMaxBounds();
        Vector2 minBounds = GetMinBounds();
        float horizontalPos = Math.Clamp(target.position.x, minBounds.x, maxBounds.x);
        float verticalPos = Math.Clamp(target.position.y, minBounds.y, maxBounds.y);
        transform.position = new Vector3(horizontalPos, verticalPos, -10f);
    }

    private void LateUpdate()
    {
        if (!target)
            throw new SystemException("No target assigned");

        Vector2 maxBounds = GetMaxBounds();
        Vector2 minBounds = GetMinBounds();
        float horizontalPos = Math.Clamp(target.position.x, minBounds.x, maxBounds.x);
        float verticalPos = Math.Clamp(target.position.y, minBounds.y, maxBounds.y);
        Vector3 position = new Vector3(horizontalPos, verticalPos, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref cameraSpeed, smoothTime);
    }
}