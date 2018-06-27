﻿using System.Windows.Controls;
using Dynamo.Wpf.Extensions;
using Dynamo.ViewModels;

namespace Monito
{
    /// <summary>
    /// The main class of this view extension.
    /// </summary>
    public class MonitoViewExtension : IViewExtension
    {
        private MenuItem monitoMenuItem;
        private MenuItem monitoPlaygroundMenuItem;
        private MenuItem monitoPlayerInputsMenuItem;
        private MenuItem monitoSearchInWorkspaceMenuItem;
        private MenuItem monitoAboutMenuItem;

        public void Dispose() { }

        public void Startup(ViewStartupParams p) { }

        public void Loaded(ViewLoadedParams p)
        {
            monitoMenuItem = new MenuItem { Header = "DynaMonito" };
            var VM = p.DynamoWindow.DataContext as DynamoViewModel;

            #region PLAYGROUND
            monitoPlaygroundMenuItem = new MenuItem { Header = "Playground" };
            monitoPlaygroundMenuItem.Click += (sender, args) =>
            {
                var viewModel = new PlaygroundViewModel(p, VM, p.DynamoWindow);
                var window = new PlaygroundWindow
                {
                    PlaygroundPanel = { DataContext = viewModel },
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            //monitoMenuItem.Items.Add(monitoPlaygroundMenuItem);
            #endregion PLAYGROUND

            #region PLAYER_INPUTS
            monitoPlayerInputsMenuItem = new MenuItem { Header = "Manage Dynamo Player Inputs" };
            monitoPlayerInputsMenuItem.ToolTip = new ToolTip { Content = "Manage which input nodes should be displayed by Dynamo Player..." };
            monitoPlayerInputsMenuItem.Click += (sender, args) =>
            {
                var viewModel = new PlayerInputsViewModel(p, VM, p.DynamoWindow);
                var window = new PlayerInputsWindow
                {
                    playerInputsPanel = { DataContext = viewModel },
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            monitoMenuItem.Items.Add(monitoPlayerInputsMenuItem);
            #endregion PLAYER INPUTS

            #region SEARCH_IN_WORKSPACE
            monitoSearchInWorkspaceMenuItem = new MenuItem { Header = "Search in Workspace" };
            monitoSearchInWorkspaceMenuItem.ToolTip = new ToolTip { Content = "Search for nodes, notes and groups in the current workspace..." };
            monitoSearchInWorkspaceMenuItem.Click += (sender, args) =>
            {
                var viewModel = new SearchInWorkspaceViewModel(p, VM, p.DynamoWindow);
                var window = new SearchInWorkspaceWindow
                {
                    searchPanel = { DataContext = viewModel },
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            monitoMenuItem.Items.Add(monitoSearchInWorkspaceMenuItem);
            #endregion SEARCH_IN_WORKSPACE

            #region ABOUT
            monitoAboutMenuItem = new MenuItem { Header = "About DynaMonito" };
            monitoAboutMenuItem.Click += (sender, args) =>
            {
                var window = new AboutWindow
                {
                    aboutPanel = { DataContext = this },
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            monitoMenuItem.Items.Add(new Separator());
            monitoMenuItem.Items.Add(monitoAboutMenuItem);
            #endregion ABOUT

            p.dynamoMenu.Items.Add(monitoMenuItem);
        }

        public void Shutdown() { }

        public string UniqueId
        {
            get
            {
                return "d8fcfe56-81e0-4e95-84af-d945ebd6478b";
            }
        }

        public string Name
        {
            get
            {
                return "DynaMonito";
            }
        }

    }
}