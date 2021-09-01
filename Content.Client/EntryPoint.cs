using Robust.Client;
using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Client.Graphics;
using Content.Client.Overlays;
using Robust.Client.State;

// DEVNOTE: Games that want to be on the hub are FORCED use the "Content." prefix for assemblies they want to load.
namespace Content.Client
{
    public class EntryPoint : GameClient
    {
        public override void PreInit() {
            IoCManager.Resolve<IClyde>().SetWindowTitle("RobustChatWalk");
        }
        public override void Init()
        {
            var factory = IoCManager.Resolve<IComponentFactory>();
            var prototypes = IoCManager.Resolve<IPrototypeManager>();

            factory.DoAutoRegistrations();

            foreach (var ignoreName in IgnoredComponents.List)
            {
                factory.RegisterIgnore(ignoreName);
            }

            foreach (var ignoreName in IgnoredPrototypes.List)
            {
                prototypes.RegisterIgnore(ignoreName);
            }

            IoCManager.Register<InputHookupManager, InputHookupManager>();

            ClientContentIoC.Register();

            IoCManager.BuildGraph();

            factory.GenerateNetIds();

            // DEVNOTE: This is generally where you'll be setting up the IoCManager further.
            IoCManager.Resolve<InputHookupManager>().Initialize();
        }

        public override void PostInit()
        {
            base.PostInit();

            // DEVNOTE: Further setup...
            var client = IoCManager.Resolve<IBaseClient>();
            
            // DEVNOTE: You might want a main menu to connect to a server, or start a singleplayer game.
            // Be sure to check out StateManager for this! Below you'll find examples to start a game.
            
            // If you want to connect to a server...
            //client.ConnectToServer("localhost", 1212);
            
            // Optionally, singleplayer also works!
            // client.StartSinglePlayer();
            
            IoCManager.Resolve<ILightManager>().Enabled = false;
            
            var overlayManager = IoCManager.Resolve<IOverlayManager>();
            overlayManager.AddOverlay(new ChatterOverlay());

            IoCManager.Resolve<IStateManager>().RequestStateChange<GameScreen>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            // DEVNOTE: You might want to do a proper shutdown here.
        }

        public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
        {
            base.Update(level, frameEventArgs);
            // DEVNOTE: Game update loop goes here. Usually you'll want some independent GameTicker.
        }
    }

    
}