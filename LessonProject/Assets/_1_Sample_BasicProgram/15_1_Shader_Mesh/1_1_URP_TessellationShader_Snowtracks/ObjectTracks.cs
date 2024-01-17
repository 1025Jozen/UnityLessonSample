using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shader_Sample
{
    public class ObjectTracks : MonoBehaviour
    {
        Rigidbody rb;

        private RenderTexture _splatmap;    //このテクスチャをもとに凹ませる
        [SerializeField] Shader _drawShader;//凹ませるための絵を描いてくれるシェーダー     
        private Material _snowMaterial;//地形のマテリアル　
        private Material _drawMaterial;//絵を描くためのマテリアル
        [SerializeField] GameObject _terrain;
        [SerializeField] Transform[] _wheel;
        private RaycastHit _groundHit;
        int _layerMask;

        [SerializeField, Range(1, 500)] float _brushSize;
        [SerializeField, Range(0, 1)]   float _brushStrength;

        void Start() {
            rb = GetComponent<Rigidbody>();

            _layerMask = LayerMask.GetMask("Ground");
            _drawMaterial = new Material(_drawShader);   //このシェーダーをもとに絵を描くためのマテリアルを作成
            _snowMaterial = _terrain.GetComponent<MeshRenderer>().material;//地形に　くっついてるマテリアルを操作
            _snowMaterial.SetTexture("_DispTex", _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));//新しいレンダーテクスチャ(凹ませるテクスチャを後で描く)を設定
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") >  0) {
                rb.velocity = new Vector3 ( 1,0,0);
            }
            if (Input.GetAxis("Horizontal") < 0) {
                rb.velocity = new Vector3( -1, 0, 0);
            }
            if (Input.GetAxis("Vertical") > 0) {
                rb.velocity = new Vector3(0, 0, 1);
            }
            if (Input.GetAxis("Vertical") < 0) {
                rb.velocity = new Vector3(0, 0, -1);
            }

            for (int i = 0; i < _wheel.Length ; i++) {
                if (Physics.Raycast(_wheel[i].position , - Vector3.up , out _groundHit , 3f, _layerMask)) {
                    //Debug.Log("地面");
                    //押した位置を_drawMaterialに付けたshaderのCoordinate変数に伝える
                    _drawMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));//_hit.textureCoord = タップした位置の UV を取得
                    _drawMaterial.SetFloat("_Size", _brushSize);
                    _drawMaterial.SetFloat("_Strength", _brushStrength);

                    RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);//一時的なレンダーテクスチャ　凹ませるテクスチャのサイズ感で作成
                    Graphics.Blit(_splatmap, temp);               //凹み参照テクスチャ　を　tempに一時的に保持
                    Graphics.Blit(temp, _splatmap, _drawMaterial);//temp から　 凹み参照テクスチャ に _drawMaterialを反映させる(赤色に)
                    RenderTexture.ReleaseTemporary(temp);         //一時的なものをリリース
                }
            }
        }
    }
}