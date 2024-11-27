using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    private Vector3 _rightTop;                      // 카메라 뷰포트 우상단

    private Vector3 _leftTop;                       // 카메라 뷰포트 좌상단

    private Vector3 _rightBottom;                   // 카메라 뷰포트 우하단

    private Vector3 _leftBottom;                    // 카메라 뷰포트 좌하단

    [SerializeField] private GameObject _test;      // 테스트용 오브젝트

    [SerializeField] private Vector2 _mousePos;

    [SerializeField] Camera _camera;

    [SerializeField] CameraController _mainCam;


    private void Start()
    {
        _camera = GetComponent<Camera>();
        _rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        _leftTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        _rightBottom = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        _leftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        _test.transform.localScale = new Vector3(Vector3.Distance(_rightTop, _leftTop), Vector3.Distance(_rightTop, _rightBottom), 0);
    }
    private void Update()
    {
        //ChaseMainCam();
        //if(Input.GetMouseButtonDown(0))
        //{
        //    CheckMousePos();
        //}
    }

    private void ChaseMainCam()
    {
        _rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        _leftTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        _rightBottom = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        _leftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        //Debug.DrawLine(_rightTop, _leftTop, Color.red);
        //Debug.DrawLine(_leftTop, _leftBottom, Color.red);
        //Debug.DrawLine(_leftBottom, _rightBottom, Color.red);
        //Debug.DrawLine(_rightBottom, _rightTop, Color.red);

        _test.transform.position = Camera.main.transform.position + new Vector3(0, 0, 10);
    }

    private void CheckMousePos()
    {
        _mousePos = Input.mousePosition;
        _mousePos = _camera.ScreenToWorldPoint(_mousePos);
        StartCoroutine(PauseMainCamMove());
        Camera.main.transform.position = _mousePos;
    }

    IEnumerator PauseMainCamMove()
    {
        _mainCam.enabled = false;
        yield return new WaitForSeconds(1);
        _mainCam.enabled = true;
        yield break;
    }
}
