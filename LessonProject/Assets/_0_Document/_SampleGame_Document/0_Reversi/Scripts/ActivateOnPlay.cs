using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Othello {
    public class ActivateOnPlay : MonoBehaviour
    {
        void Start() {
            GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }
}

