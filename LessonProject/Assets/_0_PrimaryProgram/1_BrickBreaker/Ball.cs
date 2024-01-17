using UnityEngine;

namespace BrickBreaker_Sample {
    public class Ball : MonoBehaviour
    {
        [SerializeField] float speed = 5f;// ボールの移動の速さを指定する変数

        [SerializeField] float minSpeed = 5f;// 速さの最小値を指定する変数を追加
        [SerializeField] float maxSpeed = 10f;// 速さの最大値を指定する変数を追加

        Rigidbody myRigidbody;

        AudioSource m_MyAudioSource;

        void Start() {
            myRigidbody = GetComponent<Rigidbody>();             // Rigidbodyにアクセスして変数に保持しておく    
            myRigidbody.velocity = new Vector3(speed, speed, 0f);// 右斜め45度に進む
            m_MyAudioSource = GetComponent<AudioSource>();
        }

        void Update() {// 毎フレーム速度をチェックする    
            Vector3 velocity = myRigidbody.velocity;// 現在の速度を取得
                                                    
            float clampedSpeed = Mathf.Clamp(velocity.magnitude, minSpeed, maxSpeed);//min maxを超えないように　速さを丸める
            myRigidbody.velocity = velocity.normalized * clampedSpeed;// 速度を変更
        }

        void OnCollisionEnter(Collision collisionInfo) {
            if (collisionInfo.gameObject.CompareTag("Player")) {
                m_MyAudioSource.Play();
            }
        }


    }
}

