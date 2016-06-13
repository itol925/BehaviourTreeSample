using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlutterObject : MonoBehaviour {
	Transform m_target;
    Canvas m_canvas;
    Text txtBlood;
    float offsetY = 1;
    Vector3 offset = new Vector3(0, 1.25f, 0);

    void Awake() { 
        m_canvas = GameObject.Find("GameManager/Canvas").GetComponent<Canvas>();
        transform.SetParent(m_canvas.transform);
		txtBlood = transform.FindChild("Text").GetComponent<Text>();
    }

    void Start() {
        Destroy(gameObject, 1);
		//gameObject.SetActive (true);
    }

    // Update is called once per frame
	void Update(){
        if (m_target != null && m_canvas != null){
            if(offsetY < 100)
                offsetY++;
            
            txtBlood.transform.position = Camera.main.WorldToScreenPoint(m_target.position + offset + new Vector3(0, offsetY/100, 0));
        }
    }

    public void SetTarget(Transform target) { 
        m_target = target;
    }

    public void SetText(string text, Color c) {
		txtBlood.color = c;
        txtBlood.text = text;
    }
}