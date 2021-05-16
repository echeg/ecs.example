using Example.Features.Logic.Input.Markers;
using ME.ECS;
using UnityEngine;

namespace Example.Features.Logic.Input.Modules
{
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class RandomInputModule : IModule, IUpdate
    {
        public World world { get; set; }

        private InputFeature _feature;
        private readonly Vector3[] _moves = {Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        void IModuleBase.OnConstruct()
        {
            _feature = world.GetFeature<InputFeature>();
        }

        void IModuleBase.OnDeconstruct()
        {
        }

        void IUpdate.Update(in float deltaTime)
        {
            if (!_feature.EnableRandomInput) return;
            if (KeyboardInputModule.PlayerIsEmpty(world)) return;

            var randomFire = Random.Range(1, 101);
            if (randomFire <= _feature.PercentRandomFire)
            {
                world.AddMarker(new InputFire());
            }
            
            var randomMove = Random.Range(1, 101);
            if (randomMove <= _feature.PercentRandomMove)
            {
                world.AddMarker(new InputDir(_moves[Random.Range(0,_moves.Length)]));
            }
        }
    }
}