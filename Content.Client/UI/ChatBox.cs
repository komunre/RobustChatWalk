using Robust.Client.UserInterface;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Content.Client.Chat;
using Robust.Client.Player;

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
                MinHeight = 250f,
                MaxHeight = 250f,
                //Orientation = BoxContainer.LayoutOrientation.Vertical,
                Children = {
                    (Contents = new OutputPanel {
                        Margin = new Thickness(0, 0),
                        VerticalExpand = true,
                        MinHeight = 200,
                        MaxHeight = 200,
                    }),
                    (EditPanel = new BoxContainer {
                        //VerticalAlignment = VAlignment.Bottom,
                        Margin = new Thickness(0, 250),
                        MaxHeight = 50f,
                        Orientation = BoxContainer.LayoutOrientation.Horizontal,
                        //PanelOverride = new StyleBoxFlat { BackgroundColor = Color.White },
                        Children = {
                            (LineInput = new HistoryLineEdit {
                                //Margin = new Thickness(0, 200),
                                MinHeight = 40f,
                                PlaceHolder = "Enter message",
                                MinWidth = 300,
                                HorizontalExpand = true,
                                StyleClasses = { StyleChill.LineEditClass }
                            }),
                            (SendButton = new Button {
                                Text = "Send",
                                MinWidth = 50,
                                MinHeight = 40f,
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
            LineInput.OnTextEntered += _ => {
                SendMessage();
                LineInput.Text = "";
            };

            IoCManager.Resolve<ChatManager>().SetPanel(Contents);
            SendButton.OnButtonDown += (args) => {
                SendMessage();
            };
        }

        public void SendMessage() {
            IoCManager.Resolve<ChatManager>().SendMessage(_msg, IoCManager.Resolve<IPlayerManager>().LocalPlayer.Session.AttachedEntity);
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
            LineInput.CursorPosition = _msg.Length;
            
            //Logger.Debug("Current message: " + _msg);
        }

        /*protected override void KeyBindDown(GUIBoundKeyEventArgs args) {
            if (args.CanFocus) {
                LineInput.GrabKeyboardFocus();
            }
        }*/
    }
}