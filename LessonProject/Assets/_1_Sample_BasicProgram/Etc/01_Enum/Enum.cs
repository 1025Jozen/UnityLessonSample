using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enum_Sample {
    public class Enum : MonoBehaviour {
        

        [Flags]
        public enum Animal
        {
            [InspectorName("犬")]//enumには使える 通常の変数を日本語にするにはEditorを継承して〜
            Dog = 1 << 0, //0番目に1を設定 0001

            [InspectorName("猫")]
            Cat = 1 << 1, //1番目に1を設定 0010

            [InspectorName("熊")]
            Bear = 1 << 2 //2番目に1を設定 0100
        }        
        public Animal animalKind;//インスペクターでも複数選択できる


        [Flags]
        public enum CharacterStates {
            None = 0,      //    0
            Idle = 1,      //    1
            Walking = 2,   //   10
            Running = 4,   //  100
            Jumping = 8,   // 1000
            Crouching = 16,//10000
        }
        public CharacterStates currentState = CharacterStates.None;//0

        private void Start() {
            Animal myPets = Animal.Dog | Animal.Cat;  // 0011
            //Animal wildAnimals = Animal.Bear;//0100
            //Animal allAnimals = Animal.Dog | Animal.Cat | Animal.Bear;//0111

            // 特定の動物が含まれているかをチェック
            //論理積（AND演算）//myPets(0011) & Animal.Cat(0010)　両方が真のものを返す　 0010
            if ((myPets & Animal.Cat) == Animal.Cat) {
                Debug.Log("I have a cat!");
            }

            currentState = CharacterStates.Idle;//00001
        }

        private void Update() {
            // 状態の設定
            if (Input.GetKeyDown(KeyCode.A)) {
                //ビット演算子の足し算みたいなもの　論理和 1+1=1 1+0=0 0+0=0
                //二つの入力のいずか一方あるいは両方が1のとき出力が1
                currentState |= CharacterStates.Jumping; // Jumpingフラグを追加 01001
            }

            // 状態の削除
            if (Input.GetKeyDown(KeyCode.D)) {
                currentState &= ~CharacterStates.Jumping; // Jumpingフラグを削除 00001
            }

            // 特定のStateが含まれているかをチェック
            //論理積（AND演算）//currentState & CharacterStates.Running    　両方が真のものを返す　 00100   
            if ((currentState & CharacterStates.Running) != 0) {
                Debug.Log("Running state");
            } else if ((currentState & CharacterStates.Jumping) != 0) {// 両方が真のものを返す 01000
                Debug.Log("Jumping state");
            }

        }


        // ~    NOT 演算 (論理否定) 数値の各ビットに対して、1 は 0 にし、 0 は 1 にします。
        // |    OR 演算子                              1 であれば 1、 両方とも 0 の場合に 0
        // ^    XOR 演算子                             いずれかが 1 であれば 1、 両方とも同じ値の場合に 0
        // &    AND 演算子                             両方が 1 の場合に 1 とし、 それ以外は 0 

        //左シフト演算子は <<、 右シフト演算子は >>
        //変数  演算子  シフト数 (例: x << 3) の形式でシフトする数を指定し、ビット列を右または左にシフト
        //int x = 0b1010100;
        //int y = x << 3;
        //000001010100
        //001010100000
    }
}

