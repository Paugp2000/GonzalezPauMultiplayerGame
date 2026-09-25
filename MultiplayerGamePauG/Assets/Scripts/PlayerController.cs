using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider collider;
    private AudioSource sonidoSalto;
    public float speed = 5f;
    public Vector2 inputDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>(); 
        sonidoSalto = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        float h = 0f;
        float v = 0f;
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v =  1f;  
        }
        Vector2 input = new Vector2(h, v).normalized;
        SubmitInputServerRPC(input);
        if (Keyboard.current.spaceKey.isPressed)
        {
            SubmitJumpServerRPC();
        }
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;

        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);

        Vector3 targetVelocity = moveDirection * speed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }

    }
    [ServerRpc]
    public void SubmitInputServerRPC(Vector2 input)
    {
        inputDirection = input; 
    }

    [ServerRpc]
    public void SubmitJumpServerRPC()
    {
         rb.linearVelocity += new Vector3(0, 1f, 0);
         Debug.Log("He saltado");
         sonidoSalto.Play();
    }
}

