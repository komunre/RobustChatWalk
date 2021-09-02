using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Timing;
using Robust.Shared.Configuration;
using Robust.Shared;
using Content.Server.Chat;

// DEVNOTE: Games that want to be on the hub are FORCED use the "Content." prefix for assemblies they want to load.
namespace Content.Server
{
    public class EntryPoint : GameServer
    {
        public override void Init()
        {
            base.Init();

            var factory = IoCManager.Resolve<IComponentFactory>();

            factory.DoAutoRegistrations();

            foreach (var ignoreName in IgnoredComponents.List)
            {
                factory.RegisterIgnore(ignoreName);
            }

            IoCManager.Register<ChatManager>();

            ServerContentIoC.Register();

            IoCManager.BuildGraph();

            factory.GenerateNetIds();

            // DEVNOTE: This is generally where you'll be setting up the IoCManager further.
            IoCManager.Resolve<ChatManager>().Initialize();
        }

        public override void PostInit()
        {
            base.PostInit();
            // DEVNOTE: Can also initialize IoC stuff more here.

            //IoCManager.Resolve<IConfigurationManager>().SetCVar(CVars.NetPVS, false);
        }

        public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
        {
            base.Update(level, frameEventArgs);
            // DEVNOTE: Game update loop goes here. Usually you'll want some independent GameTicker.
        }
    }
}
