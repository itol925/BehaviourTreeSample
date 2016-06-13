using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class Soldier {
    private GameManager m_gm;
    private GameManager GM {
        get {
            if (m_gm == null) { 
                m_gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            return m_gm;
        }
    }
    //------ model and view ------------    
    private SoldierView m_view; 
    private SoldierData m_data;
    public Soldier(SoldierView view) { 
        m_view = view;
    }
    public SoldierView View {
        get { return m_view;}
    }
    public SoldierData Data {
        get { return m_data; }
        set { m_data = value; }
    }
    //------- 当前的状态 ----------
    private Input m_currentState = new Input();
    public Input CurrentState {
        get { 
            float remain = (float)m_data.HP/(float)m_data.MaxHP;
            Soldier attacker = GM.GetAttacker(this);
            Soldier defender = GM.GetDefender(this);
            m_currentState.Update(remain, attacker != null, defender != null);
            m_currentState.Target = this;
            return m_currentState;
        }
    }

    //----- attack ------
    bool m_isAttacking = false;
    public void Attack() {
        if (IsDied) { 
            return;
        }
        if (m_isAttacking) { 
            return;
        }
        Soldier defender = GM.GetAttacker(this);
        if (defender == null) { 
            return;
        }
        if (defender.IsDied) { 
            return;
        }
        StopPatrol();
		StopEscape ();

        m_isAttacking = true;
		m_view.transform.LookAt (defender.View.transform.position);
        m_view.ShowAttack(m_data.AttackCD,
		    delegate(){
				float dis = Vector3.Distance(View.transform.position, defender.View.transform.position);
				if(dis > m_data.AttackDistance){
					defender.Miss();
				}else{
					defender.BeHurt(m_data.Attack + UnityEngine.Random.Range(-5, 5));
				}
			},delegate(){
	            m_isAttacking = false;
	        }
		);
        Debug.Log(m_data.ID.ToString() + ":attack...");
    }
	void StopAttack(){
		m_isAttacking = false;
		m_view.StopAttack ();
	}

    //----- escape ------
    bool m_isEscaping = false;
	public void Escape(){
		if (IsDied) {
			return;
		}
        if (m_isEscaping) { 
            return;
        }
		StopAttack ();
		StopPatrol ();
        
        m_isEscaping = true;
		Vector3 target;
		float x = UnityEngine.Random.Range(-4.5f, 4.5f);
		float z = UnityEngine.Random.Range(-4.5f, 4.5f);
			
		target = new Vector3(x, m_view.transform.position.y, z);
        
        m_view.ShowMoving();
        m_view.TransformTo(target, delegate(){
            m_isEscaping = false;
            m_view.StopShowMoving();
        });
		Debug.Log(m_data.ID.ToString() + "Escape...");
    }
	void StopEscape(){
		m_isEscaping = false;
		m_view.StopShowMoving();
		m_view.StopTransform ();
	}

    //--- hurt and recover ---
	public void BeHurt(int num) {
		if (IsDied) {
			return;
		}
        m_view.ShowFlutterHurt("-" + num.ToString(), Color.red);
        m_data.HP -= num;
        m_view.SetHpBarValue((float)m_data.HP/(float)m_data.MaxHP);
        if (IsDied) { 
            Die();
        }
    }
	public void Miss() {
		if (IsDied) {
			return;
		}
		m_view.ShowFlutterHurt("Miss", Color.red);
	}
	public void Recover(int num){
		if (m_data.HP >= m_data.MaxHP) {
			return;
		}
		if (m_isAttacking) {
			return;
		}
		m_data.HP += num;
		if (m_data.HP >= m_data.MaxHP) {
			m_data.HP = m_data.MaxHP;
		}
		m_view.ShowFlutterHurt ("+" + num.ToString(), Color.green);
		m_view.SetHpBarValue ((float)m_data.HP/(float)m_data.MaxHP);
	}
    public bool IsDied {
        get { 
            return m_data.HP <= 0;
        }
    }
	public void Die() { 
		Debug.Log (m_data.ID.ToString () + ":Die");
        m_view.DestoryHPBar();
		m_view.ShowDieAndDestory();
        m_view.StopTransform();
    }
	
	//------ patrol -------
	bool m_isPatroling = false;
    public void Patrol() {
		if (IsDied) {
			return;
		}
        if (m_isPatroling) { 
            return;
        }
		StopAttack ();
		StopEscape ();
        m_isPatroling = true;

        StartPatrolMove();
		Debug.Log(m_data.ID.ToString() + ":Patrol...");
    }
    void StartPatrolMove() { 
		if (!m_isPatroling) { 
			return;
		}
        float r = UnityEngine.Random.Range(0.0f, 10.0f);
        if (r > 4.0f) {
            float x = UnityEngine.Random.Range(-4.5f, 4.5f);
            float z = UnityEngine.Random.Range(-4.5f, 4.5f);

            Vector3 target = new Vector3(x, m_view.transform.position.y, z);
            m_view.ShowMoving();
            m_view.TransformTo(target, delegate(){
                m_view.StopShowMoving();
                StartPatrolMove();
            });
        } else { 
            float time = r;
            m_view.Idle(time, delegate(){
                StartPatrolMove();
            });
        }        
    }
    void StopPatrol() { 
		if (IsDied) {
			return;
		}
        m_view.StopShowMoving();
        m_view.StopTransform();
        m_isPatroling = false;
		Debug.Log (m_data.ID.ToString () + ":StopPatrol");
    }
}
