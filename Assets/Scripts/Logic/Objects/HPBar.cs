using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class HPBar : MonoBehaviour {    
	// ------------------------ 血条 -----------------------    
	Transform m_target;
    Vector3 offset = new Vector3(0, 1.25f, 0);

    Image m_imageBlack;
    Image m_imageRed;
    Canvas m_canvas;
    public Text m_text;

    void Awake() { 
        m_canvas = GameObject.Find("GameManager/Canvas").GetComponent<Canvas>();
        m_imageRed = transform.FindChild("ImageR").GetComponent<Image>();
        m_imageBlack = transform.FindChild("ImageB").GetComponent<Image>();
        m_text = transform.FindChild("Text").GetComponent<Text>();
        transform.SetParent(m_canvas.transform);
    }

    void Start() { 
        m_text.text = "";
    }

    void Update(){
        if (m_target != null && m_canvas != null){
            m_imageRed.transform.position = Camera.main.WorldToScreenPoint(m_target.position + offset);
            m_imageBlack.transform.position = m_imageRed.transform.position;
        }
    }

    public void SetTarget(Transform target) { 
        m_target = target;
    }
    
    public void SetHP(float v) { 
        m_imageRed.fillAmount = v;
    }

    public void ShowFlutterNum(string num, Color c) {
        UnityEngine.Object perfab = Resources.Load("Builds/UI/BloodNum/Prefabs/BloodNum");
        GameObject go = Instantiate(perfab) as GameObject;
		go.transform.position = m_imageRed.transform.position;
        FlutterObject fo = go.AddComponent<FlutterObject>();
        fo.SetTarget(m_target);
        fo.SetText(num, c);
    }
}