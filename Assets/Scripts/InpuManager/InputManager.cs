using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMaskPlacement;
    private Vector3 _lastPosition;
    
    public event Action OnClicked, OnExit;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnExit?.Invoke();
        }   
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3? GetSelectMapPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _layerMaskPlacement))
        {
            _lastPosition = hit.point;
            return _lastPosition;
        }

        return null;
    }
}
