using UnityEngine;

#region Namespaces
namespace Example.Systems {} namespace Example.Components {} namespace Example.Modules {} namespace Example.Features {} namespace Example.Markers {} namespace Example.Views {}
#endregion

namespace Example {
    
    using TState = ExampleState;
    using ME.ECS;
    using ME.ECS.Views.Providers;
    using Modules;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class ExampleInitializer : InitializerBase {

        private World _world;

        public void Update() {

            if (_world == null)
            {
                CreateWorld();
            }

            if (_world != null)
            {
                UpdateWorld();
            }

        }

        private void UpdateWorld()
        {
            var dt = Time.deltaTime;
            _world.PreUpdate(dt);
            _world.Update(dt);
        }

        private void CreateWorld()
        {
            // Initialize world with 0.033 time step
            WorldUtilities.CreateWorld<TState>(ref _world, 0.033f);
            {
#if FPS_MODULE_SUPPORT
                    this.world.AddModule<FPSModule>();
#endif
                _world.AddModule<StatesHistoryModule>();
                _world.AddModule<NetworkModule>();

                // Add your custom modules here

                // Create new state
                _world.SetState<TState>(WorldUtilities.CreateState<TState>());
                ComponentsInitializer.DoInit();
                Initialize(_world);
                _world.SetSeed((uint) 1);
                // Add your custom systems here
            }
            // Save initialization state
            _world.SaveResetState<TState>();
        }

        public void LateUpdate() {
            
            if (_world != null) _world.LateUpdate(Time.deltaTime);
            
        }

        public void OnDestroy() {
            
            if (_world == null || _world.isActive == false) return;
            
            DeInitializeFeatures(_world);
            // Release world
            WorldUtilities.ReleaseWorld<TState>(ref _world);

        }

    }
    
}

namespace ME.ECS {
    
    public static partial class ComponentsInitializer {

        public static void InitTypeId() {
            
            InitTypeIdPartial();
            
        }

        static partial void InitTypeIdPartial();

        public static void DoInit() {
            
            Init(ref Worlds.currentWorld.GetStructComponents());
            
        }

        static partial void Init(ref StructComponentsContainer structComponentsContainer);

    }

}