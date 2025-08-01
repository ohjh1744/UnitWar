using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // 카메라 이동 속도

    [SerializeField] private float _borderDistance; // 화면 가장자리와의 거리 (픽셀)

    [SerializeField] private Vector2 _moveBoundaryMin; // 카메라 이동 범위 최소값

    [SerializeField] private Vector2 _moveBoundaryMax; // 카메라 이동 범위 최대값

    [SerializeField] private Vector3 _targetPosition; // 카메라가 이동할 목표 위치

    private bool _isSet;
    

    void Update()
    {
        if(GameSceneManager.Instance.IsSetCam)
        {
            SetCamera();
            MoveCamera();
        }
    }

    private void SetCamera()
    {
        if(_isSet == false)
        {
            _targetPosition = transform.position;
            _isSet = true;
        }
    }

    private void MoveCamera()
    {
        // 마우스 위치를 스크린 공간에서 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 마우스가 화면 경계를 넘으려 할 때 카메라 이동
        if (mousePosition.x <= _borderDistance)
        {
            _targetPosition.x -= _moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= Screen.width - _borderDistance)
        {
            _targetPosition.x += _moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= _borderDistance)
        {
            _targetPosition.y -= _moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= Screen.height - _borderDistance)
        {
            _targetPosition.y += _moveSpeed * Time.deltaTime;
        }
        //Debug.Log($"{ _targetPosition.x}, { _targetPosition.y}");
         //카메라 이동 범위 제한
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _moveBoundaryMin.x, _moveBoundaryMax.x);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _moveBoundaryMin.y, _moveBoundaryMax.y);

        // 카메라 이동
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.1f);
    }
}
