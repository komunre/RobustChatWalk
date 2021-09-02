using Lidgren.Network;
using Robust.Shared.GameObjects;
using Robust.Shared.Network;

namespace Content.Shared.Chat
{
    public class ChatMessage : NetMessage
    {
        public override MsgGroups MsgGroup => MsgGroups.Command;
        //public override string MsgName => "chat-message";
        public string Message;
        public string PlayerName;
        public EntityUid Sender;

        public override void ReadFromBuffer(NetIncomingMessage buffer)
        {
            Message = buffer.ReadString();
            PlayerName = buffer.ReadString();
            Sender = buffer.ReadEntityUid();
        }

        public override void WriteToBuffer(NetOutgoingMessage buffer)
        {
            buffer.Write(Message);
            buffer.Write(PlayerName);
            buffer.Write(Sender);
        }
    }
}