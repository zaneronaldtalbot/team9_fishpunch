using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickActor : MonoBehaviour
{
    private KartActor2 kart;
  //  private Transform kartTransform;
    public float spinOutSpeed = 10.0f;
    public float spinOutTime = 3.0f;
    private bool hitSlick = false;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
            if (hitSlick)
            {
          

              //  StartCoroutine(SpinOut());

            }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponent<KartActor2>();
            //kartTransform = coll.gameObject.GetComponent<Transform>();
            //hitSlick = true;
        }

    }
    //IEnumerator SpinOut()
    //{
    //    var time = 0f;
    //    var spinIncrement = new Vector3(0, (kart.turnValue * spinOutSpeed) * Time.deltaTime, 0);
        
    //    while(time < spinOutTime)
    //    {
    //        time += Time.deltaTime;
    //        kartTransform.Rotate(spinIncrement);

    //        hitSlick = false;
    //        yield return null;
            
    //    }

    //    yield return null;
    //}
}
