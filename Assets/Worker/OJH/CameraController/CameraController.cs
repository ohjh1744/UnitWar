using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed; // 카메라 이동 속도

    [SerializeField] private float _borderDistance = 50f; // 화면 가장자리와의 거리 (픽셀)

    [SerializeField] private float _cameraScale = 2.5f; // 카메라 스케일

    [SerializeField] private Vector2 _moveBoundaryMin; // 카메라 이동 범위 최소값

    [SerializeField] private Vector2 _moveBoundaryMax; // 카메라 이동 범위 최대값

    [SerializeField] private Vector3 _targetPosition; // 카메라가 이동할 목표 위치


    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        // 마우스 위치를 스크린 공간에서 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 스케일을 고려한 마우스 좌표 변환
        mousePosition.x /= _cameraScale;
        mousePosition.y /= _cameraScale;

        // 마우스가 화면 경계를 넘으려 할 때 카메라 이동
        if (mousePosition.x <= _borderDistance)
        {
            _targetPosition.x -= _moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.x >= (Screen.width / _cameraScale) - _borderDistance)
        {
            _targetPosition.x += _moveSpeed * Time.deltaTime;
        }

        if (mousePosition.y <= _borderDistance)
        {
            _targetPosition.y -= _moveSpeed * Time.deltaTime;
        }
        else if (mousePosition.y >= (Screen.height / _cameraScale) - _borderDistance)
        {
            _targetPosition.y += _moveSpeed * Time.deltaTime;
        }

        // 카메라 이동 범위 제한 -> 맵 배경 구현 끝나면 맞춰서 수치 적용.
        //targetPosition.x = Mathf.Clamp(targetPosition.x, _moveBoundaryMin.x, _moveBoundaryMax.x);
        //targetPosition.z = Mathf.Clamp(targetPosition.z, _moveBoundaryMin.y, _moveBoundaryMax.y);

        // 카메라 이동
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.1f);
    }
}
