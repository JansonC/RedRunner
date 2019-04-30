using UnityEngine;
using RedRunner.Characters;

namespace RedRunner.Enemies
{
    public class Eye : MonoBehaviour
    {
        [SerializeField] protected float m_Radius = 1f;
        [SerializeField] protected Transform m_Pupil;
        [SerializeField] protected Transform m_Eyelid;
        [SerializeField] protected float m_MaximumDistance = 5f;
        [SerializeField] protected Character m_LatestCharacter;
        [SerializeField] protected Vector3 m_InitialPosition;
        [SerializeField] protected float m_Speed = 0.01f;
        [SerializeField] protected float m_DeadSpeed = 0.005f;
        [SerializeField] protected Vector3 m_DeadPosition;
        protected Vector3 m_PupilDestination;

        public virtual float Radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        public virtual Transform Pupil => m_Pupil;

        public virtual Vector3 InitialPosition => m_InitialPosition;

        public virtual Vector3 PupilDestination => m_PupilDestination;

        public virtual float Speed => m_Speed;

        protected virtual void Awake()
        {
//			m_InitialPosition = m_Pupil.transform.position;
        }

        protected virtual void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.parent.position, m_MaximumDistance,
                LayerMask.GetMask("Characters"));
            foreach (Collider2D t in colliders)
            {
                Character character = t.GetComponent<Character>();
                if (character != null)
                {
                    m_LatestCharacter = character;
                }
            }

            SetupPupil();
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, m_Radius);
            Gizmos.DrawWireSphere(transform.parent.position, m_MaximumDistance);
        }

        protected virtual void SetupPupil()
        {
            if (m_LatestCharacter != null)
            {
                float speed = m_Speed;
                Vector3 distanceToTarget = m_LatestCharacter.transform.position - m_Pupil.position;
                if (m_LatestCharacter.IsDead.Value)
                {
                    speed = m_DeadSpeed;
                    distanceToTarget = Vector3.ClampMagnitude(m_DeadPosition, m_Radius);
                    Vector3 finalPupilPosition = transform.position + distanceToTarget;
                    m_PupilDestination = finalPupilPosition;
                }
                else
                {
                    float distance = Vector3.Distance(m_LatestCharacter.transform.position, transform.parent.position);
                    distanceToTarget =
                        Vector3.ClampMagnitude(distance <= m_MaximumDistance ? distanceToTarget : m_InitialPosition,
                            m_Radius);
                    Vector3 finalPupilPosition = transform.position + distanceToTarget;
                    m_PupilDestination = finalPupilPosition;
                }

                m_Pupil.position = Vector3.MoveTowards(m_Pupil.position, m_PupilDestination, speed);
            }
        }
    }
}
