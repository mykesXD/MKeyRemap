using InputSimulatorStandard.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyRemap
{
    public class Keymap
    {
        public string key1 { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string key4 { get; set; }
        public string key5 { get; set; }
        public string key6 { get; set; }
        public string window { get; set; }
        public string keyMap1 { get; set; }
        public string keyMap2 { get; set; }

        public string icon { get; set; }
        public int row { get; set; }

        Guid id;
        public Keymap(string key1, string key2, string key3, string key4, string key5, string key6,string keyMap1,string keyMap2,string window,string icon, int row)
        {
            this.key1 = key1;
            this.key2 = key2;
            this.key3 = key3;
            this.key4 = key4;
            this.key5 = key5;
            this.key6 = key6;
            this.keyMap1 = keyMap1;
            this.keyMap2 = keyMap2;
            this.window = window;
            this.icon = icon;
            this.row = row;
        }
        public void Register()
        {
            int delay = 150;
            int modkey1;
            int modkey2;
            Console.WriteLine("KM: REGISTERING: {0}", window);
            if (key1.Contains("ALT"))
            {
                modkey1 = 1;
            }
            else if (key1.Contains("CTRL"))
            {
                modkey1 = 2;
            }
            else if (key1.Contains("SHIFT"))
            {
                modkey1 = 4;
            }
            else if (key1.Contains("WIN"))
            {
                modkey1 = 8;
            }
            else
            {
                modkey1 = 0;
            }
            if (key2.Contains("ALT"))
            {
                modkey2 = 1;
            }
            else if (key2.Contains("CTRL"))
            {
                modkey2 = 2;
            }
            else if (key2.Contains("SHIFT"))
            {
                modkey2 = 4;
            }
            else if (key2.Contains("WIN"))
            {
                modkey2 = 8;
            }
            else
            {
                modkey2 = 0;
            }
            if (modkey1 == 0 && modkey2 == 0)
            {
                id = MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey(KeyDictionary.keyReversed[key3], () =>
                {
                    if (!MainPage.mainPageInstance.currentWindowName.Contains("KeyRemap"))
                    {
                        if (key4 == " " && key5 == " ")
                        {
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        }
                        else if (key4 != " " && key5 == " ")
                        {
                            Thread.Sleep(delay);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        }
                        else if (key4 == " " && key5 != " ")
                        {
                            Thread.Sleep(delay);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        }
                        else if (key4 != " " && key5 != " ")
                        {
                            Thread.Sleep(delay);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                            MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        }
                    }
                    else
                    {
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key3]);
                    }
                }, true);

            }
            else if (modkey2 == 0 && modkey1 > 0)
            {
                id = MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey((KeyboardHookLibrary.ModifierKeys)modkey1, KeyDictionary.keyReversed[key3], () =>
                {
                    Console.WriteLine("WORKING1");
                    if (key4 == " " && key5 == " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                    }
                    else if (key4 != " " && key5 == " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                    }
                    else if (key4 == " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }
                    else if (key4 != " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }
                }, true);
            }
            else if (modkey1 == 0 && modkey2 > 0)
            {
                id = MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey((KeyboardHookLibrary.ModifierKeys)modkey2, KeyDictionary.keyReversed[key3], () =>
                {
                    Console.WriteLine("WORKING");
                    Console.WriteLine(modkey2);
                    if (key4 == " " && key5 == " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                    }
                    else if (key4 != " " && key5 == " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                    }
                    else if (key4 == " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }
                    else if (key4 != " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }

                }, true);
            }
            else if (modkey1 > 0 && modkey2 > 0)
            {
                id = MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey(new[] { (KeyboardHookLibrary.ModifierKeys)modkey1, (KeyboardHookLibrary.ModifierKeys)modkey2 }, KeyDictionary.keyReversed[key3], () =>
                {
                    Console.WriteLine("2 detected");
                    if (key4 == " " && key5 == " ")
                    {
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                    }
                    else if (key4 != " " && key5 == " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                    }
                    else if (key4 == " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }
                    else if (key4 != " " && key5 != " ")
                    {
                        Thread.Sleep(delay);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyDown((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyReversed[key6]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key4]);
                        MainPage.mainPageInstance.keySimulator.Keyboard.KeyUp((VirtualKeyCode)KeyDictionary.keyReversed[key5]);
                    }
                }, true);
            }
            Console.WriteLine("KM: ID: {0}", id);
        }
        public void Unregister() 
        {
            Console.WriteLine("KM: UNREGISTERING: {0},{1}",window ,id);
            MainPage.mainPageInstance.keyboardHookManager.UnregisterHotkey(id);
        }

    }
}
