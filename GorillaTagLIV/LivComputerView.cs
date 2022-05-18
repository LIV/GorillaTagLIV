using System;
using Bepinject;
using ComputerInterface;
using ComputerInterface.Interfaces;
using ComputerInterface.ViewLib;
using UnityEngine;
using Zenject;

namespace GorillaTagLIV
{
    public class LivComputerView : ComputerView
    {
        private const string highlightColor = "52fc03";
        private readonly UISelectionHandler selectionHandler;

        public LivComputerView()
        {
            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.MaxIdx = 3;
            selectionHandler.OnSelected += OnEntrySelected;
            selectionHandler.ConfigureSelectionIndicator($"<color=#{highlightColor}>></color> ", "", "  ", "");
        }

        private void OnEntrySelected(int index)
        {
            if (index == 0)
            {
                Plugin.Instance.ShowGorillaBody.Value = !Plugin.Instance.ShowGorillaBody.Value;
            }
            UpdateScreen();
        }
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            UpdateScreen();
        }
        
        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key))
            {
                UpdateScreen();
                return;
            }
            
            switch (key)
            {
                case EKeyboardKey.Back:
                    ReturnToMainMenu();
                    break;
            }
        }

        private void UpdateScreen()
        {
            SetText(str =>
            {
                str.BeginCenter();
                str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
                str.AppendClr("LIV", highlightColor).EndColor().AppendLine();
                str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
                str.EndAlign().AppendLines(1);
                str.AppendLine(selectionHandler.GetIndicatedText(0, $"Show Gorilla Body: <color={(Plugin.Instance.ShowGorillaBody.Value ? $"#{highlightColor}>Yes" : "white>No")}</color>"));
                str.AppendLines(2);
                str.AppendClr("Press ENTER to toggle setting", "ffffff10").EndColor().AppendLine();
            });
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