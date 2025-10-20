using UnityEngine;

public class CanvasHPFollowToUnit : MonoBehaviour
{
    private GameObject _unityForFollow;
    private void Start()
    {
        _unityForFollow = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void LateUpdate()
    {
        transform.rotation = _unityForFollow.transform.rotation;
    }
}
