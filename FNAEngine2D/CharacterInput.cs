using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Helper class to control a character
    /// </summary>
    public class CharacterInput
    {
        /// <summary>
        /// Left key
        /// </summary>
        public Keys LeftKey = Keys.A;

        /// <summary>
        /// Right key
        /// </summary>
        public Keys RightKey = Keys.D;

        /// <summary>
        /// Up key
        /// </summary>
        public Keys UpKey = Keys.W;

        /// <summary>
        /// Down key
        /// </summary>
        public Keys DownKey = Keys.S;

        /// <summary>
        /// Jump key
        /// </summary>
        public Keys JumpKey = Keys.Space;


        /// <summary>
        /// Is moving left?
        /// </summary>
        public bool IsLeft { get { return Input.IsKeyDown(LeftKey) && !Input.IsKeyDown(RightKey); } }

        /// <summary>
        /// Is moving right?
        /// </summary>
        public bool IsRight { get { return Input.IsKeyDown(RightKey) && !Input.IsKeyDown(LeftKey); } }

        /// <summary>
        /// Is moving up?
        /// </summary>
        public bool IsUp { get { return Input.IsKeyDown(UpKey) && !Input.IsKeyDown(DownKey); } }

        /// <summary>
        /// Is moving down?
        /// </summary>
        public bool IsDown { get { return Input.IsKeyDown(DownKey) && !Input.IsKeyDown(UpKey); } }

        /// <summary>
        /// Is jumping
        /// </summary>
        public bool IsJump { get { return Input.IsKeyDown(JumpKey); } }

        /// <summary>
        /// Is newly jumping
        /// </summary>
        public bool IsNewJump { get { return Input.IsKeyNewDown(JumpKey); } }

    }
}
