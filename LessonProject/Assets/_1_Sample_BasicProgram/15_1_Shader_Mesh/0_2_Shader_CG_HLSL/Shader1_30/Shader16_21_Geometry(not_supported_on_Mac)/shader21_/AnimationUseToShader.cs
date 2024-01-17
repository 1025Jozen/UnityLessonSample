using UnityEngine;

namespace Shader_Sample
{
    /// <summary>
    /// アニメーションで変化させた値をShaderで使ってみる
    /// </summary>
    [ExecuteAlways] //こいつ付けとけばEditorでプレビュー可能
    public class AnimationUseToShader : MonoBehaviour
    {
        // シェーダーで利用する値たち　C#側ではAnimatorで変化させる
        public float gravityValue;
        public float positionValue;
        public float rotationValue;
        public float scaleValue;

        /// <summary>
        /// ジオメトリシェーダーを適用したオブジェクトのレンダラー
        /// </summary>
        [SerializeField] private Renderer _renderer;

        // Shader側に用意した定義済みの値を受け取る変数たち
        //使ってない変数に警告が出るからコメントオフ  #if 使ってもいいけど
        //private string _gravityFactor  = "_GravityFactor";
        //private string _positionFactor = "_PositionFactor";
        //private string _rotationFactor = "_RotationFactor";
        //private string _scaleFactor    = "_ScaleFactor";

        private Material _mat;

        void Start() {
            //Editor上でマテリアルのインスタンスを作ろうとするとエラーが出るのでsharedMaterialを利用
            _mat = _renderer.sharedMaterial;
        }

        void Update() {
            //今回は手動でTriggerをオンにしてみて

            //Shaderに値を渡す //Macだとエラーが出るからコメントオフ　Windowsで試して
            //_mat.SetFloat(_gravityFactor, gravityValue);
            //_mat.SetFloat(_positionFactor, positionValue);
            //_mat.SetFloat(_rotationFactor, rotationValue);
            //_mat.SetFloat(_scaleFactor, scaleValue);
        }
    }
}