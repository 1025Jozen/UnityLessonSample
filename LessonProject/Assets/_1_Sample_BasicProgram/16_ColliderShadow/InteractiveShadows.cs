using UnityEngine;
using System.Linq;


namespace Shadow
{
    public class InteractiveShadows : MonoBehaviour
    {        
        [SerializeField] private Transform shadowTransform;//影のコライダーをつける場所       
        [SerializeField] private Transform lightTransform; //光
        private LightType lightType;                       //光の種類
        [SerializeField] private LayerMask targetLayerMask;//指定したレイヤーに影コライダーをつける      
        [SerializeField] private Vector3 extrusionDirection = Vector3.zero;//影コライダーを押し出す方向        
        private Vector3[] objectVertices;　　//頂点        
        private Mesh shadowColliderMesh;    //影由来のメッシュ・影の見た目
        private MeshCollider shadowCollider;//影にコライダーをつけて、「ここに当たったら〜」 

        //直前情報を保持　現在値と誤差が生まれたら　変化が　しておく
        private Vector3    previousPosition;
        private Quaternion previousRotation;
        private Vector3    previousScale;

        private bool canUpdateCollider = true;                                            //コライダーを変更できる状態
        [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f;


        /*          */

        private void Awake() {
            InitializeShadowCollider();//影由来のコライダーを表示させるGameObjectに MeshColliderコンポーネントをつけておく

            lightType          = lightTransform.GetComponent<Light>().type;//光のタイプ
            objectVertices     = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();//Objectのメッシュの頂点配列取得(重複なし)
            shadowColliderMesh = new Mesh();                               //メッシュをインスタンス化 変数にアドレス設定 
        }

        private void Update() {
            shadowTransform.position = transform.position;//影のオブジェクトの位置を　影のもとに合わせる
        }
        private void FixedUpdate() {
            if (TransformHasChanged() && canUpdateCollider) {//変化が生じたら
                Invoke("UpdateShadowCollider", shadowColliderUpdateTime);//(第２引数の)時間が経過した後、(第１引数の)関数(影からコライダーを作成)が実行される。同期処理
                canUpdateCollider = false;
            }
            previousPosition = transform.position;  //直前情報を保持
            previousRotation = transform.rotation;  
            previousScale    = transform.localScale;
        }

        /*          */

        private void InitializeShadowCollider() {//該当GameObjectにMeshColliderコンポーネントをつける          
            GameObject shadowGameObject = shadowTransform.gameObject;//影由来のコライダーをつけるオブジェクト

            //Hierarchyビューに表示されなくなる
            //shadowGameObject.hideFlags = HideFlags.HideInHierarchy; //OPTIONNAL

            shadowCollider           = shadowGameObject.AddComponent<MeshCollider>();//メッシュコライダーコンポーネントを追加            
            shadowCollider.convex    = true;//メッシュコライダーコンポーネントのcenvexをtrue。meshの外側を囲むように当たり判定範囲を自動でつける
            shadowCollider.isTrigger = true;//コライダーのトリガーをオンに　
        }

        /*          */

        private bool TransformHasChanged() {//影のもとになっているObjectの 変化を検知したらtrue
            return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale;
        }

        /*          */

        private void UpdateShadowCollider() {//影由来のコライダーを作成            
            shadowColliderMesh.vertices = ComputeShadowColliderMeshVertices();//影由来の立体的なコライダーを作成するための頂点を生成　　頂点配列
            shadowCollider.sharedMesh   = shadowColliderMesh;//メッシュを対応させる
            canUpdateCollider = true;
        }
        
        private Vector3[] ComputeShadowColliderMeshVertices() {//コライダーを作成するための頂点情報
            Vector3[] points         = new Vector3[objectVertices.Length * 2];//オブジェクトの頂点数*2(光の当たった点　と　光の当たった点から押し出した先のポイント　の　頂点座標を保持するための配列)　　　　　
            Vector3 raycastDirection = lightTransform.forward;                //光の前方向　　
            int n = objectVertices.Length;                                    //オブジェクトの頂点の数
            for (int i = 0; i < n; i++) {
                Vector3 point = transform.TransformPoint(objectVertices[i]);  //頂点座標を ワールド座標系での座標　に変換
                if (lightType != LightType.Directional) {                     //高原が直線出ないなら
                    raycastDirection = point - lightTransform.position;       //光源からオブジェクトの頂点へのベクトル
                }               
                points[i]     = ComputeIntersectionPoint(point, raycastDirection);//光の当たった点    ( オブジェクトの頂点　　と　光源からオブジェクトのポイント方向から  何かにぶつかるまでの交点を求める)
                points[n + i] = ComputeExtrusionPoint(point, points[i]);          //光の当たった点から押し出した先のポイント
            }
            return points;
        }

        
        private Vector3 ComputeIntersectionPoint(Vector3 fromPosition, Vector3 direction) {//スタート位置　光のベクトル　から　当たったオブジェクトでの交点を求める
            RaycastHit hit;//変数用意            
            if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, targetLayerMask)) {//レイを　影のできるところまで(光の当たったオブジェクト)　飛ばす　
                return hit.point - transform.position;//光の当たったところと　オブジェクトまでのベクトル
            }
            return fromPosition + 100 * direction - transform.position;//光がオブジェクトに当たらなかったら　ポジションから　100のところに座標をとる
        }
        
        private Vector3 ComputeExtrusionPoint(Vector3 objectVertexPosition, Vector3 shadowPointPosition) {//交点から押し出す方向を求める            
            if (extrusionDirection.sqrMagnitude == 0) {//(影コライダーを押し出すベクトルの)大きさが０の時　                
                return objectVertexPosition - transform.position;//オブジェクトから　　オブジェクトの頂点ポジ　までのベクトル
            }       
            return shadowPointPosition + extrusionDirection;//影の場所　から　コライダーを作成するための頂点(押し出す方向)
        }
    }
}