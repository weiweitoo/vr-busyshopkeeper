using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Controls interactable teleporting objects in the Demo scene.</summary>
[RequireComponent(typeof(Collider))]
public class ObjectController : MonoBehaviour
{
    public Material inactiveMaterial;
    public Material gazedAtMaterial;
    public string objectName;
    public float defaultSpeed = 6f;


    private Vector3 startingPosition;
    private Renderer myRenderer;
    private Animator animatorComponent;
    private bool moving;
    private float currSpeed;

    private Vector3 destination;

    public void SetGazedAt(bool gazedAt)
    {
        if (inactiveMaterial != null && gazedAtMaterial != null)
        {
            myRenderer.material = gazedAt ? gazedAtMaterial : inactiveMaterial;
            SetShakingAnimation(gazedAt);
            return;
        }
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


        // Hold it and play animation
        // Debug.Log("Hold" + this.objectName);
        SetFloatAnimation(true);
        Vector3 currPos = transform.parent.transform.localPosition;
        Vector3 newPos = new Vector3(currPos.x, currPos.y + 0.9f, currPos.z);
        MoveTo(newPos, 1f);

        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetPlayerController().Hold(this.gameObject);
    }

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

    public void Trigger(Vector3 dest)
    {
        // If it is still moving upward, stop it from moving again
        if(moving == true){
            return;
        }

        // Debug.Log("Trigger Object" + this.objectName);
        MoveTo(dest, defaultSpeed);
        SetFloatAnimation(false);
    }

    private void Start()
    {
        startingPosition = transform.localPosition;
        myRenderer = GetComponent<Renderer>();
        animatorComponent = transform.parent.GetComponent<Animator>();
        SetGazedAt(false);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.parent.transform.localPosition, destination) < 0.1f){
            moving = false;
        }
        else if (moving)
        {
            Debug.Log("Still moving");
            MoveToUpdate(destination, currSpeed);
        }
        
        // stop moving if near
    }

    private void MoveToUpdate(Vector3 dest, float speed){
        float step = speed * Time.deltaTime;
        transform.parent.transform.localPosition = Vector3.MoveTowards(transform.parent.transform.localPosition, dest, step);
        Debug.Log(transform.parent.transform.localPosition);
        Debug.Log(step);
    }

    private void MoveTo(Vector3 dest, float speed){
        moving = true;
        destination = dest;
        currSpeed = speed;
    }
}
