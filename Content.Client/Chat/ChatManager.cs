using Robust.Shared.IoC;
using Robust.Shared.Network;
using Content.Shared.GameOjects;
using Content.Shared;
using Content.Shared.Chat;
using Robust.Shared.Log;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;

namespace Content.Client.Chat
{
    public class ChatManager
    {
        [Dependency] private readonly INetManager _netManager = default!;
        private OutputPanel _panel;

        public void Initialize() {
            IoCManager.InjectDependencies(this);

            _netManager.RegisterNetMessage<ChatMessage>(OnChatMessage);
        }

        public void SetPanel(OutputPanel panel) {
            _panel = panel;
        }

        public void SendMessage(string msg, IEntity chatter) {
            Logger.Debug("Sending message: " + msg);
            var message = _netManager.CreateNetMessage<ChatMessage>();
            message.Message = msg;
            message.Sender = chatter.Uid;
            message.PlayerName = chatter.TryGetComponent<ChatterComponent>(out var comp) ? comp.PlayerName : "error";
            _netManager.ClientSendMessage(message);
        }

        private void OnChatMessage(ChatMessage msg) {
            Logger.Debug("Got message: " + msg.Message);
            _panel.AddText(msg.Message);
        }
    }
}