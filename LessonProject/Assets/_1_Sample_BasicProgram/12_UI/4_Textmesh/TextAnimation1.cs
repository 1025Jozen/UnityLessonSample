using System.Collections;
using TMPro;
using UnityEngine;

namespace TextMesh_Sample {
    public class TextAnimation1 : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;

        void Start() {
            StartCoroutine(Simple());

            //StartCoroutine(FadeIn());
        }


        //1文字ずつ表示するだけの例
        private IEnumerator Simple() {
            // maxVisibleCharacters が 表示される文字数
            // 文字の表示数を0に(テキストが表示されなくなる)
            tmpText.maxVisibleCharacters = 0;

            
            for (var i = 0; i < tmpText.text.Length; i++) {// テキストの文字数分ループ
                yield return new WaitForSeconds(0.2f);// 一文字ごとに0.2秒待機        
                tmpText.maxVisibleCharacters = i + 1;// 文字の表示数を増やしていく
            }
        }


        private IEnumerator FadeIn() { 
            tmpText.ForceMeshUpdate(true);   // script上でテキストを更新したとしても、TMPのUI上での更新が終わっていない場合があるので再生成
            TMP_TextInfo textInfo = tmpText.textInfo;
            TMP_CharacterInfo[] charInfos = textInfo.characterInfo;
  
            for (var i = 0; i < charInfos.Length; i++) {
                SetTextAlpha(tmpText, i, 0);
                // 全ての文字を一度非表示にする
                // (特殊文字の兼ね合いで要素と文字の数が一致しない場合がある)
            }

            // charInfosの要素数分(文字数分)ループ
            for (var i = 0; i < charInfos.Length; i++) {                
                if (char.IsWhiteSpace(charInfos[i].character)) continue;// 空白または改行文字の場合は無視            
                yield return new WaitForSeconds(0.2f);// 一文字ごとに0.2秒待機
                byte alpha = 0;

                while (true) {         
                    yield return new WaitForFixedUpdate();   // FixedUpdateのタイミングまで待つ
                    alpha = (byte)Mathf.Min(alpha + 10, 255);
                    SetTextAlpha(tmpText, i, alpha);          // 一文字の不透明度を増加させていく(自作メソッド)           
                    if (alpha >= 255) break;                  // 不透明度が255を超えたら次の文字に移る
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
        //TextMeshProでは一文字ごとにMeshを操作できるので、それ利用し、頂点カラーでフェードインを行っている
    }
}

