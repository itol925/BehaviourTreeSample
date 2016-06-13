using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class SoldierView : MonoBehaviour{
	private Animator m_animator;
    private HPBar m_hpBar;
    private Tweener m_currentMoveAction;
    private float m_moveSpeed = 1.0f;

    void Awake() { 
		m_animator = gameObject.GetComponent<Animator> ();
    }

    void Start() {
        UnityEngine.Object perfab = Resources.Load("Builds/UI/HPBar/Prefabs/HPBar");
        GameObject go = Instantiate(perfab) as GameObject;
        m_hpBar = go.AddComponent<HPBar>();
        m_hpBar.SetTarget(transform);
    }

    //------- actions --------
    public void StopShowMoving() { 
        m_animator.SetInteger ("state", 0);
    }
	public void StopAttack(){
		m_animator.SetInteger ("state", 0);
		StopAllCoroutines ();
	}
	public void ShowAttack(float attackCD, Action onActioned, Action onAttacked) {
		m_animator.SetInteger ("state", 1);

		StartCoroutine(Util.DelayToInvokeDo(delegate(){
			onActioned();
			m_animator.SetInteger ("state", 0);
		}, 1.0f));

        StartCoroutine(Util.DelayToInvokeDo(delegate(){
            onAttacked();
        }, 1.0f + attackCD));
    }
    //public void ShowSkill() {
	//	m_animator.SetInteger ("state", 2);
    //    StartCoroutine(Util.DelayToInvokeDo(delegate(){
    //        m_animator.SetInteger ("state", 0);
    //    }, 3));
    //}
    public void ShowMoving() {
		m_animator.SetInteger ("state", 3);
    }
    //public void ShowBeHurt() {
	//	m_animator.SetInteger ("state", 4);
    //   StartCoroutine(Util.DelayToInvokeDo(delegate(){
    //        m_animator.SetInteger ("state", 0);
    //    }, 3));
    //}
    public void SetHpBarValue(float v) { 
        m_hpBar.SetHP(v);
    }
    public void ShowDieAndDestory() {
		m_animator.SetInteger ("state", 5);
		DestroyObject(gameObject, 3);
		StopAllCoroutines ();
    }

    public void ShowFlutterHurt(string num, Color c) {
        if (m_hpBar != null) { 
            m_hpBar.ShowFlutterNum(num, c);
        }
    }
    
    public void StopTransform() {
        if (m_currentMoveAction != null) { 
            m_currentMoveAction.Kill(false);
            m_currentMoveAction = null;
        }
    }
    public void TransformTo(Vector3 targetPosition, Action onMoved) {
		if (m_currentMoveAction != null) {
			StopTransform();
		}
        transform.LookAt(targetPosition);
        float t = Vector3.Distance(targetPosition, transform.position)/m_moveSpeed;

        m_currentMoveAction = transform.DOMove(targetPosition, t).SetEase(Ease.Linear).OnComplete(delegate(){
            StopTransform();
            onMoved();
        });
    }

    public void Idle(float t, Action onIdled) {
        StopTransform();
        StopShowMoving();
        StartCoroutine(Util.DelayToInvokeDo(delegate(){
            onIdled();
        }, t));
    }

    public void DestoryHPBar() {
        if (m_hpBar != null) {
            Destroy(m_hpBar.gameObject);
        }
    }
}

