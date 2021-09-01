using Robust.Client.UserInterface;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using Robust.Shared.IoC;
using Robust.Shared.Log;

namespace Content.Client.UI
{
    public class ChatBox : Control
    {
        public OutputPanel Contents;
        public HistoryLineEdit LineInput;
        public Button SendButton;
        public BoxContainer EditPanel;
        private string _msg = "";
        public ChatBox() {
            AddChild(new PanelContainer {
                VerticalExpand = true,
                HorizontalExpand = true,
                MinHeight = 170f,
                MaxHeight = 170f,
                Children = {
                    (Contents = new OutputPanel {
                        VerticalExpand = true
                    }),
                    (EditPanel = new BoxContainer {
                        //VerticalAlignment = VAlignment.Bottom,
                        MaxHeight = 50f,
                        Orientation = BoxContainer.LayoutOrientation.Horizontal,
                        //PanelOverride = new StyleBoxFlat { BackgroundColor = Color.White },
                        Children = {
                            (LineInput = new HistoryLineEdit {
                                PlaceHolder = "Enter message",
                                MinWidth = 300,
                                HorizontalExpand = true,
                                StyleClasses = { StyleChill.LineEditClass }
                            }),
                            (SendButton = new Button {
                                Text = "Send",
                                MinWidth = 50,
                                StyleClasses = { StyleChill.ButtonClass }
                            })
                        }
                    })
                }
            });

            LayoutContainer.SetAnchorAndMarginPreset(Contents, LayoutContainer.LayoutPreset.TopWide);
            LayoutContainer.SetAnchorAndMarginPreset(EditPanel, LayoutContainer.LayoutPreset.BottomWide);
            LayoutContainer.SetAnchorAndMarginPreset(LineInput, LayoutContainer.LayoutPreset.BottomWide);
            LayoutContainer.SetAnchorAndMarginPreset(SendButton, LayoutContainer.LayoutPreset.BottomRight);

            //UserInterfaceManager.StateRoot.AddChild(this);
            Setup();
        }

        protected void Setup() {
            LineInput.OnTextChanged += OnTextChanged;
            //LineInput.OnMouseEntered +=  EnterFocus;
            //LineInput.OnMouseExited += DeEnterFocus;
            //LineInput.OnTextEntered +=  // Message send here
        }

        public void ToggleKeyboardFocus() {
            if (LineInput.HasKeyboardFocus()) {
                LineInput.ReleaseKeyboardFocus();
            }
            else {
                LineInput.GrabKeyboardFocus();
            }
        }

        private void EnterFocus(GUIMouseHoverEventArgs args) {
            Logger.Debug("Focus entered");
            LineInput.GrabKeyboardFocus();
        }

        private void DeEnterFocus(GUIMouseHoverEventArgs args) {
            LineInput.ReleaseKeyboardFocus();
        }

        private void OnTextChanged(LineEdit.LineEditEventArgs args) {
            _msg = args.Text;
            LineInput.Text = _msg;
            
            Logger.Debug("Current message: " + _msg);
        }

        /*protected override void KeyBindDown(GUIBoundKeyEventArgs args) {
            if (args.CanFocus) {
                LineInput.GrabKeyboardFocus();
            }
        }*/
    }
}