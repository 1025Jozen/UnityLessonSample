using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] Image bar;

        private void Start()
        {
            player.HpChanged += Player_HpChanged;
            bar.fillAmount = 1f;
        }

        private void Player_HpChanged(object sender, Player.HpEventArgs e)
        {
            bar.fillAmount = e.hpNormalized;
        }


    }
}

