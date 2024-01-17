using UnityEngine;

namespace Circle_Sample {
    public class Circle : MonoBehaviour
    {
        [SerializeField] GameObject createObject; // 生成するオブジェクト
        [SerializeField] int itemCount = 10; // 生成するオブジェクトの数
        [SerializeField] float radius = 5f; // 半径
        [SerializeField] float repeat = 1f; // 何周するか

        void Start() {

            var oneCycle = 2.0f * Mathf.PI; // sin の周期は 2π

            for (var i = 0; i < itemCount; ++i) {
                var point = ((float)i / itemCount) * oneCycle; //  (1.0 = 100% の時 2π となる)
                var repeatPoint = point * repeat; // 繰り返す

                var x = Mathf.Cos(repeatPoint) * radius;//x座標
                var y = Mathf.Sin(repeatPoint) * radius;//y座標

                var position = new Vector3(x, y);//実際の位置

                Instantiate( createObject, position, Quaternion.identity, transform);

            }
        }

    }
}


