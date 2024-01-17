using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roll_Cube_Sample {
    public class Roll_Cube1 : MonoBehaviour
    {
        public float rotationPeriod = 0.3f;     // 隣に移動するのにかかる時間
        public float sideLength = 1f;           // Cubeの辺の長さ

        bool isRotate = false;                  // Cubeが回転中かどうかを検出するフラグ
        float directionX = 0;                   // 回転方向フラグ
        float directionZ = 0;                   // 回転方向フラグ

        Vector3 startPos;                       // 回転前のCubeの位置
        float rotationTime = 0;                 // 回転中の時間経過
        float radius;                           // 重心の軌道半径
        Quaternion fromRotation;                // 回転前のCubeのクォータニオン
        Quaternion toRotation;                  // 回転後のCubeのクォータニオン

        void Start() {
            // 重心の回転軌道半径を計算
            radius = sideLength * Mathf.Sqrt(2f) / 2f; //回転軸にするところから重心までの長さ   //45度の三角形の辺の割合 1:1:√2
        }

        void Update() {

            float x = 0;
            float y = 0;

            // 水平移動優先　
            x = Input.GetAxisRaw("Horizontal");
            if (x == 0) {
                y = Input.GetAxisRaw("Vertical");
            }


            // キー入力がある　かつ　Cubeが回転中でない場合、Cubeを回転させる。
            if ((x != 0 || y != 0) && !isRotate) {
                directionX = y;                                                         // 回転方向セット (x,yどちらかは必ず0)
                directionZ = x;                                                         // 回転方向セット (x,yどちらかは必ず0)
                startPos = transform.position;                                          // 回転前の座標を保持
                fromRotation = transform.rotation;                                      // 回転前の回転情報(クォータニオン)を保持
                transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);// 回転方向に90度回転させる z軸方向に動かす(x軸回転させる)　x軸方向に動かす(z軸回転させる)　　   　(y軸回転はさせない)
                toRotation = transform.rotation;                                        // 回転後の回転情報(クォータニオン)を保持
                transform.rotation = fromRotation;                                      // CubeのRotationを回転前に戻す。（
                rotationTime = 0;                                                           // 回転中の経過時間を0に。
                isRotate = true;                                                            // 回転中フラグをたてる。
            }
        }

        void FixedUpdate() {

            if (isRotate) {//回転中の場合

                rotationTime += Time.fixedDeltaTime;                                    // 経過時間を増やす
                float ratio = Mathf.Lerp(0, 1, rotationTime / rotationPeriod);          // 回転の時間に対する今の経過時間の割合

                // 移動
                float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);                   // 回転角をラジアンで。
                float distanceX = -directionX * radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));      // X軸の移動距離。 -の符号はキーと移動の向きを合わせるため。
                float distanceY = radius * (Mathf.Sin(45f * Mathf.Deg2Rad + thetaRad) - Mathf.Sin(45f * Mathf.Deg2Rad));                        // Y軸の移動距離
                float distanceZ = directionZ * radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));           // Z軸の移動距離
                transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);                       // 現在の位置をセット

                // 回転
                transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);      // Quaternion.Lerpで現在の回転から次の回転までをセット

                // 回転時間終了時、　移動・回転終了時に各パラメータを初期化。isRotateフラグを下ろす。
                if (ratio == 1) {
                    isRotate = false;
                    directionX = 0;
                    directionZ = 0;
                    rotationTime = 0;
                }
            }
        }
    }
}

