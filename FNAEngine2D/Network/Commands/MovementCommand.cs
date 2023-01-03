using FNAEngine2D.Components;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace FNAEngine2D.Network.Commands
{
    [Command(65502)]
    public class MovementCommand: IClientCommand, IServerCommand
    {
        /// <summary>
        /// Game object id to move
        /// </summary>
        [DefaultValue(null)]
        public Guid? ID { get; set; } = null;

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
        public void Serialize(BinWriter writer)
        {
            writer.Write(this.ID);
            writer.Write(this.Movement);
            writer.Write(this.StartPosition);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public void Deserialize(BinReader reader)
        {
            this.ID = reader.ReadNullableGuid();
            this.Movement = reader.ReadVector2();
            this.StartPosition = reader.ReadVector2();
        }



        /// <summary>
        /// Execute
        /// </summary>
        public void ExecuteClient(NetworkClient client)
        {
            if (this.ID == null)
                return;

            var gameObject = client.GetGameObject(this.ID.Value);

            if (gameObject != null)
            {
                MovementComponent serverMovement = gameObject.GetComponent<MovementComponent>();

                if (serverMovement != null)
                    serverMovement.SetNextMovement(this.Movement, this.StartPosition);
            }
        }

        /// <summary>
        /// Movement of a player
        /// </summary>
        public void ExecuteServer(ClientWorker cw)
        {
            if (cw.Player != null)
            {
                MovementComponent component = cw.Player.GetComponent<MovementComponent>();

                if (component != null)
                {
                    component.SetNextMovement(this.Movement, this.StartPosition);

                    //Resending commands...
                    cw.SendCommandToAllClients(new MovementCommand() { ID = cw.Player.ID, Movement = this.Movement, StartPosition = this.StartPosition });

                    //clientWorker.LogInfo(clientWorker.Character.CharacterName + " moved: " + this.Movement.ToString() + " from " + this.StartPosition);
                }
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
