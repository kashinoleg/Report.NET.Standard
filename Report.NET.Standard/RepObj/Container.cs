using Report.NET.Standard.Base;
using System.Collections.Generic;
using System.Diagnostics;

namespace Root.Reports
{
    /// <summary>Container for report objects</summary>
    public abstract class Container : RepObj, IEnumerable<RepObj>
    {
        /// <summary>Array that contains all report objects of this container</summary>
        private readonly List<RepObj> list_RepObj;

        /// <summary>Creates a new container.</summary>
        protected Container()
        {
            list_RepObj = new List<RepObj>(20);
        }

        /// <summary>Creates a new container.</summary>
        public Container(UnitModel rWidth, UnitModel rHeight)
        {
            this.rWidth = rWidth;
            this.rHeight = rHeight;
        }

        /// <summary>This method will be called after the report object has been added to the container.</summary>
        public void Remove(RepObj repObj)
        {
            list_RepObj.Remove(repObj);
            repObj.container = null;
        }

        /// <summary>This method will be called after the report object has been added to the container.</summary>
        internal protected override void OnAdded()
        {
            oRepObjX = report.formatter.oCreate_Container();
        }

        /// <summary>Appends a report object to the container.</summary>
        /// <param name="repObj">Report object that must be added to the container</param>
        internal void Add(RepObj repObj)
        {
            Add(repObj, null);
        }

        /// <summary>Adds a report object to the container.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void Add(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            repObj.matrixD.rDX = rX.Point;
            repObj.matrixD.rDY = rY.Point;
            Add(repObj);
        }

        /// <summary>Adds a report object to the container.</summary>
        /// <param name="repObj">Report object that must be added to the container</param>
        /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
        /// If the position is null it will be appended to the end of the list.</param>
        internal void Add(RepObj repObj, RepObj repObj_Pos)
        {
            if (repObj.container != null)
            {
                throw new ReportException("Report objects cannot be added to more than one container");
            }
            if (ReferenceEquals(this, repObj))
            {
                throw new ReportException("Report objects cannot be added to itself");
            }

            if (repObj_Pos == null)
            {
                list_RepObj.Add(repObj);
            }
            else
            {
                if (!ReferenceEquals(repObj_Pos.container, this))
                {
                    throw new ReportException("The report object indicating the position is not a member of the container");
                }
                int iIndex = list_RepObj.IndexOf(repObj_Pos);
                Debug.Assert(iIndex >= 0);
                list_RepObj.Insert(iIndex, repObj);
            }
            repObj.container = this;
            repObj.OnAdded();
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
        /// <param name="repObj">Report object to add to the container</param>
        /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
        /// If the position is null it will be appended to the end of the list.</param>
        internal void AddAligned(UnitModel rX, double rAlignH, UnitModel rY, double rAlignV, RepObj repObj, RepObj repObj_Pos)
        {
            repObj.matrixD.rDX = rX.Point;
            repObj.rAlignH = rAlignH;
            repObj.matrixD.rDY = rY.Point;
            repObj.rAlignV = rAlignV;
            Add(repObj, repObj_Pos);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddAligned(UnitModel rX, double rAlignH, UnitModel rY, double rAlignV, RepObj repObj)
        {
            AddAligned(rX, rAlignH, rY, rAlignV, repObj, null);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
        /// If the position is null it will be appended to the end of the list.</param>
        public void AddLT(UnitModel rX, UnitModel rY, RepObj repObj, RepObj repObj_Pos)
        {
            AddAligned(rX, rAlignLeft, rY, rAlignTop, repObj, repObj_Pos);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddLT(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddLT(rX, rY, repObj, null);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddCT(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, rAlignCenter, rY, rAlignTop, repObj);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddCC(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, rAlignCenter, rY, rAlignCenter, repObj);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddCB(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, rAlignCenter, rY, rAlignBottom, repObj);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddRT(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, rAlignRight, rY, rAlignTop, repObj);
        }

        /// <summary>Adds a report object to the container and sets the alignment.</summary>
        /// <param name="rX">X-coordinate of the report object</param>
        /// <param name="rY">Y-coordinate of the report object</param>
        /// <param name="repObj">Report object to add to the container</param>
        internal void AddRC(UnitModel rX, UnitModel rY, RepObj repObj)
        {
            AddAligned(rX, rAlignRight, rY, rAlignCenter, repObj);
        }

        #region IEnumerable Members
        /// <summary>Returns an enumerator that can iterate through the report objects.</summary>
        /// <returns>An enumerator that can be used to iterate through the report objects</returns>
        public IEnumerator<RepObj> GetEnumerator() => list_RepObj.GetEnumerator();

        /// <summary>Returns an enumerator that can iterate through the report objects.</summary>
        /// <returns>An enumerator that can be used to iterate through the report objects</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list_RepObj.GetEnumerator();
        }
        #endregion
    }
}
