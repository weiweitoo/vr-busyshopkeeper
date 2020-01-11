using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Controls interactable teleporting objects in the Demo scene.</summary>
[RequireComponent(typeof(Collider))]
public class ObjectController : MonoBehaviour
{
    public Mesh ObjectModel;
    // public Material inactiveMaterial;
    // public Material gazedAtMaterial;
    public string objectName;
    public float damage;
    public float defaultSpeed = 6f;


    private Vector3 startingPosition;
    private Renderer myRenderer;
    private Animator animatorComponent;
    private bool moving;
    private float currSpeed;

    private Vector3 destination;
    private PlayerController playerController;
    private Rigidbody rigidbody;
    private MeshCollider collider;
    private MeshFilter meshFilter;
    private bool isPlayerProjectile;

    
    private void Start()
    {
    }

    private void Update()
    {
        // stop moving if near
        if (moving && Vector3.Distance(transform.parent.transform.localPosition, destination) < 0.1f)
        {
            moving = false;
        }
        else if (moving)
        {
            MoveToUpdate(destination, currSpeed);
        }
    }

    public void setProfile(Mesh mesh, string objectName, float damage, bool isPlayerProjectile){
        this.ObjectModel = mesh;
        this.objectName = objectName;
        this.damage = damage;
        this.isPlayerProjectile = isPlayerProjectile;

        init();
    }

    public void SetGazedAt(bool gazedAt)
    {
        // if (inactiveMaterial != null && gazedAtMaterial != null)
        // {
        //     // myRenderer.material = gazedAt ? gazedAtMaterial : inactiveMaterial;
        // }
        SetShakingAnimation(gazedAt);
        return;
    }

    public void Recenter()
    {
        #if !UNITY_EDITOR
                GvrCardboardHelpers.Recenter();
        #else
                if (GvrEditorEmulator.Instance != null)
                {
                    GvrEditorEmulator.Instance.Recenter();
                }
        #endif  // !UNITY_EDITOR
    }

    public void Hold(BaseEventData eventData)
    {
        // Only trigger on left input button, which maps to
        // Daydream controller TouchPadButton and Trigger buttons.
        PointerEventData ped = eventData as PointerEventData;
        if (ped != null)
        {
            if (ped.button != PointerEventData.InputButton.Left)
            {
                return;
            }
        }

        // If do not hold anything, hold it then, else put it down
        if (playerController.GetHoldedObject() == null)
        {
            // Hold it and play animation
            SetFloatAnimation(true);
            Vector3 currPos = transform.parent.transform.localPosition;
            Vector3 newPos = new Vector3(currPos.x, currPos.y + 1.4f, currPos.z);
            MoveTo(newPos, 1f);
            rigidbody.useGravity = false;
            isPlayerProjectile = true;

            playerController.Hold(transform.parent.gameObject);
        }
        else
        {
        }
    }

    public void Release(){
        ResetAnimation();
        rigidbody.useGravity = true;
        playerController.Release();
    }

    
    public bool Trigger(Vector3 dest)
    {
        // If it is still moving upward, stop it from moving again
        if (moving == true)
        {   
            return false;
        }
        else if (transform.position == dest)
        {
            return false;
        }

        collider.isTrigger = true;
        MoveTo(dest, defaultSpeed);
        SetFloatAnimation(false);
        return true;
    }


    /*
    * Private method
    */

    private void SetShakingAnimation(bool play)
    {
        animatorComponent.SetBool("Shaking", play);
        // animatorComponent.SetBool("Floating", !play);
    }

    private void SetFloatAnimation(bool play)
    {
        animatorComponent.SetBool("Floating", play);
        // animatorComponent.SetBool("Shaking", !play);
    }

    private void ResetAnimation()
    {
        animatorComponent.SetBool("Floating", false);
        animatorComponent.SetBool("Shaking", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().Trigger(damage);
            Destroy(gameObject);
        }
        // not player projectile and hit player
        else if(!isPlayerProjectile && other.tag == "Player"){
            Debug.Log("Deal damage to player");
        }
    }

    private void MoveToUpdate(Vector3 dest, float speed)
    {
        float step = speed * Time.deltaTime;
        transform.parent.transform.localPosition = Vector3.MoveTowards(transform.parent.transform.localPosition, dest, step);
    }

    private void MoveTo(Vector3 dest, float speed)
    {
        moving = true;
        destination = dest;
        currSpeed = speed;
    }

    private void init(){
        myRenderer = GetComponent<Renderer>();
        animatorComponent = transform.parent.GetComponent<Animator>();
        rigidbody = transform.parent.GetComponent<Rigidbody>();
        collider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        playerController = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetPlayerController();
        SetGazedAt(false);
        startingPosition = transform.localPosition;
        meshFilter.mesh = ObjectModel;
        collider.sharedMesh = ObjectModel;
    }

}
