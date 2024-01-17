using UnityEngine;

namespace BrickBreaker_Sample
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float speed = 10f; // プレイヤーの移動の速さ
        Rigidbody _myRigidbody;

        void Start() {
            _myRigidbody = GetComponent<Rigidbody>();// Rigidbodyにアクセスして変数に保持
        }
        void Update() {   
            _myRigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, 0f);// 左右のキー入力により速度を変更する
        }

    }
}