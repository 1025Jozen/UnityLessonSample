using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;

    [SerializeField] Vector3 startVelocity;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float power = 1;
    [SerializeField] Transform particleParent;

    void Start() {
        velocity = startVelocity;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(velocity , ForceMode.Impulse);
    }

    void Update()
    {
        rb.velocity = velocity * power;
    }

    private void OnCollisionEnter(Collision collision)
    {
        velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);//第1引数:入力方向　第2引数:法線　//反射結果が変える

        ParticleSystem particleObj = Instantiate(particle, rb.transform.position, Quaternion.identity);
        particleObj.transform.parent = particleParent;

        //平面に沿って何かしたい場合
        //Vector3 onPlane = Vector3.ProjectOnPlane(velocity, collision.contacts[0].normal);//平面に沿ったベクトル
        //particleObj.transform.rotation = Quaternion.LookRotation(-onPlane);//平面に沿って

        particleObj.transform.rotation = Quaternion.LookRotation(velocity);//単純に反射方向に
    }


}
