using Modding;
using UnityEngine;
using System;
using System.Reflection;

namespace AnyRad3GlitchGenerator {
    public class AnyRad3GlitchGenerator: Mod, ITogglableMod {
        public static AnyRad3GlitchGenerator instance;
        private bool inPlatsPhase = false;
        private bool glitched = false;
        internal static FieldInfo portalPortalState;
        internal static Type portalType;
        public AnyRad3GlitchGenerator(): base ("AnyRad 3 Glitch Generator") => instance = this;

        public override void Initialize() {
            Log("Initializing...");

            ModHooks.HeroUpdateHook += HeroUpdate;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            On.CameraLockArea.Awake += CamLockAwake;
            glitched = false;

            Log("Initialized.");
            Log("Mathy is a cool guy.");
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public void HeroUpdate() {
            if (!glitched && inPlatsPhase) {
                if (
                    GameObject.Find("Absolute Radiance") != null &&
                    GameObject.Find("Portal 1") != null &&
                    GameObject.Find("Portal 1").GetComponent<CircleCollider2D>() != null &&
                    GameObject.Find("Portal 1").GetComponent<CircleCollider2D>().bounds.extents.y == 3.16679f &&
                    GameObject.Find("Legs") != null &&
                    GameObject.Find("Abyss Pit").transform.position == new Vector3(61.77f, 30, 0)
                ) {
                    Modding.Logger.Log("Inducing platform phase glitch.");
                    GameObject.Find("Absolute Radiance").transform.position = GameObject.Find("Portal 1").transform.position + new Vector3(0.5f, -1, 0);
                    glitched = true;
                }
            }
        }

        public void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);

            if (self.FsmName == "Control" && self.gameObject.name == "Absolute Radiance") {
                glitched = false;
                inPlatsPhase = false;
            }
        }

        public void CamLockAwake(On.CameraLockArea.orig_Awake orig, CameraLockArea self) {
            orig(self);

            if (self.gameObject.name == "CamLock A2") {
                Modding.Logger.Log(self.gameObject.name);
                inPlatsPhase = true;
            }
        }

        public void Unload() {
            ModHooks.HeroUpdateHook -= HeroUpdate;
        }
    }
}