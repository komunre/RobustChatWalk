using Content.Shared.Chat;
using Robust.Shared.Network;
using Robust.Shared.IoC;
using Robust.Shared.Log;

namespace Content.Server.Chat
{
    public class ChatManager
    {
        [Dependency] private readonly INetManager _netManager = default!;
        public void Initialize() {
            IoCManager.InjectDependencies(this);

            _netManager.RegisterNetMessage<ChatMessage>(OnChatMessage);
        }

        private void OnChatMessage(ChatMessage msg) {
            Logger.Debug("Got message: " + msg + ". Broadcasting...");
            _netManager.ServerSendToAll(msg);
        }
    }
}