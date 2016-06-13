using UnityEngine;
using System.Collections;
using BHaviourTree;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
    ArrayList m_soldierList = new ArrayList();
    BehavTree m_btree = null;

	int recoverTime = 50;

	// Use this for initialization
	void Start () {
        LoadSoldiers();
        m_btree = LoadBehaviourTree();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_btree != null) {
			for (int i = 0; i < m_soldierList.Count; i++) { 
				Soldier soldier = m_soldierList [i] as Soldier;
				if (soldier.IsDied) {
					continue;
				}
				Input input = soldier.CurrentState;
				List<string> actions = m_btree.Run (input);
				for (int j = 0; j < actions.Count; j++) {
					string action = actions [j];
					switch (action) {
					case "escape":
						soldier.Escape ();
						break;
					case "attack":
						soldier.Attack ();
						break;
					case "patrol":
						soldier.Patrol ();
						break;
					}
				}
			}
		}

		if (recoverTime <= 0) {
			for (int i = 0; i < m_soldierList.Count; i++) { 
				Soldier soldier = m_soldierList [i] as Soldier;
				if (soldier.IsDied) {
					continue;
				}
				soldier.Recover(5);
			}
			recoverTime = 50;
		}
		recoverTime--;
	}
	BehavTree LoadBehaviourTree() { 
		string treePath = Application.dataPath + "/" + "Trees/test.json";
        if(!File.Exists(treePath)){
            Debug.LogError("behaviour tree file missed!");
            return null;
        }
        BehavTree tree = BehavTreeMagager.Instance.LoadTree(treePath);
        return tree;
    }

    void LoadSoldiers() { 
	    Soldier soldier = CreateSoldier(1);
        soldier.View.transform.position = new Vector3(-4, soldier.View.transform.position.y, -4);
		SoldierData data = new SoldierData(1, 100, 20, 1.0f);
		soldier.Data = data;
        m_soldierList.Add(soldier);

	    Soldier soldier2 = CreateSoldier(2);
		soldier2.View.transform.position = new Vector3(4, soldier.View.transform.position.y, 4);
		SoldierData data2 = new SoldierData(2, 100, 15, 2.25f);
		soldier2.Data = data2;
        m_soldierList.Add(soldier2);
    }

    Soldier CreateSoldier(int type) {
        string name = "";
        if(type == 1){
            name = "Soldier1";
        } else { 
            name = "Soldier2";
        }
        UnityEngine.Object perfab = Resources.Load("Builds/Objects/Soldier/Prefabs/" + name);
        GameObject go = Instantiate(perfab) as GameObject;
        go.transform.parent = transform;
        
        SoldierView view = go.AddComponent<SoldierView>();
        Soldier soldier = new Soldier(view);

        return soldier;        
    }

	public Soldier GetAttacker(Soldier self){
		return GetNearSoldier (self, self.Data.EscapeDistance);
	}

	public Soldier GetDefender(Soldier self) {
		return GetNearSoldier (self, self.Data.AttackDistance);
    }
	public Soldier GetNearSoldier(Soldier self, float distance){
		for (int i = 0; i < m_soldierList.Count; i++) { 
			Soldier soldier = m_soldierList[i] as Soldier;
			if (soldier == self) { 
				continue;
			}
			if (soldier.IsDied) { 
				continue;
			}
			float dis = Vector3.Distance(self.View.transform.position, soldier.View.transform.position);
			if (dis <= distance) { 
				return soldier;
			}
		}
		return null;
	}
}
