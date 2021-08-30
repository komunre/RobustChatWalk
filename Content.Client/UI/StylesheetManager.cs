using Robust.Client.UserInterface;
using Robust.Client.ResourceManagement;
using Robust.Shared.IoC;

namespace Content.Client.UI
{
    public class StylesheetManager
    {
        [Dependency] private readonly IResourceCache _resourceCache = default!;
        [Dependency] private readonly IUserInterfaceManager _UIManager = default!;

        public Stylesheet SheetChill { get; private set; }
        public void Initialize() {
            SheetChill = new StyleChill(_resourceCache).Stylesheet;

            _UIManager.Stylesheet = SheetChill;
        }
    }
}