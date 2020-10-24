using UnityEngine;

public class BallSpeedUp : MonoBehaviour
{

  public bool IsDashing => _dashingElased > 0;
  public int KillBonus = 4;

  public float Fill => 1f - _dashCooldown / 8f;
  private float _dashingElased;
  private float _dashCooldown;
  
  private Rigidbody _rigidbody;

  void Awake()
  {
    _rigidbody = GetComponent<Rigidbody>();
  }
  public void Use(Vector3 forward)
  {
    if (_dashCooldown < 0)
    {
      forward.y = 0;
      forward.Normalize();

      _rigidbody.velocity = new Vector3();
      _rigidbody.AddForce(forward * 1300);
      _dashCooldown = 8;
      _dashingElased = 1f;
    }
  }

  private void Update()
  {
    _dashCooldown -= Time.deltaTime;
    _dashingElased -= Time.deltaTime;
  }
}