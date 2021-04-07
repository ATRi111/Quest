using UnityEngine;
public class bullet1 : bullet
{
    const float rotateSpeed=10f;
    void Start()
    {
        damage = 4;
        criticalDamage = 8;
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed);
    }
}
