using Duckov.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Airborne
{
    public class AirPlane : MonoBehaviour
    {
        private CharacterMainControl m_LocalPlayer;

        private Vector3 m_StartPoint;

        private Vector3 m_EndPoint;

        private bool m_IsEnabled = false;

        private InputAction m_DashAction;

        private bool m_isPlayerLeaved = false;

        private Transform m_PlayerPrevParent;

        [SerializeField]
        private float m_FlySpeed = 5f;

        void Awake()
        {
            m_DashAction = GameManager.MainPlayerInput.actions["Dash"];
            if (m_DashAction != null)
            {
                m_DashAction.performed -= OnDashPerformed;
                m_DashAction.performed += OnDashPerformed;
            }
        }

        void OnDestory()
        {
            if(m_DashAction != null)
            {
                m_DashAction.performed -= OnDashPerformed;
            }
        }


        void Update()
        {
            if (!m_IsEnabled)
                return;

            var dir = (m_EndPoint - m_StartPoint).normalized;
            transform.position += dir * Time.deltaTime * m_FlySpeed;

            if (IsReachEnd())
            {
                m_IsEnabled = false;
                PlayerJumpOut();
            }
        }


        void PlayerJumpOut()
        {
            if (m_isPlayerLeaved)
                return;
            if (m_LocalPlayer == null)
                return;
            m_LocalPlayer.transform.SetParent(m_PlayerPrevParent);
            m_LocalPlayer.gameObject.SetActive(true);
            m_isPlayerLeaved = true;
            Destroy(gameObject, 1f);
        }

        bool IsReachEnd()
        {
            var distance = Vector3.Distance(transform.position, m_EndPoint);
            return distance <= 0.5f;
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (!m_IsEnabled)
                return;
            if (m_isPlayerLeaved)
                return;
            if(context.started)
            {
                PlayerJumpOut();
            }
        }


        public void BeginFly(Vector3 startPos, Vector3 endPos, CharacterMainControl player)
        {
            // 先设置飞机位置
            m_StartPoint = startPos;
            m_EndPoint = endPos;
            transform.position = startPos;
            transform.LookAt(endPos, Vector3.up);
            m_LocalPlayer = player;
            m_LocalPlayer.gameObject.SetActive(false);
            m_PlayerPrevParent = m_LocalPlayer.transform.parent;
            m_LocalPlayer.transform.SetParent(transform);
            m_LocalPlayer.transform.localPosition = Vector3.zero;
            m_IsEnabled = true;

        }
    }
}
