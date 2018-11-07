using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nViewTest : MonoBehaviour {

    NetworkView nView;


    [RPC]//AttributeはRPC
    void AnyMetho(int num,string chara)//引数を入れることも可能
    {
        //中身はご自由に
        Debug.Log("RPCテスト成功 : " + num + chara);

    }


    // Use this for initialization
    void Start () {
        nView = GetComponent<NetworkView>();
        nView.RPC("AnyMetho", RPCMode.Others, 1,"てすと"); //第一引数に関数名　第２引数に※RPCモード　第三引数以降に実際に動く関数の引数を入れていく
	}                                                      //※RPCモード　 All→自分含めた全員に行く？　others→自分以外の全員に行く
	
	// Update is called once per frame
	void Update () {
		
	}
}
