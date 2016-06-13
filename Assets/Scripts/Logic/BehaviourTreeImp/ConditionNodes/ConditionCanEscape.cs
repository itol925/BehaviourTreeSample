using System;
using BHaviourTree;

class ConditionCanEscape : BConditionNode{
	public bool HasAttacker;
	public float RemainHP;

    public ConditionCanEscape() { 
        this.m_name = "ConditionCanEscape";
    }

    protected override void OnEnter(object input) {
        
    }

    protected override ActionResult Excute(object input, ref string param) {
        Input ipt = input as Input;
		if (ipt.HasAttacker == HasAttacker && ipt.RemainHP < RemainHP) {
			return ActionResult.SUCCESS;
		} else {
			return ActionResult.FAILURE;
		}
    }
}
