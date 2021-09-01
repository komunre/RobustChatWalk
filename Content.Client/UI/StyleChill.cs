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
        public StyleChill(IResourceCache resCache) {
            var notoSansRes = resCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf");
            var buttonTextureRes = resCache.GetResource<TextureResource>("/Textures/Input/button.png");
            
            Button = new StyleBoxTexture {
                Texture = buttonTextureRes
            };

            var lineEdit = new StyleBoxFlat {
                BackgroundColor = Color.FromHex("76cc91")
            };
            
            StyleRules = new [] {
                new StyleRule (
                    new SelectorElement(null, null, null, null),
                    new [] {
                        new StyleProperty("font", notoSansRes)
                    }
                ),
                new StyleRule(
                    new SelectorElement(typeof(TextureButton), new [] {ButtonClass}, null, new[] {TextureButton.StylePseudoClassNormal}),
                    new[] {
                        new StyleProperty(TextureButton.StylePropertyTexture, buttonTextureRes),
                        new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#FFFFFF")),
                        new StyleProperty("font", notoSansRes),
                    }
                ),
                new StyleRule(
                    new SelectorElement(typeof(LineEdit), new[] {LineEditClass}, null, null),
                    new[] {
                        new StyleProperty(LineEdit.StylePropertyStyleBox, new StyleBoxEmpty()),
                        new StyleProperty("font", notoSansRes),
                    }
                )
            };

            Stylesheet = new Stylesheet(StyleRules);
        }
    }
}