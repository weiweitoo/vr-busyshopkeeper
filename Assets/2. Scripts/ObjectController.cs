using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Controls interactable teleporting objects in the Demo scene.</summary>
[RequireComponent(typeof(Collider))]
// [RequireComponent(typeof(AudioSource))]
public class ObjectController : MonoBehaviour
{
    public Mesh ObjectModel;
    public BuyerScript.GoodsType goodsType;
    public float damage;
    public float defaultSpeed = 6f;
    public bool moving;
    public Transform playerLocation;

    [Header("Audio Setting")]
    public AudioClip buyerRageAudio;
    public AudioClip buyerHappyAudio;
    public AudioClip SmashPlayerAudio;


    private AudioSource audioSourceComponent;
    private Vector3 startingPosition;
    private Renderer myRenderer;
    private Animator animatorComponent;
    private float currSpeed;
    private Vector3 destination;
    private PlayerController playerController;
    private Rigidbody rigidbody;
    private MeshCollider collider;
    private MeshFilter meshFilter;
    private bool isPlayerProjectile;
    private bool destroyThis;


    private void Update()
    {
        // stop moving if near
        if (moving && Vector3.Distance(transform.parent.transform.position, destination) < 0.1f)
        {
            moving = false;
        }
        else if (moving)
        {
            MoveToUpdate(destination, currSpeed);
        }
        CheckIfDestroy();
    }

    public void setProfile(Transform playerLocation, Mesh mesh, BuyerScript.GoodsType type, float damage, bool isPlayerProjectile){
        this.goodsType = type;
        this.playerLocation = playerLocation;
        this.ObjectModel = mesh;
        this.damage = damage;
        this.isPlayerProjectile = isPlayerProjectile;

        init();
    }

    public void SetGazedAt(bool gazedAt)
    {
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

        // If do not hold anything, hold it, else do nothing
        if (playerController.GetHoldedObject() == null)
        {
            // If this projectile is on ground(player projectile), make it fly up
            if(isPlayerProjectile){
                Vector3 currPos = transform.parent.transform.localPosition;
                Vector3 newPos = new Vector3(currPos.x, currPos.y + 1.4f, currPos.z);
                MoveTo(newPos, 1f);
            }
            else{
                Vector3 currPos = transform.parent.transform.localPosition;
                Vector3 newPos = new Vector3(currPos.x, currPos.y + 0.7f, currPos.z);
                MoveTo(newPos, 1f);
            }

            // Hold it and play animation
            SetFloatAnimation(true);
            rigidbody.useGravity = false;
            isPlayerProjectile = true;
            playerController.Hold(transform.parent.gameObject);
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

        Shoot(dest, defaultSpeed);
        // DrawRayCast(dest);
        return true;
    }

    

    public void Shoot(Vector3 dest, float movingSpeed){
        MoveTo(dest, movingSpeed);
        // SetFloatAnimation(true);
        ResetAnimation();
        collider.isTrigger = true;
        rigidbody.useGravity = false;
        transform.LookAt(dest);
    
        // Add a roatation effect
        rigidbody.AddTorque(new Vector3(0f, 1f, 1f) * 200f);
    }

    /* 
    * Private method
    */ 
    // private void DrawRayCast(Vector3 dest){
        // Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        // Debug.DrawRay(transform.parent.position, forward, Color.red);
        // if (Physics.Linecast(transform.position, dest))
        // {
        //     Debug.Log("blocked");
        // }
    // }

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
        if (isPlayerProjectile && other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().Trigger(damage);
            SelfDestroy();
        }
        // not player projectile and hit player
        else if(!isPlayerProjectile && other.tag == "Player"){
            other.GetComponent<PlayerLifeScript>().Trigger(damage);
            SelfDestroy();
        }
        else if(isPlayerProjectile && other.tag == "Buyer"){
            if(other.GetComponent<BuyerScript>().Trigger(goodsType)){
                PlayAudioAndDestroy(buyerHappyAudio);
            }
            else{             
                PlayAudio(buyerRageAudio);
                
                // playerLocation       
                this.damage = 4;
                isPlayerProjectile = false;
                Shoot(playerLocation.position, 12);
            }
        }
    }
    
    private void CheckIfDestroy(){
        // Destroy if allow(sound not playing)
        if(destroyThis && !audioSourceComponent.isPlaying){
            Destroy(transform.parent.gameObject);
        }
    }

    private void PlayAudio(AudioClip audio){
        audioSourceComponent.clip = audio;
        audioSourceComponent.Play();
    }

    private void PlayAudioAndDestroy(AudioClip audio){
        // Hide the mesh renderer and collider and play sound
        // In the end destroy the object
        myRenderer.enabled = false;
        collider.enabled = false;
        PlayAudio(audio);
        StartCoroutine(SelfDestroyAfterSoundCoroutine());
    }

    private void SelfDestroy(){
        destroyThis = true;
        // Destroy(transform.parent.gameObject);
    }

    private IEnumerator SelfDestroyAfterSoundCoroutine(){
        float waitFor = audioSourceComponent.clip.length;
        yield return new WaitForSecondsRealtime(waitFor);
        // SelfDestroy();
        destroyThis = true;
        yield return null;
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

        audioSourceComponent = GetComponent<AudioSource>();
        collider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        playerController = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetPlayerController();
        SetGazedAt(false);
        startingPosition = transform.localPosition;
        meshFilter.mesh = ObjectModel;
        collider.sharedMesh = ObjectModel;
    }

}
