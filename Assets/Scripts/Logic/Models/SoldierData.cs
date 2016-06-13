using System;

public class SoldierData {
    public int HP;
	public int Attack;
	public float AttackCD;
    public int AttackDistance;
    public int MaxHP;
	public int ID;
	public int FollowDistance;
	public int EscapeDistance;

    public SoldierData(int id, int hp, int attack, float cd) {
		ID = id;
        HP = hp;
        MaxHP = hp;
        Attack = attack;
		AttackCD = cd;
        AttackDistance = 2;
		FollowDistance = 3;
		EscapeDistance = 3;
    }
}
