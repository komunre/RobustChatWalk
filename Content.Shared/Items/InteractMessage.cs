using Robust.Shared.Network;
using Robust.Shared.GameObjects;
using Lidgren.Network;

namespace Content.Shared.Items
{
    public class InteractMessage : NetMessage 
    {
        public EntityUid Sender;
        public EntityUid Clicked;

        public override void ReadFromBuffer(NetIncomingMessage buffer)
        {
            Sender = buffer.ReadEntityUid();
            Clicked = buffer.ReadEntityUid();
        }

        public override void WriteToBuffer(NetOutgoingMessage buffer)
        {
            buffer.Write(Sender);
            buffer.Write(Clicked);
        }
    }
}