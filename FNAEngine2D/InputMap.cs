using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Mapping for an input action/key
    /// </summary>
    public class InputMap
    {
        /// <summary>
        /// Key from the Keyboard
        /// </summary>
        public Keys Key { get; set; } = Keys.None;

        /// <summary>
        /// Mouse button
        /// </summary>
        public MouseButton MouseButton { get; set; } = MouseButton.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public InputMap()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InputMap(Keys key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InputMap(MouseButton mouseButton)
        {
            this.MouseButton = mouseButton;
        }

        //Keyboard mappings
        public static readonly InputMap None = new InputMap(Keys.None);
        public static readonly InputMap Back = new InputMap(Keys.Back);
        public static readonly InputMap Tab = new InputMap(Keys.Tab);
        public static readonly InputMap Enter = new InputMap(Keys.Enter);
        public static readonly InputMap CapsLock = new InputMap(Keys.CapsLock);
        public static readonly InputMap Escape = new InputMap(Keys.Escape);
        public static readonly InputMap Space = new InputMap(Keys.Space);
        public static readonly InputMap PageUp = new InputMap(Keys.PageUp);
        public static readonly InputMap PageDown = new InputMap(Keys.PageDown);
        public static readonly InputMap End = new InputMap(Keys.End);
        public static readonly InputMap Home = new InputMap(Keys.Home);
        public static readonly InputMap Left = new InputMap(Keys.Left);
        public static readonly InputMap Up = new InputMap(Keys.Up);
        public static readonly InputMap Right = new InputMap(Keys.Right);
        public static readonly InputMap Down = new InputMap(Keys.Down);
        public static readonly InputMap Select = new InputMap(Keys.Select);
        public static readonly InputMap Print = new InputMap(Keys.Print);
        public static readonly InputMap Execute = new InputMap(Keys.Execute);
        public static readonly InputMap PrintScreen = new InputMap(Keys.PrintScreen);
        public static readonly InputMap Insert = new InputMap(Keys.Insert);
        public static readonly InputMap Delete = new InputMap(Keys.Delete);
        public static readonly InputMap Help = new InputMap(Keys.Help);
        public static readonly InputMap D0 = new InputMap(Keys.D0);
        public static readonly InputMap D1 = new InputMap(Keys.D1);
        public static readonly InputMap D2 = new InputMap(Keys.D2);
        public static readonly InputMap D3 = new InputMap(Keys.D3);
        public static readonly InputMap D4 = new InputMap(Keys.D4);
        public static readonly InputMap D5 = new InputMap(Keys.D5);
        public static readonly InputMap D6 = new InputMap(Keys.D6);
        public static readonly InputMap D7 = new InputMap(Keys.D7);
        public static readonly InputMap D8 = new InputMap(Keys.D8);
        public static readonly InputMap D9 = new InputMap(Keys.D9);
        public static readonly InputMap A = new InputMap(Keys.A);
        public static readonly InputMap B = new InputMap(Keys.B);
        public static readonly InputMap C = new InputMap(Keys.C);
        public static readonly InputMap D = new InputMap(Keys.D);
        public static readonly InputMap E = new InputMap(Keys.E);
        public static readonly InputMap F = new InputMap(Keys.F);
        public static readonly InputMap G = new InputMap(Keys.G);
        public static readonly InputMap H = new InputMap(Keys.H);
        public static readonly InputMap I = new InputMap(Keys.I);
        public static readonly InputMap J = new InputMap(Keys.J);
        public static readonly InputMap K = new InputMap(Keys.K);
        public static readonly InputMap L = new InputMap(Keys.L);
        public static readonly InputMap M = new InputMap(Keys.M);
        public static readonly InputMap N = new InputMap(Keys.N);
        public static readonly InputMap O = new InputMap(Keys.O);
        public static readonly InputMap P = new InputMap(Keys.P);
        public static readonly InputMap Q = new InputMap(Keys.Q);
        public static readonly InputMap R = new InputMap(Keys.R);
        public static readonly InputMap S = new InputMap(Keys.S);
        public static readonly InputMap T = new InputMap(Keys.T);
        public static readonly InputMap U = new InputMap(Keys.U);
        public static readonly InputMap V = new InputMap(Keys.V);
        public static readonly InputMap W = new InputMap(Keys.W);
        public static readonly InputMap X = new InputMap(Keys.X);
        public static readonly InputMap Y = new InputMap(Keys.Y);
        public static readonly InputMap Z = new InputMap(Keys.Z);
        public static readonly InputMap LeftWindows = new InputMap(Keys.LeftWindows);
        public static readonly InputMap RightWindows = new InputMap(Keys.RightWindows);
        public static readonly InputMap Apps = new InputMap(Keys.Apps);
        public static readonly InputMap Sleep = new InputMap(Keys.Sleep);
        public static readonly InputMap NumPad0 = new InputMap(Keys.NumPad0);
        public static readonly InputMap NumPad1 = new InputMap(Keys.NumPad1);
        public static readonly InputMap NumPad2 = new InputMap(Keys.NumPad2);
        public static readonly InputMap NumPad3 = new InputMap(Keys.NumPad3);
        public static readonly InputMap NumPad4 = new InputMap(Keys.NumPad4);
        public static readonly InputMap NumPad5 = new InputMap(Keys.NumPad5);
        public static readonly InputMap NumPad6 = new InputMap(Keys.NumPad6);
        public static readonly InputMap NumPad7 = new InputMap(Keys.NumPad7);
        public static readonly InputMap NumPad8 = new InputMap(Keys.NumPad8);
        public static readonly InputMap NumPad9 = new InputMap(Keys.NumPad9);
        public static readonly InputMap Multiply = new InputMap(Keys.Multiply);
        public static readonly InputMap Add = new InputMap(Keys.Add);
        public static readonly InputMap Separator = new InputMap(Keys.Separator);
        public static readonly InputMap Subtract = new InputMap(Keys.Subtract);
        public static readonly InputMap Decimal = new InputMap(Keys.Decimal);
        public static readonly InputMap Divide = new InputMap(Keys.Divide);
        public static readonly InputMap F1 = new InputMap(Keys.F1);
        public static readonly InputMap F2 = new InputMap(Keys.F2);
        public static readonly InputMap F3 = new InputMap(Keys.F3);
        public static readonly InputMap F4 = new InputMap(Keys.F4);
        public static readonly InputMap F5 = new InputMap(Keys.F5);
        public static readonly InputMap F6 = new InputMap(Keys.F6);
        public static readonly InputMap F7 = new InputMap(Keys.F7);
        public static readonly InputMap F8 = new InputMap(Keys.F8);
        public static readonly InputMap F9 = new InputMap(Keys.F9);
        public static readonly InputMap F10 = new InputMap(Keys.F10);
        public static readonly InputMap F11 = new InputMap(Keys.F11);
        public static readonly InputMap F12 = new InputMap(Keys.F12);
        public static readonly InputMap F13 = new InputMap(Keys.F13);
        public static readonly InputMap F14 = new InputMap(Keys.F14);
        public static readonly InputMap F15 = new InputMap(Keys.F15);
        public static readonly InputMap F16 = new InputMap(Keys.F16);
        public static readonly InputMap F17 = new InputMap(Keys.F17);
        public static readonly InputMap F18 = new InputMap(Keys.F18);
        public static readonly InputMap F19 = new InputMap(Keys.F19);
        public static readonly InputMap F20 = new InputMap(Keys.F20);
        public static readonly InputMap F21 = new InputMap(Keys.F21);
        public static readonly InputMap F22 = new InputMap(Keys.F22);
        public static readonly InputMap F23 = new InputMap(Keys.F23);
        public static readonly InputMap F24 = new InputMap(Keys.F24);
        public static readonly InputMap NumLock = new InputMap(Keys.NumLock);
        public static readonly InputMap Scroll = new InputMap(Keys.Scroll);
        public static readonly InputMap LeftShift = new InputMap(Keys.LeftShift);
        public static readonly InputMap RightShift = new InputMap(Keys.RightShift);
        public static readonly InputMap LeftControl = new InputMap(Keys.LeftControl);
        public static readonly InputMap RightControl = new InputMap(Keys.RightControl);
        public static readonly InputMap LeftAlt = new InputMap(Keys.LeftAlt);
        public static readonly InputMap RightAlt = new InputMap(Keys.RightAlt);
        public static readonly InputMap BrowserBack = new InputMap(Keys.BrowserBack);
        public static readonly InputMap BrowserForward = new InputMap(Keys.BrowserForward);
        public static readonly InputMap BrowserRefresh = new InputMap(Keys.BrowserRefresh);
        public static readonly InputMap BrowserStop = new InputMap(Keys.BrowserStop);
        public static readonly InputMap BrowserSearch = new InputMap(Keys.BrowserSearch);
        public static readonly InputMap BrowserFavorites = new InputMap(Keys.BrowserFavorites);
        public static readonly InputMap BrowserHome = new InputMap(Keys.BrowserHome);
        public static readonly InputMap VolumeMute = new InputMap(Keys.VolumeMute);
        public static readonly InputMap VolumeDown = new InputMap(Keys.VolumeDown);
        public static readonly InputMap VolumeUp = new InputMap(Keys.VolumeUp);
        public static readonly InputMap MediaNextTrack = new InputMap(Keys.MediaNextTrack);
        public static readonly InputMap MediaPreviousTrack = new InputMap(Keys.MediaPreviousTrack);
        public static readonly InputMap MediaStop = new InputMap(Keys.MediaStop);
        public static readonly InputMap MediaPlayPause = new InputMap(Keys.MediaPlayPause);
        public static readonly InputMap LaunchMail = new InputMap(Keys.LaunchMail);
        public static readonly InputMap SelectMedia = new InputMap(Keys.SelectMedia);
        public static readonly InputMap LaunchApplication1 = new InputMap(Keys.LaunchApplication1);
        public static readonly InputMap LaunchApplication2 = new InputMap(Keys.LaunchApplication2);
        public static readonly InputMap OemSemicolon = new InputMap(Keys.OemSemicolon);
        public static readonly InputMap OemPlus = new InputMap(Keys.OemPlus);
        public static readonly InputMap OemComma = new InputMap(Keys.OemComma);
        public static readonly InputMap OemMinus = new InputMap(Keys.OemMinus);
        public static readonly InputMap OemPeriod = new InputMap(Keys.OemPeriod);
        public static readonly InputMap OemQuestion = new InputMap(Keys.OemQuestion);
        public static readonly InputMap OemTilde = new InputMap(Keys.OemTilde);
        public static readonly InputMap OemOpenBrackets = new InputMap(Keys.OemOpenBrackets);
        public static readonly InputMap OemPipe = new InputMap(Keys.OemPipe);
        public static readonly InputMap OemCloseBrackets = new InputMap(Keys.OemCloseBrackets);
        public static readonly InputMap OemQuotes = new InputMap(Keys.OemQuotes);
        public static readonly InputMap Oem8 = new InputMap(Keys.Oem8);
        public static readonly InputMap OemBackslash = new InputMap(Keys.OemBackslash);
        public static readonly InputMap ProcessKey = new InputMap(Keys.ProcessKey);
        public static readonly InputMap Attn = new InputMap(Keys.Attn);
        public static readonly InputMap Crsel = new InputMap(Keys.Crsel);
        public static readonly InputMap Exsel = new InputMap(Keys.Exsel);
        public static readonly InputMap EraseEof = new InputMap(Keys.EraseEof);
        public static readonly InputMap Play = new InputMap(Keys.Play);
        public static readonly InputMap Zoom = new InputMap(Keys.Zoom);
        public static readonly InputMap Pa1 = new InputMap(Keys.Pa1);
        public static readonly InputMap OemClear = new InputMap(Keys.OemClear);
        public static readonly InputMap ChatPadGreen = new InputMap(Keys.ChatPadGreen);
        public static readonly InputMap ChatPadOrange = new InputMap(Keys.ChatPadOrange);
        public static readonly InputMap Pause = new InputMap(Keys.Pause);
        public static readonly InputMap ImeConvert = new InputMap(Keys.ImeConvert);
        public static readonly InputMap ImeNoConvert = new InputMap(Keys.ImeNoConvert);
        public static readonly InputMap Kana = new InputMap(Keys.Kana);
        public static readonly InputMap Kanji = new InputMap(Keys.Kanji);
        public static readonly InputMap OemAuto = new InputMap(Keys.OemAuto);
        public static readonly InputMap OemCopy = new InputMap(Keys.OemCopy);
        public static readonly InputMap OemEnlW = new InputMap(Keys.OemEnlW);


        //MouseButtons
        public static readonly InputMap MouseLeftButton = new InputMap(MouseButton.Left);
        public static readonly InputMap MouseRightButton = new InputMap(MouseButton.Right);
        public static readonly InputMap MouseMiddleButton = new InputMap(MouseButton.Middle);
        public static readonly InputMap MouseXButton1 = new InputMap(MouseButton.XButton1);
        public static readonly InputMap MouseXButton2 = new InputMap(MouseButton.XButton2);

    }
}
