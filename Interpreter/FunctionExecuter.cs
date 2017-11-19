using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using SeaOfGreed.Inventory;
using SeaOfGreed.Inventory.Items;
using SeaOfGreed.StatusEffects;
using StatusEffects.Implementations.InstantHeal;
using UnityEditor;
using UnityEngine;

namespace SeaOfGreed.SOGLang
{
    class FunctionExecuter
    {

        Dictionary<string, object> functionmap = new Dictionary<string, object>();

        public GameObject player;
        public GameManager gamemanager;
        public CharacterDriver driver;

        public static Dictionary<string, object> virtualVariables = new Dictionary<string, object>();

        // In this class we are kinda reinventing the wheel because we pass a list with objects as parameter, and check in the function if they are i.e the correct type
        // I can't think of a better way though
        // This does allow for easily overloading functions


        public object playerpos(List<ParserTypes.FArg> args)
        {
            if (args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            return player.transform.position;
        }

        public object currentmap(List<ParserTypes.FArg> args)
        {
            if(args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            return gamemanager.getRoomAtLocation(player.transform.position).roomPrefab.name;
        }

        public object setplayerpos(List<ParserTypes.FArg> args)
        {
            if(args.Count != 1)
            {
                throwParamCountException(args.Count);
            }

            if( (args[0].value is Vector3) == false)
            {
                throwInvalidTypeException("Vector3", 1);
            }

            player.transform.position = (Vector3)args[0].value;

            return player.transform.position;


        }

        public object setplayerpos_rel(List<ParserTypes.FArg> args)
        {
            if (args.Count != 1)
            {
                throwParamCountException(args.Count);
            }

            if ((args[0].value is Vector3) == false)
            {
                throwInvalidTypeException("Vector3", 1);
            }

            player.transform.position = player.transform.position + (Vector3)args[0].value;

            return player.transform.position;


        }

        public object setmap(List<ParserTypes.FArg> args)
        {
            if (args.Count != 1)
            {
                throwParamCountException(args.Count);
            }

            if ((args[0].value is string) == false)
            {
                throwInvalidTypeException("String", 1);
            }

            RoomData room = gamemanager.getRoomByName(Convert.ToString(args[0].value));

            // Offset so we don't get stuck
            player.transform.position = room.position;

            return player.transform.position;


        }

		public object vectorthree(List<ParserTypes.FArg> args)
		{
			if (args.Count != 3) 
			{
				throwParamCountException(args.Count);
			}

            for (int i = 0; i < 3; i++)
            {
                if ((args[i].value is float) == false)  
                {
                    
                    throwInvalidTypeException("Single", i + 1);
                }
            }

            return new Vector3(
                Convert.ToSingle(args[0].value),
                Convert.ToSingle(args[1].value),
                Convert.ToSingle(args[2].value)
            );
		}

        public object debug(List<ParserTypes.FArg> args)
        {
            foreach (ParserTypes.FArg obj in args)
            {
                Debug.Log(obj.value);
            }
            return true;
        }

        public object initquest(List<ParserTypes.FArg> args)
        {
            if(args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            Quests.QuestHandler.init();
            return true;
        }

        public object setvar(List<ParserTypes.FArg> args)
        {
            if(args.Count != 2)
            {
                throwParamCountException(args.Count);
            }

            if( (args[0].value is string) == false)
            {
                throwInvalidTypeException("string", 0);
            }

            if(virtualVariables.ContainsKey((string)args[0].value))
            {
                throw new ArgumentException("Variable '" + args[0].value + "' is already set");
            }

            // Set dictionary var
            

            virtualVariables[(string)args[0].value] = args[1].value;
            return true;
        }

        public object getvar(List<ParserTypes.FArg> args)
        {
            if (args.Count != 1)
            {
                throwParamCountException(args.Count);
            }

            if ((args[0].value is string) == false)
            {
                throwInvalidTypeException("string", 0);
            }

            // Set dictionary var

