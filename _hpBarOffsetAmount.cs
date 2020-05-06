using UnityEngine;
using System.Collections;


public class _hpBarOffsetAmount : MonoBehaviour {

	// Use this for initialization
	public float _offset=0.0f; 	//0.0f - 0.5f, 0.0 is %100 healthy, 0.5f is %0 hp which is full red. 
	
	void Start () {
        if (this.networkView != null)
        {
            this.networkView.observed = this.GetComponent(typeof(_hpBarOffsetAmount));
        }
        else
        {
            //Debug.Log("not available hp bar network view");
        }
	}
	
	// Update is called once per frame
	void Update () {
		//error control
		if (_offset < 0){
			Debug.Log("_hpBarOffsetAmount clasindaki _offset degeri 0 dan kucuk olamazm 0 a esitliyorum");
			_offset=0.0f;
		}
		if (_offset > 0.5f){
			Debug.Log("_hpBarOffsetAmount clasindaki _offset degeri 0.5 den buyuk olamaz 0.5 e  a esitliyorum");
			_offset=0.5f;
		}
		renderer.material.mainTextureScale = new Vector2 (0.5f,1); // makes the texture scaled by 2
		renderer.material.mainTextureOffset = new Vector2 (_offset,0);
	}

    public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        
        if (stream.isWriting)
        {
            stream.Serialize(ref _offset);
        }
        else
        {
            stream.Serialize(ref _offset);
        }
    }
}
