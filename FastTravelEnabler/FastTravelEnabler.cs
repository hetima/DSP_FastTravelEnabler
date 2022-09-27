using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
//using System.Text;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace FastTravelEnablerMod
{
    [BepInPlugin(__GUID__, __NAME__, "1.0.1")]
    public class FastTravelEnabler : BaseUnityPlugin
    {
        public const string __NAME__ = "FastTravelEnabler";
        public const string __GUID__ = "com.hetima.dsp." + __NAME__;

        new internal static ManualLogSource Logger;
        void Awake()
        {
            Logger = base.Logger;
            //Logger.LogInfo("Awake");

            new Harmony(__GUID__).PatchAll(typeof(Patch));
        }


        public static bool ModSandboxToolsEnabled()
        {
            return GameMain.instance != null;
        }

        public static IEnumerable<CodeInstruction> PatchSandboxToolsEnabled(IEnumerable<CodeInstruction> instructions)
        {

            MethodInfo target = typeof(GameMain).GetMethod("get_sandboxToolsEnabled");
            MethodInfo rep = typeof(FastTravelEnabler).GetMethod("ModSandboxToolsEnabled");

            foreach (var ins in instructions)
            {
                if (ins.opcode == OpCodes.Call && ins.operand is MethodInfo o && o == target)
                {
                    ins.operand = rep;
                    yield return ins;
                }
                else
                {
                    yield return ins;
                }
            }
        }

        static class Patch
        {

            [HarmonyTranspiler, HarmonyPatch(typeof(UIGlobemap), "_OnUpdate")]
            public static IEnumerable<CodeInstruction> UIGlobemap__OnUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "_OnUpdate")]
            public static IEnumerable<CodeInstruction> UIStarmap__OnUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "UpdateCursorView")]
            public static IEnumerable<CodeInstruction> UIStarmap_UpdateCursorView_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "StartFastTravelToUPosition")]
            public static IEnumerable<CodeInstruction> UIStarmap_TeleportToUPosition_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "OnScreenClick")]
            public static IEnumerable<CodeInstruction> UIStarmap_OnScreenClick_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "SandboxRightClickFastTravelLogic")]
            public static IEnumerable<CodeInstruction> UIStarmap_SandboxRightClickFastTravelLogic_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "DoRightClickFastTravel")]
            public static IEnumerable<CodeInstruction> UIStarmap_DoRightClickFastTravel_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            [HarmonyTranspiler, HarmonyPatch(typeof(UIStarmap), "OnFastTravelButtonClick")]
            public static IEnumerable<CodeInstruction> UIStarmap_OnFastTravelButtonClick_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return PatchSandboxToolsEnabled(instructions);
            }

            

            //Protection for achievement friendly
            [HarmonyPrefix, HarmonyPatch(typeof(ABN_MechaPosition), "OnGameTick")]
            public static bool ABN_MechaPosition_OnGameTick_Prefix(ABN_MechaPosition __instance)
            {
                return false;
            }
            
        }


    }
}