            try
            {
                return virtualVariables[(string)args[0].value];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public object applystatuseffects(List<ParserTypes.FArg> args)
        {
            List<string> keys = new List<string>(EffectRegister.effects.Keys);
            
            for (int i = 0;  i < args.Count; i++)
            {
                if ((args[i].value is string) == false)
                {
                    throwInvalidTypeException("string", i);
                }

                if (!(keys.Contains(args[i].value as string)))
                {
                    throw new ArgumentException("No effect called '" + args[i].value + "' exists");
                }
                
                
            }

            foreach (var arg in args)
            {
                Type t = EffectRegister.effects[(string) arg.value];
                
                Effect e = (Effect)Activator.CreateInstance(t, new object[] {driver});

                
                
                driver.addEffect(e);
            }

            return true;
        }

        public object getplayerhealth(List<ParserTypes.FArg> args)
        {
            if (args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            return driver.Health;
        }

        public object additems(List<ParserTypes.FArg> args)
        {
            List<string> keys = new List<string>(ItemRegister.items.Keys);
            
            if (args.Count == 0)
            {
                throwParamCountException(0);
            }
            
            List<Item> items = new List<Item>();
            
            for (int i = 0; i < args.Count; i++)
            {
                if ((args[i].value is string) == false)
                {
                    throwInvalidTypeException("string", i);
                }
                
                if (!(keys.Contains(args[i].value as string)))
                {
                    throw new ArgumentException("No item called '" + args[i].value + "' exists");
                }
                
                
            }

            foreach (ParserTypes.FArg a in args)
            {
                Item t = ItemRegister.items[a.value as string];
                
                driver.inventory.addItem(t.clone());
            }

            return true;
        }

        public object getitems(List<ParserTypes.FArg> args)
        {
            string result = "";

            Inventory.Inventory i = driver.inventory;
            
            if (args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            
            result += "Amount of stacks in inventory: " + i.items.Count + "\n\n";

            foreach (ItemStack s in i.items)
            {
                result += "Name          \t" + s.baseItem.name + "\n";
                result += "Tooltip       \t" + s.baseItem.tooltip + "\n";
                result += "Type          \t" + s.baseItem.type + "\n";
                result += "Stack Size    \t" + s.Count + "\n";
                result += "Max Stack Size\t" + s.maxStackSize + "\n\n";

                result += "Is Sellable   \t" + s.baseItem.isSellable + "\n";
                
                if (s.baseItem.isSellable)
                {
                result += "Base Value    \t" + s.baseItem.baseValue + "\n";
                }

                result += "\n----------------------------------------------------\n\n";

            }

            return result;

        }

        public object getitemschema(List<ParserTypes.FArg> args)
        {
            string result = "";
            
            if (args.Count != 0)
            {
                throwParamCountException(args.Count);
            }

            foreach (string s in ItemRegister.items.Keys)
            {
                result += s + "\n";
            }

            return result;
        }
        
        public FunctionExecuter()
        {
            this.player = GameObject.FindGameObjectWithTag("Player");

            this.gamemanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

            this.driver = player.GetComponent<CharacterDriver>();

            functionmap.Add("playerpos", (Func<List<ParserTypes.FArg>, object>)playerpos);
            functionmap.Add("currentmap", (Func<List<ParserTypes.FArg>, object>)currentmap);

            functionmap.Add("setplayerpos", (Func<List<ParserTypes.FArg>, object>)setplayerpos);
            functionmap.Add("~setplayerpos", (Func<List<ParserTypes.FArg>, object>)setplayerpos_rel);

            functionmap.Add("vector3", (Func<List<ParserTypes.FArg>, object>)vectorthree);


            functionmap.Add("setmap", (Func<List<ParserTypes.FArg>, object>)setmap);
            functionmap.Add("debug", (Func<List<ParserTypes.FArg>, object>)debug);

            functionmap.Add("initquest", (Func<List<ParserTypes.FArg>, object>)initquest);

            functionmap.Add("setvar", (Func<List<ParserTypes.FArg>, object>)setvar);
            functionmap.Add("getvar", (Func<List<ParserTypes.FArg>, object>)getvar);
            
            functionmap.Add("applystatuseffects", (Func<List<ParserTypes.FArg>, object>)applystatuseffects);
            
            functionmap.Add("getplayerhealth", (Func<List<ParserTypes.FArg>, object>)getplayerhealth);
            
            functionmap.Add("additems", (Func<List<ParserTypes.FArg>, object>)additems);
            
            functionmap.Add("getitems", (Func<List<ParserTypes.FArg>, object>)getitems);
            functionmap.Add("getitemschema", (Func<List<ParserTypes.FArg>, object>)getitemschema);

        }

        public object execute(ParserTypes.FName fname, List<ParserTypes.FArg> args)
        {
            try
            {
                return ((Func<List<ParserTypes.FArg>, object>)functionmap[fname.name])(args);
            }
            catch(KeyNotFoundException e)
            {
                throwFunctionNameException(fname.name);
                return null;
            }
        }

        public void throwParamCountException(int count)
        {
            throw new ArgumentException("No overload for this function allows " + count.ToString() + " parameters");
        }

        public void throwFunctionNameException(string functionName)
        {
            throw new ArgumentException("Undefined function: " + functionName);
        }

        public void throwInvalidTypeException(string expectedType, int parameterIndex)
        {
            throw new ArgumentException("Wrong type for parameter " + parameterIndex + ". Should be " + expectedType);
        }
    }
}