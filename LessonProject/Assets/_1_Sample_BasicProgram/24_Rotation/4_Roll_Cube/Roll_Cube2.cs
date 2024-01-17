using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roll_Cube_Sample {
    public class Roll_Cube2 : MonoBehaviour
    {
        public float rotationPeriod = 0.3f;     // 隣に移動するのにかかる時間
        Vector3 scale;                          // 直方体のサイズ

        bool isRotate = false;                  // Cubeが回転中かどうかを検出するフラグ
        float directionX = 0;                   // 回転方向フラグ
        float directionZ = 0;                   // 回転方向フラグ

        float startAngleRad = 0;                // 回転前の重心の水平面からの角度
        Vector3 startPos;                       // 回転前のCubeの位置
        float rotationTime = 0;                 // 回転中の時間経過
        float radius = 1;                       // 重心の軌道半径 (とりあえず仮で1)
        Quaternion fromRotation;                // 回転前のCubeのクォータニオン
        Quaternion toRotation;                  // 回転後のCubeのクォータニオン

        // Use this for initialization
        void Start() {

            // 直方体のサイズを取得
            scale = transform.lossyScale;
            //Debug.Log ("[x, y, z] = [" + scale.x + ", " + scale.y + ", " + scale.z + "]");

        }

        // Update is called once per frame
        void Update() {

            float x = 0;
            float y = 0;

            // キー入力を拾う。
            x = Input.GetAxisRaw("Horizontal");
            if (x == 0) {
                y = Input.GetAxisRaw("Vertical");
            }


            // キー入力がある　かつ　Cubeが回転中でない場合、Cubeを回転する。
            if ((x != 0 || y != 0) && !isRotate) {
                directionX = y;                                                             // 回転方向セット (x,yどちらかは必ず0)
                directionZ = x;                                                             // 回転方向セット (x,yどちらかは必ず0)
                startPos = transform.position;                                              // 回転前の座標を保持
                fromRotation = transform.rotation;                                          // 回転前のクォータニオンを保持
                transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);     // 回転方向に90度回転させる
                toRotation = transform.rotation;                                            // 回転後のクォータニオンを保持
                transform.rotation = fromRotation;                                          // CubeのRotationを回転前に戻す。（transformのシャローコピーとかできないんだろうか…。）
                setRadius();                                                                // 回転半径を計算
                rotationTime = 0;                                                           // 回転中の経過時間を0に。
                isRotate = true;                                                            // 回転中フラグをたてる。
            }
        }

        void FixedUpdate() {

            if (isRotate) {

                rotationTime += Time.fixedDeltaTime;                                    // 経過時間を増やす
                float ratio = Mathf.Lerp(0, 1, rotationTime / rotationPeriod);          // 回転の時間に対する今の経過時間の割合

                // 移動
                float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);                   // 回転角をラジアンで。
                float distanceX = -directionX * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + thetaRad));      // X軸の移動距離。 -の符号はキーと移動の向きを合わせるため。
                float distanceY = radius * (Mathf.Sin(startAngleRad + thetaRad) - Mathf.Sin(startAngleRad));                        // Y軸の移動距離
                float distanceZ = directionZ * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + thetaRad));           // Z軸の移動距離
                transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);           // 現在の位置をセット

                // 回転
                transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);      // Quaternion.Lerpで現在の回転角をセット（なんて便利な関数）

                // 移動・回転終了時に各パラメータを初期化。isRotateフラグを下ろす。
                if (ratio == 1) {
                    isRotate = false;
                    directionX = 0;
                    directionZ = 0;
                    rotationTime = 0;
                }
            }
        }

        void setRadius() {

            Vector3 dirVec = new Vector3(0, 0, 0);          // 移動方向ベクトル
            Vector3 nomVec = Vector3.up;                    // (0,1,0) グローバル座標

            // 移動方向をベクトルに変換
            if (directionX != 0) {                          // X方向に移動
                dirVec = Vector3.right;                     // (1,0,0)
            } else if (directionZ != 0) {                   // Z方向に移動
                dirVec = Vector3.forward;                   // (0,0,1)
            }

            // 移動方向ベクトルとObjectの向きの内積から移動方向のradiusとstartAngleを計算

            //移動する向きのベクトルとObjectの向きが同じであれば内積が1になる
            //数値計算の誤差でピッタリ1にならないことがあるので、”>0.99″を使っている。
            if (Mathf.Abs(Vector3.Dot(transform.right, dirVec)) > 0.99) {                       // 移動方向がobjectのx方向
                if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99) {                  // globalのy軸がobjectのy方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f)); // 回転半径
                    startAngleRad = Mathf.Atan2(scale.y, scale.x);                              // 回転前の重心の水平面からの角度
                } else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99) {      // globalのy軸がobjectのz方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                    startAngleRad = Mathf.Atan2(scale.z, scale.x);
                }

            } else if (Mathf.Abs(Vector3.Dot(transform.up, dirVec)) > 0.99) {                   // 移動方向がobjectのy方向
                if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99) {                   // globalのy軸がobjectのx方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                    startAngleRad = Mathf.Atan2(scale.x, scale.y);
                } else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99) {      // globalのy軸がobjectのz方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                    startAngleRad = Mathf.Atan2(scale.z, scale.y);
                }
            } else if (Mathf.Abs(Vector3.Dot(transform.forward, dirVec)) > 0.99) {              // 移動方向がobjectのz方向
                if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99) {                   // globalのy軸がobjectのx方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                    startAngleRad = Mathf.Atan2(scale.x, scale.z);
                } else if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99) {               // globalのy軸がobjectのy方向
                    radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                    startAngleRad = Mathf.Atan2(scale.y, scale.z);
                }
            }
            //Debug.Log (radius + ", " + startAngleRad);
        }
    }
}

