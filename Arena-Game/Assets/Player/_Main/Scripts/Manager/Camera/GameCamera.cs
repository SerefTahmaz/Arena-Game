using UnityEngine;

namespace Cinemachine.Manager
{
    public class GameCamera : MonoBehaviour
    {
        public GameObject m_Cam;
        public cCamShake m_CameraShake;

        public void SetActive(bool value)
        {
            m_Cam.SetActive(value);
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual bool IsAvailable()
        {
            return true;
        }
    }
}