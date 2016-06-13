using System;
using BHaviourTree;
using UnityEngine;

public class Input {
    private Soldier m_target;
	public Soldier Target{
        get { return m_target; }
        set { m_target = value; }
    }
	private float m_remainHP;       	// 还有多少血
    private bool m_hasAttacker;    	// 可以攻击自己的人
    private bool m_hasDefender;   	// 可以被自己攻击的人

    public void Update(float remainHP, bool hasAttacker, bool hasDefender) { 
        m_remainHP = remainHP;
		m_hasAttacker = hasAttacker;
		m_hasDefender = hasDefender;
    }
    public bool HasDefender {
        get { 
			return m_hasDefender;
        }
    }
	public bool HasAttacker{
		get{
			return m_hasAttacker;
		}
	}
	public float RemainHP{
		get{
			return m_remainHP;
		}
	}

    public bool ShouldEscape {
        get {
			if(m_target.IsDied){
				return false;
			}
			if (m_hasAttacker && m_remainHP < 0.3f) { 
                return true;
            }
			return false;
        }
    }
}

