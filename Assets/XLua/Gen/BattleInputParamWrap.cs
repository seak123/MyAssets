#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class BattleInputParamWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BattleInputParam);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 2, 2);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectPos", _g_get_selectPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "selectPath", _g_get_selectPath);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectPos", _s_set_selectPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "selectPath", _s_set_selectPath);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<UnityEngine.Vector2Int>(L, 2) && translator.Assignable<UnityEngine.Vector2Int[]>(L, 3))
				{
					UnityEngine.Vector2Int _pos;translator.Get(L, 2, out _pos);
					UnityEngine.Vector2Int[] _array = (UnityEngine.Vector2Int[])translator.GetObject(L, 3, typeof(UnityEngine.Vector2Int[]));
					
					BattleInputParam gen_ret = new BattleInputParam(_pos, ref _array);
					translator.Push(L, gen_ret);
                    translator.Push(L, _array);
                        
                    
					return 2;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BattleInputParam constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleInputParam gen_to_be_invoked = (BattleInputParam)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.selectPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_selectPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleInputParam gen_to_be_invoked = (BattleInputParam)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.selectPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleInputParam gen_to_be_invoked = (BattleInputParam)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2Int gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.selectPos = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_selectPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleInputParam gen_to_be_invoked = (BattleInputParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.selectPath = (UnityEngine.Vector2Int[])translator.GetObject(L, 2, typeof(UnityEngine.Vector2Int[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
