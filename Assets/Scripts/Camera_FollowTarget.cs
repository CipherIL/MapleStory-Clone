using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowTarget : MonoBehaviour
{
    public Transform target;
    public EdgeCollider2D boundingEdge;
    [SerializeField] private Vector3 cameraSpeed = Vector3.zero; // Velocity used by SmoothDamp
    [SerializeField] private float smoothTime = 0.3f;    // Smooth time for the camera delay
    
    private float _topBoundary;
    private float _bottomBoundary;
    private float _leftBoundary;
    private float _rightBoundary;

    private void Awake()
    {
        SetCameraBoundaries();
        PositionOnTarget();
    }
    private void SetCameraBoundaries()
    {
        if (!boundingEdge)
        {
            _topBoundary = Single.PositiveInfinity;
            _bottomBoundary = Single.NegativeInfinity;
            _leftBoundary = Single.NegativeInfinity;
            _rightBoundary = Single.PositiveInfinity;
            return;
        }

        // Calculate the camera's half dimensions
        Camera mainCamera = Camera.main;
        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = mainCamera.aspect * camHalfHeight;

        // Calculate min and max bounds from the edge collider
        Vector2[] points = boundingEdge.points;
        float maxY = points[0].y;
        float minY = points[0].y;
        float maxX = points[0].x;
        float minX = points[0].x;

        // Loop through the points to find the extreme bounds
        foreach (var point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.x > maxX) maxX = point.x;
            if (point.y > maxY) maxY = point.y;
        }

        _topBoundary = maxY - camHalfHeight;
        _bottomBoundary = minY + camHalfHeight;
        _leftBoundary = minX + camHalfWidth;
        _rightBoundary = maxX - camHalfWidth;
    }
    private void PositionOnTarget()
    {
        if (!target)
            throw new SystemException("No target assigned");

        float horizontalPos = Math.Clamp(target.position.x, _leftBoundary, _rightBoundary);
        float verticalPos = Math.Clamp(target.position.y, _bottomBoundary, _topBoundary);
        transform.position = new Vector3(horizontalPos, verticalPos, -10f);
    }
    
    void LateUpdate()
    {
        if (!target)
            throw new SystemException("No target assigned");

        float horizontalPos = Math.Clamp(target.position.x, _leftBoundary, _rightBoundary);
        float verticalPos = Math.Clamp(target.position.y, _bottomBoundary, _topBoundary);
        Vector3 position = new Vector3(horizontalPos, verticalPos, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref cameraSpeed, smoothTime);
    }
}