﻿using System;
using Bepinject;
using ComputerInterface;
using ComputerInterface.Interfaces;
using ComputerInterface.ViewLib;
using Zenject;

namespace GorillaTagLIV
{
    public class LivComputerView : ComputerView
    {
        public static bool ShowGorillaBody { get; private set; }
        
        // This is called when you view is opened
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            UpdateText();
        }
        
        public override void OnKeyPressed(EKeyboardKey key)
        {
            switch (key)
            {
                case EKeyboardKey.Option1:
                    ToggleShowGorillaBody();
                    break;
            }
        }

        private void ToggleShowGorillaBody()
        {
            ShowGorillaBody = !ShowGorillaBody;
            UpdateText();
        }

        private void UpdateText()
        {
            Text = $@"LIV Support for Gorilla Tag

Gorilla body is {(ShowGorillaBody ? "" : "NOT")} VISIBLE

Press Option1 to toggle.";
        }
    }
    
    public class LivModComputerEntry : IComputerModEntry
    {
        public string EntryName => "LIV";

        public Type EntryViewType => typeof(LivComputerView);
    }
    
    internal class LivModComputerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<LivModComputerEntry>().AsSingle();
        }

        public static void Install()
        {
            Zenjector.Install<LivModComputerInstaller>().OnProject();
        }
    }
}