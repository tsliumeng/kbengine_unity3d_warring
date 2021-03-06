namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	
    public class Avatar : AvatarBase
    {
    	public CombatImpl combat = null;
    	
    	public static SkillBox skillbox = new SkillBox();
    	
		private Dictionary<string, object> temp_propertys_ = 
			new Dictionary<string, object>();

		public Avatar()
		{
		}
		
		public override void __init__()
		{
			combat = new CombatImpl(this);
		}
		
		public override void onDestroy ()
		{
			if(isPlayer())
			{
				KBEngine.Event.deregisterIn(this);
			}
		}

		public object getTempProperty(string name)
		{
			object obj;
			if(!temp_propertys_.TryGetValue(name, out obj))
			{
				return null;
			}

			return obj;
		}
		
		public void setTempProperty(string name, object val)
		{
			object obj;
			if(!temp_propertys_.TryGetValue(name, out obj))
				temp_propertys_.Add(name, val);
			else
				temp_propertys_[name] = val;
		}

		public void relive(Byte type)
		{
			cellEntityCall.relive(type);
		}
		
		public bool useTargetSkill(Int32 skillID, Int32 targetID)
		{
			Skill skill = SkillBox.inst.get(skillID);
			if(skill == null)
				return false;

			SCEntityObject scobject = new SCEntityObject(targetID);
			if(skill.validCast(this, scobject))
			{
				skill.use(this, scobject);
			}

			return true;
		}
		
		public override void onAddSkill(Int32 skillID)
		{
			Dbg.DEBUG_MSG(className + "::onAddSkill(" + skillID + ")"); 
			Event.fireOut("onAddSkill", new object[]{this});
			
			Skill skill = new Skill();
			skill.id = skillID;
			skill.name = skillID + " ";
			switch(skillID)
			{
				case 1:
					break;
				case 1000101:
					skill.canUseDistMax = 20f;
					break;
				case 2000101:
					skill.canUseDistMax = 20f;
					break;
				case 3000101:
					skill.canUseDistMax = 20f;
					break;
				case 4000101:
					skill.canUseDistMax = 20f;
					break;
				case 5000101:
					skill.canUseDistMax = 20f;
					break;
				case 6000101:
					skill.canUseDistMax = 20f;
					break;
				default:
					break;
			};

			SkillBox.inst.add(skill);
		}
		
		public override void onRemoveSkill(Int32 skillID)
		{
			Dbg.DEBUG_MSG(className + "::onRemoveSkill(" + skillID + ")"); 
			Event.fireOut("onRemoveSkill", new object[]{this});
			SkillBox.inst.remove(skillID);
		}

		public override void dialog_addOption(Byte arg1, UInt32 arg2, string arg3, Int32 arg4) {}
		public override void dialog_close() {}
		public override void dialog_setText(string arg1, Byte arg2, UInt32 arg3, string arg4) {} 

		public void jump()
		{
			cellEntityCall.jump();
		}
		
		public override void onJump()
		{
			Dbg.DEBUG_MSG(className + "::onJump: " + id);
			Event.fireOut("otherAvatarOnJump", new object[]{this});
		}

		public override void onEnterWorld()
		{
			base.onEnterWorld();

			if(isPlayer())
			{
				Event.fireOut("onAvatarEnterWorld", new object[]{KBEngineApp.app.entity_uuid, id, this});
				SkillBox.inst.pull();
			}
		}

		public override void onHPChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_HP: " + old + " => " + v); 
			Event.fireOut("set_HP", new object[]{this, HP, HP_Max});
		}
		
		public override void onMPChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_MP: " + old + " => " + v); 
			Event.fireOut("set_MP", new object[]{this, MP, MP_Max});
		}
		
		public override void onHP_MaxChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_HP_Max: " + old + " => " + v); 
			Event.fireOut("set_HP_Max", new object[]{this, HP_Max, HP});
		}
		
		public override void onMP_MaxChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_MP_Max: " + old + " => " + v); 
			Event.fireOut("set_MP_Max", new object[]{this, MP_Max, MP});
		}
		
		public override void onLevelChanged(UInt16 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_level: " + old + " => " + v); 
			Event.fireOut("set_level", new object[]{this, level});
		}
		
		public override void onNameChanged(string oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_name: " + old + " => " + v); 
			Event.fireOut("set_name", new object[]{this, name});
		}
		
		public override void onStateChanged(SByte oldValue)
		{
			Dbg.DEBUG_MSG(className + "::set_state: " + oldValue + " => " + state); 
			Event.fireOut("set_state", new object[]{this, state});
		}

		public override void onMoveSpeedChanged(Byte oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_moveSpeed: " + oldValue + " => " + moveSpeed); 
			Event.fireOut("set_moveSpeed", new object[]{this, moveSpeed});
		}
		
		public override void onModelScaleChanged(Byte oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_modelScale: " + old + " => " + v); 
			Event.fireOut("set_modelScale", new object[]{this, modelScale});
		}
		
		public override void onModelIDChanged(UInt32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_modelID: " + old + " => " + v); 
			Event.fireOut("set_modelID", new object[]{this, modelID});
		}

		public override void recvDamage(Int32 attackerID, Int32 skillID, Int32 damageType, Int32 damage)
		{
			// Dbg.DEBUG_MSG(className + "::recvDamage: attackerID=" + attackerID + ", skillID=" + skillID + ", damageType=" + damageType + ", damage=" + damage);
			Entity entity = KBEngineApp.app.findEntity(attackerID);

			Event.fireOut("recvDamage", new object[]{this, entity, skillID, damageType, damage});
		}
    }
} 
