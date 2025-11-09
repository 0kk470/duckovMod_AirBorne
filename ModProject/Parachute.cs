using ECM2;
using UnityEngine;

namespace Airborne
{
    public class Parachute : MonoBehaviour
    {
        private CharacterMainControl m_Character;

        private CharacterMovement m_Movement;

        [SerializeField]
        private float m_DropSpeed = -5f;

        private bool m_IsLanded = false;

        public void BindCharacter(CharacterMainControl character)
        {
            m_IsLanded = false;
            m_Character = character;
            m_Movement = character.GetComponentInChildren<CharacterMovement>();
            transform.SetParent(m_Character.characterModel.transform);
            transform.localPosition = Vector3.up * 0.1f;
        }


        void Update()
        {
            if (m_Character == null)
                return;
            if(m_Movement != null)
                m_Movement.velocity.y = m_DropSpeed;
            if (m_Character.IsOnGround && !m_IsLanded)
            {
                m_IsLanded = true;
                Destroy(this.gameObject, 0.2f);
            }
        }
    }
}
