using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glass
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float projectileForce = 10;
        [SerializeField] Transform firePoint;     //発射位置
        [SerializeField] Rigidbody projectile;    //発射物
        //[SerializeField] AudioSource audioSource;

        private void Start()
        {
            Often.InputManager.Instance.OnFire += Fire;
        }

        private void Fire(object sender , System.EventArgs e) {
            Rigidbody newProjectile = Instantiate(projectile, firePoint.position , Quaternion.identity);
            newProjectile.AddForce(firePoint.forward * projectileForce, ForceMode.VelocityChange);//瞬間的に力を加える点ではImpulseと同じ。質量無視ver
            //audioSource.Play();
        }
    }
}