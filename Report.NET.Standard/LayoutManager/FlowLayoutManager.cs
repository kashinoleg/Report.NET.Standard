using Report.NET.Standard.Base;
using System;
using System.Diagnostics;

namespace Root.Reports
{
    /// <summary>Flow Layout Manager</summary>
    public class FlowLayoutManager : LayoutManager
    {
        /// <summary>Current horizontal position</summary>
        public UnitModel rX_Cur = new UnitModel();

        /// <summary>Current vertical position</summary>
        public UnitModel rY_Cur = new UnitModel();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        /// <summary>Status of the layout manager</summary>
        private enum Status
        {
            /// <summary>Initialization mode</summary>
            Init,
            /// <summary>Table open</summary>
            Open,
            /// <summary>Container closed</summary>
            Closed
        }

        /// <summary>Status of the layout manager</summary>
        private Status status = Status.Init;

        /// <summary>Creates a new flow layout manager.</summary>
        public FlowLayoutManager() : base(null)
        {
        }

        /// <summary>Creates a new flow layout manager.</summary>
        /// <param name="container">Container that must be bound to this layout manager</param>
        public FlowLayoutManager(Container container) : this()
        {
            this._container_Cur = container;
        }

        /// <summary>Adds a report object to the current container at the current position.</summary>
        /// <param name="repObj">Report object to add to the container</param>
        public void Add(RepObj repObj)
        {
            if (status == Status.Init)
            {
                if (_container_Cur == null)
                {
                    CreateNewContainer();
                }
            }

            if (repObj is RepString)
            {
                var repString = (RepString)repObj;
                var fp = repString.fontProp;
                var sText = repString.sText;

                Int32 iLineStartIndex = 0;
                Int32 iIndex = 0;
                while (true)
                {
                    if (rY_Cur.Point > container_Cur.rHeight.Point)
                    {
                        _container_Cur = null;
                        CreateNewContainer();
                    }
                    var iLineBreakIndex = 0;
                    Double rPosX = rX_Cur.Point;
                    Double rLineBreakPos = 0;
                    while (true)
                    {
                        if (iIndex >= sText.Length)
                        {
                            iLineBreakIndex = iIndex;
                            rLineBreakPos = rPosX;
                            break;
                        }
                        var c = sText[iIndex];
                        rPosX += fp.rGetTextWidth(Convert.ToString(c));
                        if (rPosX >= container_Cur.rWidth.Point)
                        {
                            if (iLineBreakIndex == 0)
                            {
                                if (iIndex == iLineStartIndex)
                                {  // at least one character must be printed
                                    iIndex++;
                                }
                                iLineBreakIndex = iIndex;
                                break;
                            }
                            iIndex = iLineBreakIndex;
                            break;
                        }
                        if (c == ' ')
                        {
                            iLineBreakIndex = iIndex + 1;
                            rLineBreakPos = rPosX;
                        }
                        else if (c == '\n')
                        {
                            iLineBreakIndex = iIndex;
                            iIndex++;
                            break;
                        }
                        iIndex++;
                    }

                    if (iLineStartIndex == 0 && iIndex >= sText.Length)
                    {  // add entire object
                        container_Cur.Add(rX_Cur, rY_Cur, repObj);
                        rX_Cur.Point = rLineBreakPos;
                        break;
                    }
                    var sLine = sText.Substring(iLineStartIndex, iLineBreakIndex - iLineStartIndex);
                    container_Cur.Add(rX_Cur, rY_Cur, new RepString(fp, sLine));
                    if (iIndex >= sText.Length)
                    {
                        rX_Cur.Point = rLineBreakPos;
                        break;
                    }
                    rX_Cur = new UnitModel();
                    rY_Cur.Point += fp.rLineFeed;
                    iLineStartIndex = iIndex;
                }
            }
            else
            {
                Debug.Fail("Unknown object type");
                container_Cur.Add(rX_Cur, rY_Cur, repObj);
            }
        }

        /// <summary>Adds a report object to the current container on a new line.</summary>
        /// <param name="repString">Report object to add to the container</param>
        public void AddNew(RepString repString)
        {
            NewLine(new UnitModel() { Point = repString.fontProp.rLineFeed });
            Add(repString);
        }

        /// <summary>Makes a new line.</summary>
        /// <param name="rLineFeed">Line feed</param>
        public void NewLine(UnitModel rLineFeed)
        {
            rX_Cur = new UnitModel();
            if (rY_Cur.Point + rLineFeed.Point > container_Cur.rHeight.Point)
            {
                _container_Cur = null;
                CreateNewContainer();
            }
            rY_Cur.Point += rLineFeed.Point;
        }

        #region Container
        /// <summary>Default height of the container (points, 1/72 inch)</summary>
        public UnitModel rContainerHeight { get; set; }

        /// <summary>Default width of the container (points, 1/72 inch)</summary>
        public UnitModel rContainerWidth { get; set; }

        private Container _container_Cur;
        /// <summary>Current container</summary>
        public Container container_Cur
        {
            get { return _container_Cur; }
        }

        /// <summary>Provides data for the NewContainer event</summary>
        public class NewContainerEventArgs : EventArgs
        {
            /// <summary>Flow layout manager</summary>
            public readonly FlowLayoutManager flowLayoutManager;

            /// <summary>New container</summary>
            public readonly Container container;

            /// <summary>Creates a data object for the NewContainer event.</summary>
            /// <param name="flowLayoutManager">Handle of the flow layout manager</param>
            /// <param name="container">New container: this container must be added to a page or a container.</param>
            internal NewContainerEventArgs(FlowLayoutManager flowLayoutManager, Container container)
            {
                this.flowLayoutManager = flowLayoutManager;
                this.container = container;
            }
        }

        /// <summary>Represents the method that will handle the NewContainer event.</summary>
        public delegate void NewContainerEventHandler(Object oSender, NewContainerEventArgs ea);

        /// <summary>Occurs when a new container must be created.</summary>
        public event NewContainerEventHandler eNewContainer;

        /// <summary>Raises the NewContainer event.</summary>
        /// <param name="ea">Event argument</param>
        internal protected virtual void OnNewContainer(NewContainerEventArgs ea)
        {
            if (eNewContainer != null)
            {
                eNewContainer(this, ea);
            }
        }

        /// <summary>Creates a new container.</summary>
        private void CreateNewContainer()
        {
            if (_container_Cur == null)
            {
                _container_Cur = new StaticContainer(rContainerWidth, rContainerHeight);
                NewContainerEventArgs ea = new NewContainerEventArgs(this, _container_Cur);
                OnNewContainer(ea);
            }
            rX_Cur = new UnitModel();
            rY_Cur = new UnitModel();
        }

        /// <summary>This method will create a new container that will be added to the parent container at the specified position.</summary>
        /// <param name="container_Parent">Parent container</param>
        /// <param name="rX">X-coordinate of the new container (points, 1/72 inch)</param>
        /// <param name="rY">Y-coordinate of the new container (points, 1/72 inch)</param>
        /// <exception cref="ReportException">The layout manager status is not 'Init'</exception>
        public Container container_Create(Container container_Parent, UnitModel rX, UnitModel rY)
        {
            //      if (status != Status.Init && status != Status.Closed) {
            //        throw new ReportException("The layout manager must be in initialization mode or it must be closed; cannot create a new container.");
            //      }
            if (_container_Cur != null)
            {
                throw new ReportException("The container has been defined already.");
            }
            CreateNewContainer();
            if (container_Parent != null)
            {
                container_Parent.Add(rX, rY, _container_Cur);
            }
            return _container_Cur;
        }
        #endregion
    }
}
