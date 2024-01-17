using System.Collections;
using UnityEngine;

namespace NoiseSample {
    public class Noise : MonoBehaviour
    {
        [SerializeField] private GameObject ball;
        public float timeSpeed = 0.3f;
        private float time = 0;

        void Start() {
            StartCoroutine("Create");
        }

        void Update() {
            time += Time.deltaTime * timeSpeed;
        }

        IEnumerator Create() {

            //while (true) {
            //    float rand = Random.Range(-5, 5);
            //    Instantiate(ball, new Vector3(rand * 1.2f, 7, 0), Quaternion.identity);
            //    yield return new WaitForSeconds(0.5f);
            //}

            //PerlinNoise  0~1   カメラ揺れなどに使ってる
            while (true) {
                float rand1 = Random.Range(-1, 1);
                float rand2 = Random.Range(-1, 1);
                float noise = Mathf.PerlinNoise(time * rand1, time * rand2);//0.0 < noise < 1.0
                Instantiate(ball, new Vector3(noise * 7, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }

            
        }
    }
}

