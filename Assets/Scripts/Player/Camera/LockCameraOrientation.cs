using System.Linq;
using UnityEngine;

public class LockCameraOrientation : MonoBehaviour
{
    private void Start() { }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }
}