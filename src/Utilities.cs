using Dynamo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Monito
{
    /// <summary>
    /// Shared utility functions for view models
    /// </summary>
    class ViewModelUtils
    {
        private DynamoViewModel viewModel;
        private Window dynWindow;
        
        public ViewModelUtils(DynamoViewModel vm, Window dw)
        {
            viewModel = vm;
            dynWindow = dw;
        }
        /// <summary>
        /// Zoom in on the object with the given GUID.
        /// </summary>
        public void ZoomToObject(string guid)
        {
            bool isNode = viewModel.Model.CurrentWorkspace.Nodes.Count(x => x.GUID.ToString() == guid) > 0;
            bool isNote = viewModel.Model.CurrentWorkspace.Notes.Count(x => x.GUID.ToString() == guid) > 0;
            bool isAnno = viewModel.Model.CurrentWorkspace.Annotations.Count(x => x.GUID.ToString() == guid) > 0;
            double objectCenterX = 0;
            double objectCenterY = 0;
            if (isNode)
            {
                var zoomNode = viewModel.Model.CurrentWorkspace.Nodes.First(x => x.GUID.ToString() == guid);
                objectCenterX = zoomNode.CenterX;
                objectCenterY = zoomNode.CenterY;
            }
            else if (isNote)
            {
                var zoomNote = viewModel.Model.CurrentWorkspace.Notes.First(x => x.GUID.ToString() == guid);
                objectCenterX = zoomNote.CenterX;
                objectCenterY = zoomNote.CenterY;
            }
            else if (isAnno)
            {
                var zoomAnno = viewModel.Model.CurrentWorkspace.Annotations.First(x => x.GUID.ToString() == guid);
                objectCenterX = zoomAnno.CenterX;
                objectCenterY = zoomAnno.CenterY;
            }
            var maxZoom = 4d;
            var corrX = -objectCenterX * maxZoom + dynWindow.ActualWidth / 2.2;
            var corrY = -objectCenterY * maxZoom + dynWindow.ActualHeight / 2.2;
            viewModel.CurrentSpace.Zoom = maxZoom;
            viewModel.CurrentSpace.X = corrX;
            viewModel.CurrentSpace.Y = corrY;
            if (objectCenterX != 0 || objectCenterY !=0)
            {
                viewModel.ZoomInCommand.Execute(null);
            }           
        }
    }

    /// <summary>
    /// Shared general utility functions
    /// </summary>
    static class GeneralUtils
    {
        public static string Abbreviate(this String str)
        {
            str = str.Replace(Environment.NewLine, " ");
            if (str.Length > 60)
            {
                str = str.Substring(0, 60) + "...";
            }
            return str;
        }
    }

    /// <summary>
    /// Class for structured storage of objects in workspace (nodes, text notes, groups).
    /// </summary>
    class ObjectInWorkspace
    {
        private string objectName;
        private string objectGUID;
        private double objectScore;
        private string objectDetails;

        public ObjectInWorkspace(string name, string guid, double score = 0, string details = "")
        {
            objectName = name;
            objectGUID = guid;
            objectScore = score;
            objectDetails = details;
        }

        public string Name
        {
            get { return objectName; }
        }

        public string GUID
        {
            get { return objectGUID; }
        }

        public double Score
        {
            get { return objectScore; }
        }

        public string Details
        {
            get { return objectDetails; }
        }
    }

    public static class WPFextensions
    {
        // https://stackoverflow.com/questions/10279092/how-to-get-children-of-a-wpf-container-by-type
        public static T GetChildOfType<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
        // https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type/978352
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
