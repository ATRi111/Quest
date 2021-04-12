using UnityEngine;
public class bullet1 : CBullet
{
    const float rotateSpeed=10f;
    void Start()
    {
        Damage = 4;
        CriticalDamage = 8;
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed);
    }
}
