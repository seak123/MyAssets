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
    public class BattleManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BattleManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 6, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSelectParam", _m_GetSelectParam);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartBattle", _m_StartBattle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState", _m_ChangeState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetState", _m_GetState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsInCardActiveState", _m_IsInCardActiveState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SelectCard", _m_SelectCard);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleaseCard", _m_ReleaseCard);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurMode", _g_get_CurMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectPos", _g_get_SelectPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onReleaseCard", _g_get_onReleaseCard);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onSelectMap", _g_get_onSelectMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onOperationModeChange", _g_get_onOperationModeChange);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onTouchMap", _g_get_onTouchMap);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "onReleaseCard", _s_set_onReleaseCard);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onSelectMap", _s_set_onSelectMap);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onOperationModeChange", _s_set_onOperationModeChange);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onTouchMap", _s_set_onTouchMap);
            
			
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
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					BattleManager gen_ret = new BattleManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BattleManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSelectParam(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        BattleInputParam gen_ret = gen_to_be_invoked.GetSelectParam(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartBattle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StartBattle(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _newState = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.ChangeState( _newState );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        BattleState gen_ret = gen_to_be_invoked.GetState(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsInCardActiveState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsInCardActiveState(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SelectCard(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    CardItem _card = (CardItem)translator.GetObject(L, 2, typeof(CardItem));
                    
                        bool gen_ret = gen_to_be_invoked.SelectCard( _card );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseCard(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    CardItem _card = (CardItem)translator.GetObject(L, 2, typeof(CardItem));
                    
                        bool gen_ret = gen_to_be_invoked.ReleaseCard( _card );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onReleaseCard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onReleaseCard);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onSelectMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onSelectMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onOperationModeChange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onOperationModeChange);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onTouchMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onTouchMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onReleaseCard(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onReleaseCard = translator.GetDelegate<System.Action<GestureData>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onSelectMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onSelectMap = translator.GetDelegate<System.Action<UnityEngine.Vector3>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onOperationModeChange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onOperationModeChange = translator.GetDelegate<System.Action<BattleOperationMode>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onTouchMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BattleManager gen_to_be_invoked = (BattleManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onTouchMap = translator.GetDelegate<System.Action<MapCoordinates>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
