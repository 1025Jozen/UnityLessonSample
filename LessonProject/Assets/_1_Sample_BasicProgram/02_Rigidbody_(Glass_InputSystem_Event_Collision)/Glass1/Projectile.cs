using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glass {
    //発射物
    public class Projectile : AutoDestroy
    {
        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.TryGetComponent(out Glass glass)) {
        //        glass.Break();
        //    }
        //    Destroy(gameObject);
        //}

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.TryGetComponent(out Glass glass)) {
                glass.Break(transform.position);//発射物の当たったところを基準にグラスを爆発させる
            }

            Destroy(gameObject);
        }
    }
}

