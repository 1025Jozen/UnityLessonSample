using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace WarpAttack
{
    public class WarpAttack : MonoBehaviour
    {
        private Animator anim;
        [SerializeField] CinemachineFreeLook cameraFreeLook;//
        [SerializeField] Transform target;                  //目標
        [SerializeField] float warpDuration = 0.5f;         //

        private Volume postProcessingVolume;//カメラのPostProcessを有効にして「Volume」をアタッチ
        private VolumeProfile postProfile;//PostProcessに色々追加(Add Override)してここからそれぞれを操作
        //private Bloom bloomEffect;//Bloomを操作する場合

        private CinemachineImpulseSource impulse;

        [Space]

        //武器関係
        [SerializeField] Transform sword;      //剣
        [SerializeField] Transform swordSocket;//
        private Vector3 swordOrigRot;          //元々の回転角度
        private Vector3 swordOrigPos;          //
        private MeshRenderer swordMesh;

        [Space]

        public Material glowMaterial;          //輝くマテリアル

        [Space]

        [Header("Particles")]
        public ParticleSystem blueTrail;       
        public ParticleSystem whiteTrail;      
        public ParticleSystem swordParticle;

        [Space]

        [Header("Prefabs")]
        public GameObject hitParticle;


        /*            */
        private void Start()
        {
            anim = GetComponent<Animator>();

            swordOrigRot = sword.localEulerAngles;
            swordOrigPos = sword.localPosition;
            swordMesh    = sword.GetComponentInChildren<MeshRenderer>();
            sword.gameObject.SetActive(false);//剣を出して

            postProcessingVolume = Camera.main.GetComponent<Volume>();
            postProfile = postProcessingVolume.profile;


            impulse = GetComponent<CinemachineImpulseSource>();
        }




        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) {

                //RotateTowards(target);//ターゲットの方向を向ける
                //canMove = false;//ワープ中は操作効かなくする

                swordParticle.Play();
                sword.gameObject.SetActive(true);//剣を表示
                anim.SetTrigger("WarpAttack");
                //攻撃アニメーションの途中でAttack関数を呼び出す
                //スクリプトもしくはanimationのトリガーで実装
            }
        }

        public void Attack() {
            //残影を残す
            GameObject clone = Instantiate(this.gameObject, transform.position, transform.rotation);
            Destroy(clone.GetComponent<Animator>());
            Destroy(clone.GetComponent<WarpAttack>());
            Destroy(clone.GetComponent<CharacterController>());
            Destroy(clone.GetComponent<WarpAttack>().sword.gameObject);

            //Destroy(clone.GetComponent<WarpAttack>().sword.gameObject);
            //Destroy(clone.GetComponent<WarpAttack>());
            //Destroy(clone.GetComponent<CharacterController>());

            //マテリアルを変更
            SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList) {
                smr.material = glowMaterial;
                smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(() => Destroy(clone));//閾値まで数値を上げて消えたらObjectを消す
            }


            ShowBody(false);//体を消す
            anim.speed = 0;//アニメーションを一時停止して

            //剣の位置・手につけたSocketの位置・Playerの位置などから補正した位置にPlayerを動かした方がいいかも
            //例：
            //アニメーションをストップさせてた時のsocket位置を取得
            //soket位置とPLayerの位置の差を把握
            //差の分ずらして移動させるなど
            //
            //ワープ移動。移動後、ワープを終わらした時の処理を実行
            transform.DOMove(target.position, warpDuration)
                .SetEase(Ease.InExpo)
                .OnComplete(() => FinishWarp());

            //ワープ移動時、Particles を再生
            blueTrail.Play();
            whiteTrail.Play();

            //剣の移動
            sword.parent   = null;//親（プレイヤーや武器ソケット）との関係を切る     
            sword.DOMove(target.position , warpDuration / 2f);//プレイヤーより剣を早めにターゲットに向かわせる
        }

        //ワープ終了後
        void FinishWarp() {
            ShowBody(true);　　　　　　　　　　　　//体を表す

            sword.parent           = swordSocket; //親子関係に戻って
            sword.localPosition    = swordOrigPos;//初期位置に
            sword.localEulerAngles = swordOrigRot;

            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList) {
                GlowAmount(30);
                DOVirtual.Float(30, 0, .5f, GlowAmount);
            }

            //パーティクルを発生させて　　　　当たった時などの条件分岐は今回つけない
            Instantiate(hitParticle, sword.position, Quaternion.identity);

            //敵のアニメーション
            //target.GetComponentInParent<Animator>().SetTrigger("hit");
            //ノックバック
            //target.parent.DOMove(target.position + transform.forward, .5f);


            StartCoroutine(HideSword());      
            StartCoroutine(PlayAnimation());//アニメーション再生
            StartCoroutine(StopParticles());



            //いらない装飾　コメントアウトでいい

            //カメラを揺らす
            //Cinemachine Collision Impulse Sourceをアタッチ　cinemachineにはimpuseをadd
            impulse.GenerateImpulse(Vector3.right);

            //Lens Distortion
            DOVirtual.Float(1, 0, 0.2f, DistortionAmount);//intensity.
            DOVirtual.Float(2f, 1, 0.1f, ScaleAmount);//scale
        }




        IEnumerator PlayAnimation() {
            yield return new WaitForSeconds(.2f);
            anim.speed = 1;
        }

        IEnumerator StopParticles() {
            yield return new WaitForSeconds(.2f);
            blueTrail.Stop();
            whiteTrail.Stop();
        }

        IEnumerator HideSword() {
            yield return new WaitForSeconds(.8f);
            swordParticle.Play();

            GameObject swordClone = Instantiate(sword.gameObject, sword.position, sword.rotation);

            sword.gameObject.SetActive(false);

            MeshRenderer swordMR = swordClone.GetComponentInChildren<MeshRenderer>();
            Material[] materials = swordMR.materials;

            for (int i = 0; i < materials.Length; i++) {
                Material m = glowMaterial;
                materials[i] = m;
            }

            swordMR.materials = materials;

            for (int i = 0; i < swordMR.materials.Length; i++) {
                swordMR.materials[i].DOFloat(1, "_AlphaThreshold", .3f).OnComplete(() => Destroy(swordClone));
            }

        }





        //体を消す
        void ShowBody(bool state) {
            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList) {
                smr.enabled = state;
            }
        }

        void GlowAmount(float x) {
            SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList) {
                smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
            }
        }

        private LensDistortion lensDistortionEffect;
        ///
        void DistortionAmount(float x) {
            postProfile.TryGet(out lensDistortionEffect);
            lensDistortionEffect.intensity.value = x;
        }
        void ScaleAmount(float x) {
            postProfile.TryGet(out lensDistortionEffect);
            lensDistortionEffect.scale.value = x;
        }
    }
}