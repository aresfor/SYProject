// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="BrainsCreatureBrain"/> which controls the creature using keyboard input.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "KeyboardBrain", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Keyboard Brain")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsKeyboardBrain")]
    public sealed class BrainsKeyboardBrain : BrainsCreatureBrain
    {
        /************************************************************************************************************************/

        [SerializeField]
        private BrainsCreatureState _Locomotion;

        /************************************************************************************************************************/

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
            {
                // Get the camera's forward and right vectors and flatten them onto the XZ plane.
                var camera = Camera.main.transform;

                var forward = camera.forward;
                forward.y = 0;
                forward.Normalize();

                var right = camera.right;
                right.y = 0;
                right.Normalize();

                // Build the movement vector by multiplying the input by those axes.
                MovementDirection =
                    right * input.x +
                    forward * input.y;

                // Determine if the player wants to run or not.
                IsRunning = Input.GetButton("Fire1");//Left Shift by default.

                // Enter the locomotion state if we aren't already in it.
                _Locomotion.TryEnterState();
            }
            else
            {
                // Clear the movement vector, though nothing should be using it while idle anyway.
                MovementDirection = Vector3.zero;

                // Enter the idle state if we aren't already in it.
                Creature.Idle.TryEnterState();
            }
        }

        /************************************************************************************************************************/
    }
}
