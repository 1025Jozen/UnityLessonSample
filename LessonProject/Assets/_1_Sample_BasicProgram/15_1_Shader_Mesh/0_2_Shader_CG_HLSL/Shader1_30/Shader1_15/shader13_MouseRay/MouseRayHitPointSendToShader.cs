
using UnityEngine;

namespace Shader_Sample
{
    /// <summary>マウスから出たRayとオブジェクトの衝突座標をShaderに渡す。アタッチされたオブジェクトで反応</summary>
    public class MouseRayHitPointSendToShader : MonoBehaviour
    {
        /// <summary> ポインターを出したいオブジェクトのレンダラー。前提：Shaderは座標受け取りに対応したものを適用しておく</summary>
        [SerializeField] private Renderer _renderer;

        /// <summary>Shader側で定義済みの座標を受け取る変数</summary>
        private string propName = "_MousePosition";

        private Material mat;//matに設定したシェーダーに情報を渡す

        void Start() {
            mat = _renderer.material;
        }

        void Update() {            
            if (Input.GetMouseButton(0)) {//マウスの入力                
                Ray ray             = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit_info = new RaycastHit();
                float max_distance  = 100f;
                bool is_hit         = Physics.Raycast(ray, out hit_info, max_distance);//Rayを飛ばす

                
                if (is_hit) {//Rayとオブジェクトが衝突したとき                    
                    Debug.Log(hit_info.point);              //衝突座標                    
                    mat.SetVector(propName, hit_info.point);//Shaderに座標情報を渡す
                }
            }
        }
    }
}