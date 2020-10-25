using System;
using Controllers;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class Ball : MonoBehaviour
    {
      public float Fill => 1f - _jumpCooldownElapsed / 3f;
      [SerializeField] public AnimationCurve PowerCurveBySize;
      [SerializeField] public AnimationCurve PowerJumpCurveBySize;
        [SerializeField] public float m_bonusMult = 1;
        [SerializeField] public float m_MovePowerBonus = 0;
        [SerializeField] public float m_MoveRunBonus = 1;
        [SerializeField] public float m_botFarBonus = 1;
        [SerializeField] public float m_TorgueBonus = 1;
        [SerializeField] public bool m_UseTorque = true; // Whether or not to use torque to move the ball.
        [SerializeField] public float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
        [SerializeField] public float m_JumpPower = 2; // The force added to the ball when it jumps.

        private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
        private Rigidbody m_Rigidbody;
        private SphereCollider _collider;

        private float _jumpCooldownElapsed;
        private bool _appliedJump;


        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
        }


        public void Move(Vector3 moveDirection, bool jump)
        {
          var power = PowerCurveBySize.Evaluate(_collider.radius);
          var powerWithBonus = (power + m_MovePowerBonus) * m_bonusMult * m_botFarBonus;
            if (m_UseTorque)
            {
                m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x)*powerWithBonus*m_TorgueBonus);
            }
            else
            {
                m_Rigidbody.AddForce(moveDirection*powerWithBonus);
            }
            
            if(m_MoveRunBonus > 1)
                m_Rigidbody.AddForce(moveDirection*m_MoveRunBonus);

            _jumpCooldownElapsed -= Time.fixedDeltaTime;
            
            // If on the ground and jump is pressed...
            if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength - 0.62f + _collider.radius) && jump && _jumpCooldownElapsed < 0f && !_appliedJump)
            {
                // ... add force in upwards.
                m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.VelocityChange);
                _appliedJump = true;
                AudioController.Instance.PlayJump(GetComponent<AudioSource>());
            }
            else
            {
              if (_appliedJump)
              {
                _jumpCooldownElapsed = 3f;
                _appliedJump = false;
              }
            }
        }
    }
}
