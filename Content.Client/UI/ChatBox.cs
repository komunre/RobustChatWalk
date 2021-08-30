using Robust.Client.UserInterface;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using Robust.Shared.IoC;

namespace Content.Client.UI
{
    public class ChatBox : Control
    {
        public OutputPanel Contents;
        public HistoryLineEdit LineInput;
        public Button SendButton;
        private string _msg = "";
        public ChatBox() {
            AddChild(new PanelContainer {
                PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#81d4df") },
                VerticalExpand = true,
                HorizontalExpand = true,
                MinHeight = 100f,
                MaxHeight = 100f,
                Children = {
                    (Contents = new OutputPanel {
                        VerticalExpand = true
                    }),
                    new PanelContainer {
                        MaxHeight = 10f,
                        PanelOverride = new StyleBoxFlat { BackgroundColor = Color.White },
                        Children = {
                            (LineInput = new HistoryLineEdit {
                                PlaceHolder = "Enter message",
                                MinWidth = 75,
                                HorizontalExpand = true,
                                StyleClasses = { StyleChill.LineEditClass }
                            }),
                            (SendButton = new Button {
                                Text = "Send",
                                MinWidth = 15,
                                StyleClasses = { StyleChill.ButtonClass }
                            })
                        }
                    }
                }
            });

            //UserInterfaceManager.StateRoot.AddChild(this);
        }

        protected void Setup() {
            LineInput.OnTextChanged += OnTextChanged;
        }

        private void OnTextChanged(LineEdit.LineEditEventArgs args) {
            _msg = args.Text;
        }

        protected override void KeyBindDown(GUIBoundKeyEventArgs args) {
            if (args.CanFocus) {
                LineInput.GrabKeyboardFocus();
            }
        }
    }
}