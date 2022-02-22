using UnityEngine;

namespace Source.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class GameplayCamera : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        private void Awake() => _camera = GetComponent<UnityEngine.Camera>();

        public void SetCamera(Transform target)
        {
            Vector3 centerOfTarget = GetCenterOfTarget(target);
            transform.LookAt(centerOfTarget);
            // transform.position += centerOfTarget;
        }
        
        private Vector3 GetCenterOfTarget(Transform target)
        {
            Vector3 sum = Vector3.zero;
            
            foreach (Transform child in target.transform)
                sum += child.transform.position;

            return sum / target.transform.childCount;
        }
    }
}