using UnityEngine;
using UnityEngine.SceneManagement;

namespace Often {
    public class SceneReset : MonoBehaviour
    {

        [SerializeField] KeyCode _resetSceneKey = KeyCode.R;

        private void Update() {
            if (Input.GetKeyDown(_resetSceneKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

    }
}
