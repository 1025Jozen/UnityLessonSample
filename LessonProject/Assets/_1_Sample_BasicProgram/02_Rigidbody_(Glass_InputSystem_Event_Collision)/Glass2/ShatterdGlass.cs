using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当たった位置で爆発するように変えていく
namespace Glass
{
    public class ShatterdGlass : AutoDestroy //
    {
        [SerializeField] float explosiveForce  = 500f;
        [SerializeField] float explosiveRadius = 1f;

        public void ApplyForce(Vector3 projectilePosition) {
            Collider[] colliders = Physics.OverlapSphere(projectilePosition,explosiveRadius);//球型で当たり判定を取り
            foreach (Collider collider in colliders) {
                if (collider.TryGetComponent(out Rigidbody rigidbody)) {//rigidbodyが付いてれば、爆発させる
                    rigidbody.AddExplosionForce(explosiveForce,projectilePosition,explosiveRadius);
                }
            }
        }
    }
}

