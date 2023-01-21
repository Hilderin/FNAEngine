using FNAEngine2D.Physics;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace FNAEngine2D.Network.Commands
{
    public class MovementCommand: ClientServerCommand
    {
        /// <summary>
        /// Movement to do
        /// </summary>
        public Vector2 Movement { get; set; }


        /// <summary>
        /// Start position
        /// </summary>
        public Vector2 StartPosition { get; set; }


        /// <summary>
        /// Serialize
        /// </summary>
        public override void Serialize(BinWriter writer)
        {
            //writer.Write(this.ID);
            writer.Write(this.Movement);
            writer.Write(this.StartPosition);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public override void Deserialize(BinReader reader)
        {
            //this.ID = reader.ReadNullableGuid();
            this.Movement = reader.ReadVector2();
            this.StartPosition = reader.ReadVector2();
        }



        /// <summary>
        /// Execute
        /// </summary>
        public override void ExecuteClient(NetworkClient client)
        {
            if (this.ID == Guid.Empty)
                return;

            var gameObject = client.GetGameObject(this.ID);

            if (gameObject != null)
            {
                RigidBody rigidBody = gameObject.GetComponent<RigidBody>();

                if (rigidBody != null)
                    rigidBody.SetNextMovement(this.Movement, this.StartPosition);
            }
        }

        /// <summary>
        /// Movement of a player
        /// </summary>
        public override void ExecuteServer(ServerCommandArgs args)
        {
            RigidBody rigidBody = args.GameObject.GetComponent<RigidBody>();

            if (rigidBody != null)
            {
                rigidBody.SetNextMovement(this.Movement, this.StartPosition);

                //Resending commands...
                args.GameObject.SendCommandToAllClients(new MovementCommand() { ID = args.GameObject.ID, Movement = this.Movement, StartPosition = this.StartPosition });

                //clientWorker.LogInfo(clientWorker.Character.CharacterName + " moved: " + this.Movement.ToString() + " from " + this.StartPosition);
            }
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "MovementCommand - moved: " + this.Movement.ToString() + " from " + this.StartPosition;
        }
    }
}
