using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample5 : MonoBehaviour
    {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                OnUnloadScene();
            }
        }

        public void OnUnloadScene() {
            StartCoroutine(CoUnload());
        }

        IEnumerator CoUnload() {
            //Scene_Sample1をアンロード    *****不使用アセットのアンロードは行われません。
            var op = SceneManager.UnloadSceneAsync("Scene_Sample1");
            yield return op;

            //アンロード後の処理を書く

            //必要に応じて不使用アセットをアンロードしてメモリを解放する
            //けっこう重い処理なので、別に管理するのも手
            yield return Resources.UnloadUnusedAssets();

            //Application.lowMemory　といった　　メモリが少なくなった段階で呼ばれるイベントを使うなどもいいかも

            //不使用アセットのアンロードをしたつもりで、完全にアンロードできていないということがある

            //例えば下記プログラムのように
            //SampleAをDestroyしても、Bに参照が残っている限りは、
            //Aは消えず参照も残り、不使用状態にならないため
            //Resources.UnloadUnusedAssets()の対象にならない
            //public class SampleA : MonoBehaviour
            //{
            //    //インスペクター上でテクスチャを参照している
            //    public Texture texture;
            //}
            //public class SampleB : MonoBehaviour
            //{
            //    //インスペクター上でAを参照している
            //    public SampleA sampleA;
            //}

            //Destroy(gameObject);などでも同様のことが起こるので
            //なるべくDestroyしたものへの参照を持つようにはしないようにする
            //リークを解消するには、nullを代入して参照を消すことsampleA = null;


            //下記のようなプログラムで　SampleAをDestory　するのは問題ない
            //public class SampleA : MonoBehaviour
            //{
            //    //Bを参照している
            //    public SampleB sampleB;
            //}
            //public class SampleB : MonoBehaviour
            //{
            //    //Aを参照してはいない
            //}
        }
    }
}

