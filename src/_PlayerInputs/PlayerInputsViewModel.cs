using System;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dynamo.Controls;
using System.Windows;
using Dynamo.UI.Commands;
using System.Windows.Input;

namespace Monito
{
    class PlayerInputsViewModel : NotificationObject, IDisposable
    {
        private ReadyParams readyParams;
        private DynamoViewModel viewModel;
        private Window dynWindow;
        public ICommand ResetAll { get; set; }
        public ICommand ResetSelected { get; set; }
        public ICommand SetSelectedAsInput { get; set; }

        public PlayerInputsViewModel(ReadyParams p, DynamoViewModel vm, Window dw)
        {
            readyParams = p;
            viewModel = vm;
            dynWindow = dw;
            p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodesChanged;
            p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodesChanged;
            ResetAll = new DelegateCommand(OnResetAllClicked);
            ResetSelected = new DelegateCommand(OnResetSelectedClicked);
            SetSelectedAsInput = new DelegateCommand(OnSetSelectedAsInputClicked);
        }

        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodesChanged;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodesChanged;
        }

        private string currentInputsMsg;
        public string CurrentInputsMsg
        {
            get
            {
                if (currentInputs.Count > 0) { currentInputsMsg = "All Dynamo Player inputs in current workspace:"; }
                else { currentInputsMsg = "No Dynamo Player inputs in current workspace..."; }
                return currentInputsMsg;
            }
        }

        private ObservableCollection<ObjectInWorkspace> currentInputs = new ObservableCollection<ObjectInWorkspace>();
        /// <summary>
        /// The search results as a list representation
        /// </summary>
        public ObservableCollection<ObjectInWorkspace> CurrentInputs
        {
            get
            {
                List<ObjectInWorkspace> unorderedInputs = new List<ObjectInWorkspace>();
                foreach (NodeModel node in readyParams.CurrentWorkspaceModel.Nodes)
                {
                    if (node.IsSetAsInput) { unorderedInputs.Add(new ObjectInWorkspace(node.NickName, node.GUID.ToString())); }
                }
                currentInputs.Clear();
                foreach (ObjectInWorkspace item in unorderedInputs.OrderBy(x => x.Name)) { currentInputs.Add(item); }
                RaisePropertyChanged(nameof(CurrentInputsMsg));
                return currentInputs;
            }
        }

        public void OnResetAllClicked(object obj)
        {
            foreach (NodeModel node in readyParams.CurrentWorkspaceModel.Nodes)
            {
                if (node.IsSetAsInput) { node.IsSetAsInput = false; }
            }
            RaisePropertyChanged(nameof(CurrentInputs));
        }

        public void OnResetSelectedClicked(object obj)
        {
            foreach (var item in readyParams.CurrentWorkspaceModel.CurrentSelection)
            {
                if (item.IsSetAsInput) { item.IsSetAsInput = false; }
            }
            RaisePropertyChanged(nameof(CurrentInputs));
        }

        public void OnSetSelectedAsInputClicked(object obj)
        {
            foreach (var item in readyParams.CurrentWorkspaceModel.CurrentSelection)
            {
                if (!item.IsSetAsInput) { item.IsSetAsInput = true; }
            }
            RaisePropertyChanged(nameof(CurrentInputs));
        }

        private string highlightGUID;
        /// <summary>
        /// The GUID of the node that should be highlighted
        /// </summary>
        public string HighlightGUID
        {
            get
            {
                return highlightGUID;
            }
            set
            {
                highlightGUID = value;
                foreach (var nv in dynWindow.FindVisualChildren<NodeView>())
                {
                    NodeModel node = nv.DataContext as NodeModel;
                    if (node.GUID.ToString() == value)
                    {
                        MessageBox.Show(node.NickName);
                    }
                }
            }
        }

        private string unhighlightGUID;
        /// <summary>
        /// The GUID of the node that should no longer be highlighted
        /// </summary>
        public string UnhighlightGUID
        {
            get
            {
                return unhighlightGUID;
            }
            set
            {
                unhighlightGUID = value;
            }
        }

        private void CurrentWorkspaceModel_NodesChanged(NodeModel obj)
        {
            RaisePropertyChanged(nameof(CurrentInputs));
        }
    }
}