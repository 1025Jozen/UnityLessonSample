using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shader_Sample
{
    //マウスでなぞったところを凹ませる(絵を描く)
    public class DrawTracks : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] Shader _drawShader;//凹ませるための色を描く

        private RenderTexture _splatmap;// 新しいテクスチャを用意し、絵を描いて、   このテクスチャをもとに凹ませる　
        private Material _snowMaterial, _drawMaterial;//雪　

        private RaycastHit _hit;//マウス

        [SerializeField, Range(1, 500)] float _brushSize;
        [SerializeField, Range(0, 1)] float _brushStrength;


        //ObjectTracks .csでもテクスチャを生成してるから　今のままのスクリプトを共存させるとかち合う
        void Start() {
            _drawMaterial = new Material(_drawShader);    //このシェーダーをもとにマテリアルを作成　(絵を描くためのマテリアル)
            _drawMaterial.SetVector("_Color", Color.red);//シェーダーに色の設定

            _snowMaterial = GetComponent<MeshRenderer>().material;//GameObjectに　くっついてるマテリアルを操作

            _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);//新しいレンダーテクスチャを設定
            _snowMaterial.SetTexture("_DispTex", _splatmap);//凹ませるテクスチャを設定
        }


        void Update() {
            if (Input.GetKey(KeyCode.Mouse0)) {//マウスを押してるとき　
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit)) {

                    //押した位置を_drawMaterialに付けたshaderの_Coordinate(座標)変数に伝える
                    _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));//_hit.textureCoord = タップした位置の UV を取得
                    _drawMaterial.SetFloat("_Size", _brushSize);
                    _drawMaterial.SetFloat("_Strength", _brushStrength);


                    //Debug.Log($" U : {_hit.textureCoord.x} , V : {_hit.textureCoord.y}");

                    RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);//一時的なレンダーテクスチャ　凹ませるテクスチャのサイズ感で作成
                    Graphics.Blit(_splatmap, temp);               //凹み参照テクスチャ　を　tempに一時的に保持
                    Graphics.Blit(temp, _splatmap, _drawMaterial);//temp から　 凹み参照テクスチャ に _drawMaterialを反映させる(赤色に)
                    RenderTexture.ReleaseTemporary(temp);         //一時的なものをリリース
                }
            }
        }

        private void OnGUI()//毎フレーム
        {
            GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);   //毎フレーム　凹み参照テクスチャ を　uiとして表示
        }
    }
}