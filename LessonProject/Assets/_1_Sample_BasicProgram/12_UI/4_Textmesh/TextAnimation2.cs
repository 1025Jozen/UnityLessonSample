using System.Collections;
using TMPro;
using UnityEngine;
namespace TextMesh_Sample
{
    public class TextAnimation2 : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;

        [SerializeField, Tooltip("1秒間における不透明度の増加量")]
        private float FadeSpeed = 2.0f;

        private Coroutine animationCoroutine;

        public void Restart() {
            Run();
        }

        private void Start() {
            Run();
        }

        private void Run() {
            if (animationCoroutine != null) {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(Seamless());
        }

        private IEnumerator Seamless() {
            tmpText.ForceMeshUpdate(true);// script上でテキストを更新した場合、TMPの更新が終わっていない場合があるので再生成
            TMP_TextInfo textInfo = tmpText.textInfo;
            TMP_CharacterInfo[] charInfos = textInfo.characterInfo;
       
            for (var i = 0; i < charInfos.Length; i++) {
                SetTextAlpha(tmpText, i, 0);// 全ての文字を一度非表示にする(特殊文字の兼ね合いで要素と文字の数が一致しない場合がある)
            }

            
            for (var i = 0; i < charInfos.Length; i++) {// charInfosの要素数分ループ
                if (char.IsWhiteSpace(charInfos[i].character)) continue;// 空白または改行文字の場合は無視

                yield return new WaitForSeconds(0.2f);// 一文字ごとに0.2秒待機
                float alpha = 0.0f;

                while (true) {
                    
                    yield return new WaitForFixedUpdate();// FixedUpdateのタイミングまで待つ
                    float alphaDelta = FadeSpeed * Time.fixedDeltaTime;//一定の割合で数を増やす
                    alpha = Mathf.Min(alpha + alphaDelta  , 1.0f);// 小さい方
                    SetTextAlpha(tmpText, i, (byte)(255 * alpha));// 一文字の不透明度を増加させていく

                    
                    if (alpha >= 1.0f) break;// 不透明度が1.0を超えたら次の文字に移る
                }

            }
        }

        // charIndexで指定した文字の透明度を変更
        private void SetTextAlpha(TMP_Text text, int charIndex, byte alpha) {     
            TMP_TextInfo textInfo      = text.textInfo;
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];// charIndex番目の文字のデータ構造体を取得
            TMP_MeshInfo meshInfo      = textInfo.meshInfo[charInfo.materialReferenceIndex];// 文字を構成するメッシュ(矩形)を取得

            var rectVerticesNum = 4;// 矩形なので4頂点
            for (var i = 0; i < rectVerticesNum; ++i) {
                meshInfo.colors32[charInfo.vertexIndex + i].a = alpha;// 一文字を構成する矩形の頂点の透明度を変更
            }    
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);// 頂点カラーを変更したことを通知
        }
    }
}