using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerScript : MonoBehaviour
{
    enum State
    {
        Run,
        Waiting,
        Flying
    }
    public enum GoodsType
    {
        None,
        AppleWhite,
        AppleRed,
        Pear
    }

    public float movingSpeed;
    public string buyerName;
    public Transform buyerCentrePoint;
    public Vector3 destPoint;
    public float centerDistance;
    public float minDistance;
    public GoodsType buyerType;
    private Animator animatorComponent;
    private State currState;
    private bool moving;

    void Start()
    {
        moving = true;
        animatorComponent = GetComponentInParent<Animator>();
        currState = State.Run;
        animatorComponent.SetBool("Walk", true);
        UpdatebuyerCentrePoint();
    }

    void Update()
    {
        if (moving && Vector3.Distance(transform.parent.transform.localPosition, destPoint) < centerDistance)
        {
            moving = false;
            animatorComponent.SetBool("Walk", false);
            UpdatebuyerCentrePoint();
            StartCoroutine(MoveAfter(Random.Range(2.0f, 8.0f)));
        }
        else if (moving)
        {
            MoveToUpdate(destPoint, movingSpeed);
        }
    }

    public void SetBuyerProfile(Transform centrePoint, float movingSpeed)
    {
        this.buyerCentrePoint = centrePoint;
        this.movingSpeed = movingSpeed;
    }

    public bool Trigger(GoodsType goodsType)
    {
        if(goodsType == buyerType){
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().releaseSlot(transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
            return true;
        }
        else{
            return false;
        }
        
    }

    IEnumerator MoveAfter(float seconds){
        yield return new WaitForSeconds(seconds);
        LookDirection();
        animatorComponent.SetBool("Walk", true);
        moving = true;
    }

    private void LookDirection()
    {
        transform.LookAt(buyerCentrePoint);
    }

    private void MoveToUpdate(Vector3 dest, float speed)
    {
        float step = speed * Time.deltaTime;
        transform.parent.transform.localPosition = Vector3.MoveTowards(transform.parent.transform.localPosition, dest, step);
    }

    private void UpdatebuyerCentrePoint()
    {
        float distance = 0;
        float newX = 0;
        float newZ = 0;
        Vector3 newPos = new Vector3(0,0,0);
        while(distance <= minDistance){
            newX = buyerCentrePoint.localPosition.x + RandomOfRanges(-2.0f, -5.5f, 2.0f, 4.5f);
            newZ = buyerCentrePoint.localPosition.z + RandomOfRanges(-2.0f, -3.0f, 2.0f, 3.0f);
            newPos = new Vector3(newX, buyerCentrePoint.localPosition.y, newZ);
            distance = Vector3.Distance(newPos, destPoint);
        }
        destPoint = newPos;
    }

    private float RandomOfRanges(float r1start, float r1end, float r2start, float r2end){
        return randomBoolean() ? Random.Range(r1start, r1end) :Random.Range(r2start, r2end);
    }

    private bool randomBoolean()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }
}
