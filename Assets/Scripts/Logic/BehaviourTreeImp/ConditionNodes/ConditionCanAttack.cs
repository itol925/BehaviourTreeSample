using System;
using BHaviourTree;

class ConditionCanAttack : BConditionNode{
    public bool HasDefender;

    public ConditionCanAttack() { 
        this.m_name = "ConditionCanAttack";
    }

    protected override void OnEnter(object input) {
        
    }

    protected override ActionResult Excute(object input, ref string param) {
        Input ipt = input as Input;
		if (ipt.HasDefender == HasDefender) { // 条件相同
            return ActionResult.SUCCESS;
        } else { 
            return ActionResult.FAILURE;
        }        
    }
}
