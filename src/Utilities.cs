using Dynamo.Extensions;
using Dynamo.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace Monito
{
    /// <summary>
    /// Shared utility functions for view models
    /// </summary>
    class ViewModelUtils
    {
        private ReadyParams readyParams;
        private DynamoViewModel viewModel;
        private Window dynWindow;
        
        public ViewModelUtils(ReadyParams p, DynamoViewModel vm, Window dw)
        {
            readyParams = p;
            viewModel = vm;
            dynWindow = dw;
        }
        /// <summary>
        /// Zoom in on the object with the given GUID.
        /// </summary>
        public void ZoomToObject(string guid)
        {
            bool isNode = readyParams.CurrentWorkspaceModel.Nodes.Count(x => x.GUID.ToString() == guid) > 0;
            bool isNote = viewModel.Model.CurrentWorkspace.Notes.Count(x => x.GUID.ToString() == guid) > 0;
            bool isAnno = viewModel.Model.CurrentWorkspace.Annotations.Count(x => x.GUID.ToString() == guid) > 0;
            double objectCenterX = 0;
            double objectCenterY = 0;
            double objectWidth = 0;
            double objectHeight = 0;
            if (isNode)
            {
                var zoomNode = readyParams.CurrentWorkspaceModel.Nodes.First(x => x.GUID.ToString() == guid);
                objectCenterX = zoomNode.CenterX;
                objectCenterY = zoomNode.CenterY;
                objectWidth = zoomNode.Rect.TopRight.X - zoomNode.Rect.BottomLeft.X;
                objectHeight = zoomNode.Rect.TopRight.Y - zoomNode.Rect.BottomLeft.Y;
            }
            else if (isNote)
            {
                var zoomNote = viewModel.Model.CurrentWorkspace.Notes.First(x => x.GUID.ToString() == guid);
                objectWidth = zoomNote.Rect.TopRight.X - zoomNote.Rect.BottomLeft.X;
                objectHeight = zoomNote.Rect.TopRight.Y - zoomNote.Rect.BottomLeft.Y;
                objectCenterX = zoomNote.CenterX;
                objectCenterY = zoomNote.CenterY;
            }
            else if (isAnno)
            {
                var zoomAnno = viewModel.Model.CurrentWorkspace.Annotations.First(x => x.GUID.ToString() == guid);
                objectWidth = zoomAnno.Rect.TopRight.X - zoomAnno.Rect.BottomLeft.X;
                objectHeight = zoomAnno.Rect.TopRight.Y - zoomAnno.Rect.BottomLeft.Y;
                objectCenterX = zoomAnno.CenterX;
                objectCenterY = zoomAnno.CenterY;
            }
            var maxZoom = 4d;
            var corrX = -objectCenterX * maxZoom + dynWindow.ActualWidth / (maxZoom / 2) * 1.1;
            var corrY = -objectCenterY * maxZoom + dynWindow.ActualHeight / (maxZoom / 2) * 1.1;
            viewModel.CurrentSpace.Zoom = maxZoom;
            viewModel.CurrentSpace.X = corrX;
            viewModel.CurrentSpace.Y = corrY;
            if (objectCenterX != 0 || objectCenterY != 0) { viewModel.ZoomInCommand.Execute(null); }           
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
            if (str.Length > 60) { str = str.Substring(0, 60) + "..."; }
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
}
