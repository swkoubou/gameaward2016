﻿using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;

public class halfGoal : MonoBehaviour {
    public GameObject RainDrop;
    private Text count;
    private Button @return;
    void Start() {
        //CountGoal();
        GoalRain( );
        @return = GameObject.Find("UI/Return").GetComponent<Button>( ); //リターンカウント

    }
    void CountGoal() {
        var goal = this.OnTriggerEnterAsObservable( )
            .Where(goaltag => goaltag.tag == "rain");
        goal
            .Buffer(4)
            .FirstOrDefault( )
            .Subscribe(_ => {
                @return.interactable = true; //反転できるように
            }).AddTo(this.gameObject);
    }

   
    void GoalRain()
    {
        var goal = this.OnTriggerEnterAsObservable()
            .Where(goaltag => goaltag.tag == "rain")
            .Subscribe(goaltag => {
                StartCoroutine("destroyobj",goaltag.gameObject);
                CountGoal( );
            });
    }

    IEnumerator destroyobj(GameObject destroy) {
        yield return new WaitForSeconds(0.1f);
        this.UpdateAsObservable( )
            .Subscribe(_ => {
                var time = Time.deltaTime;
                destroy.transform.localScale -= new Vector3(time , time , time);
                

                if ( destroy.transform.localScale.y < 0 ) {
                    Destroy(destroy);
                    float n = 0.4f;
                    var createPos = this.gameObject.transform;

                    if ( this.gameObject.name.Contains("up") ) {
                    }
                    if ( this.gameObject.name.Contains("down") ) {
                        n = -n;
                    }
                    InstancedummyObj(createPos , n);
                }
            })
            .AddTo(destroy);
    }
    //プールにオブジェクトを生成する
    void InstancedummyObj(Transform createPos,float n) {
        
        Vector3 inspos = new Vector3(createPos.position.x , createPos.position.y + n , createPos.position.z);

        var dummy = (GameObject)Instantiate(RainDrop , inspos , Quaternion.identity);
        dummy.tag = "otherrain";
        var rigidbody = dummy.GetComponent<Rigidbody>( );
        float random = Random.Range(-17f , 7f);
        rigidbody.AddForce(random*10 , 0 , 0 , ForceMode.Force);
        rigidbody.mass = 0.1f;
    }
}
