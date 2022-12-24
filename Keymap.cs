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
        public string key1;
        public string key2;
        public string key3;
        public string key4;
        public string key5;
        public string key6;
        public string window;
        public int delay;
        public int modkey1;
        public int modkey2;
        public int row;

        Guid id;
        public Keymap(string key1, string key2, string key3, string key4, string key5, string key6,string window,int row)
        {
            this.key1 = key1;
            this.key2 = key2;
            this.key3 = key3;
            this.key4 = key4;
            this.key5 = key5;
            this.key6 = key6;
            this.window = window;
            this.row = row;
            delay = 150;
        }
        public void Register()
        {
            Console.WriteLine("KM: REGISTERING");
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
                    if(!MainPage.mainPageInstance.currentWindowName.Contains("KeyRemap"))
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
                this.id = MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey(new[] { (KeyboardHookLibrary.ModifierKeys)modkey1, (KeyboardHookLibrary.ModifierKeys)modkey2 }, KeyDictionary.keyReversed[key3], () =>
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
        }
        public void Unregister()
        {
            if (id != null)
            {
                MainPage.mainPageInstance.keyboardHookManager.UnregisterHotkey(id);
            }
        }

    }
}
