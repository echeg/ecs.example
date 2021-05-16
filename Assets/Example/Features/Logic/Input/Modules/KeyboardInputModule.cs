using ME.ECS;
using UnityEngine;

namespace Example.Features.Logic.Input.Modules
{
    using Components;
    using Modules;
    using Systems;
    using Features;
    using Markers;

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public sealed class KeyboardInputModule : IModule, IUpdate
    {
        public World world { get; set; }

        void IModuleBase.OnConstruct()
        {
        }

        void IModuleBase.OnDeconstruct()
        {
        }

        void IUpdate.Update(in float deltaTime)
        {
            if (PlayerIsEmpty()) return;

            var dir = GetMoveInput();
            if (dir != Vector3.zero)
            {
                world.AddMarker(new InputDir()
                {
                    dir = dir
                });
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                world.AddMarker(new InputFire());
            }
        }

        private static Vector3 GetMoveInput()
        {
            var dir = Vector3.zero;

            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                dir += Vector3.left;
            }

            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                dir += Vector3.right;
            }

            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                dir += Vector3.forward;
            }

            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                dir += Vector3.back;
            }

            return dir;
        }

        private bool PlayerIsEmpty()
        {
            var playersFeature = world.GetFeature<PlayersFeature>();
            var playerEntity = playersFeature.GetEntityByActorId(playersFeature.GetActivePlayerId());
            return playerEntity == Entity.Empty;
        }
    }
}