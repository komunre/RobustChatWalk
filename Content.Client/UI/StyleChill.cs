using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;

namespace Content.Client.UI
{
    public class StyleChill
    {
        public StyleRule[] StyleRules;
        public Stylesheet Stylesheet;
        public StyleBoxTexture Button;
        public static string LineEditClass = "line-edit";
        public static string ButtonClass = "button";
        public static string PanelClass = "panel";
        public StyleChill(IResourceCache resCache) {
            var notoSansRes = resCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf");
            var oswaldRes = resCache.GetResource<FontResource>("/Fonts/Oswald/Oswald-Regular.ttf");
            var buttonTextureRes = resCache.GetResource<TextureResource>("/Textures/Input/button.png");
            
            var notoSans = new VectorFont(notoSansRes, 14);
            var oswald = new VectorFont(oswaldRes, 14);

            Button = new StyleBoxTexture {
                Texture = buttonTextureRes
            };

            var lineEdit = new StyleBoxFlat {
                BackgroundColor = Color.FromHex("#76cc91")
            };

            var panel = new StyleBoxFlat {
                BackgroundColor = Color.FromHex("#81d4df"),
                BorderColor = Color.FromHex("#47747a"),
            };

            var output = new StyleBoxFlat {
                BackgroundColor = Color.FromHex("#c3dfe3"),
            };
            
            StyleRules = new [] {
                new StyleRule (
                    new SelectorElement(null, null, null, null),
                    new [] {
                        new StyleProperty("font", oswald),
                        new StyleProperty(PanelContainer.StylePropertyPanel, panel),
                    }
                ),
                new StyleRule(
                    new SelectorElement(typeof(TextureButton), new [] {ButtonClass}, null, new[] {TextureButton.StylePseudoClassNormal}),
                    new[] {
                        new StyleProperty(TextureButton.StylePropertyTexture, buttonTextureRes.Texture),
                        //new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#FFFFFF")),
                    }
                ),
                new StyleRule(
                    new SelectorElement(typeof(LineEdit), new[] {LineEditClass}, null, null),
                    new[] {
                        new StyleProperty(LineEdit.StylePropertyStyleBox, lineEdit),
                        //new StyleProperty("font", notoSans),
                    }
                ),
                new StyleRule(
                    new SelectorElement(typeof(OutputPanel), null, null, null),
                    new [] {
                        new StyleProperty(OutputPanel.StylePropertyStyleBox, output),
                    }
                ),
            };

            Stylesheet = new Stylesheet(StyleRules);
        }
    }
}