using UnityEngine;
namespace Slope
{
    [RequireComponent(typeof(CharacterController))]
    public class Slope : MonoBehaviour
    {
        public float moveSpeed      = 5f;
        [SerializeField]float rotateSpeed    = 30f;
        public float jumpForce      = 8f;
        public float gravity        = 20f;
        public float slopeForce     = 5f;
        public float slopeRayLength = 2f;

        [SerializeField] LayerMask slopelayerMask;
        [SerializeField] LayerMask groundlayerMask;

        private CharacterController controller;
        private Vector3 moveDirection = Vector3.zero;

        void Start() {
            controller = GetComponent<CharacterController>();
        }

        void Update() {

            bool rayhit = Physics.Raycast(transform.position, -transform.up, out RaycastHit hitground, slopeRayLength, groundlayerMask);
            //Debug.Log(rayhit);

            Debug.DrawRay(
                transform.position ,
                transform.position + transform.up * 0.5f - transform.up * slopeRayLength,
                Color.green
            );

            if (rayhit) {

                Debug.Log("移動");
                
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput);
                moveDirection = transform.TransformDirection(inputDirection);
                moveDirection *= moveSpeed;


                if (Input.GetButtonDown("Jump")) {
                    moveDirection.y = jumpForce;
                }

                // Check for slopes and adjust movement
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRayLength, slopelayerMask)) {
                    Debug.Log("坂道");
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle > controller.slopeLimit) {
                        //坂道方向のベクトル
                        moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized * moveSpeed;

                        //入力に対応する形でしないと勝手に動く

                        moveDirection.y -= gravity * 10 * Time.deltaTime;
                        //坂道で止まってる時gravityをかけない



                  
                    }

                    if (Input.GetButtonDown("Jump")) {
                        moveDirection.y = jumpForce;
                    }
                }

            } else {
                // Apply gravity
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Debug.Log(moveDirection);
           
            // Move the character
            controller.Move(moveDirection * Time.deltaTime);

            // Rotate the character based on input
            transform.Rotate(Vector3.up * rotateSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        }
    }
}