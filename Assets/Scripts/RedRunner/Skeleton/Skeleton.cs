using UnityEngine;

namespace RedRunner
{
    public class Skeleton : MonoBehaviour
    {
        #region Delegates

        public delegate void ActiveChangedHandler(bool active);

        #endregion

        #region Events

        public event ActiveChangedHandler OnActiveChanged;

        #endregion

        #region Fields

        [Header("Skeleton")] [Space] [SerializeField]
        private Rigidbody2D m_Body;

        [SerializeField] private Rigidbody2D m_RightFoot;
        [SerializeField] private Rigidbody2D m_LeftFoot;
        [SerializeField] private Rigidbody2D m_RightHand;
        [SerializeField] private Rigidbody2D m_LeftHand;
        [SerializeField] private Rigidbody2D m_RightArm;
        [SerializeField] private Rigidbody2D m_LeftArm;
        [SerializeField] private Transform m_LeftEye;
        [SerializeField] private Transform m_RightEye;
        [SerializeField] private bool m_IsActive = false;

        #endregion

        #region Properties

        public Rigidbody2D Body => m_Body;

        public Rigidbody2D RightFoot => m_RightFoot;

        public Rigidbody2D LeftFoot => m_LeftFoot;

        public Rigidbody2D RightHand => m_RightHand;

        public Rigidbody2D LeftHand => m_LeftHand;

        public Rigidbody2D RightArm => m_RightArm;

        public Rigidbody2D LeftArm => m_LeftArm;

        public Transform LeftEye => m_LeftEye;

        public Transform RightEye => m_RightEye;

        public bool IsActive => m_IsActive;

        #endregion

        #region Public Methods

        public void SetActive(bool active, Vector2 velocity)
        {
            if (m_IsActive != active)
            {
                if (!active)
                {
                    m_Body.velocity = velocity;
                }

                m_IsActive = active;
                m_Body.simulated = active;
                m_RightFoot.simulated = active;
                m_LeftFoot.simulated = active;
                m_RightHand.simulated = active;
                m_LeftHand.simulated = active;
                m_RightArm.simulated = active;
                m_LeftArm.simulated = active;
                OnActiveChanged?.Invoke(active);
            }
        }

        #endregion
    }
}
