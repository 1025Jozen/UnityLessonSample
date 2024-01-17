using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glass {
    public class Glass : MonoBehaviour
    {
        //[SerializeField] GameObject shatteredVersion;
        //[ContextMenu("Shatter")]
        //public void Break()
        //{
        //    GameObject shatteredGlass = Instantiate(shatteredVersion , transform.position , transform.rotation);
        //    shatteredGlass.transform.localScale = transform.localScale;
        //    Destroy(gameObject);
        //}


        [SerializeField] ShatterdGlass shatteredVersion;
        
        public void Break(Vector3 projectilePosition ) {
            ShatterdGlass shatteredGlass = Instantiate(shatteredVersion, transform.position, transform.rotation);
            shatteredGlass.transform.localScale = transform.localScale;
            shatteredGlass.ApplyForce(projectilePosition);

            Destroy(gameObject);
        }
    }
}