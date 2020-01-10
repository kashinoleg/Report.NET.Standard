using Report.NET.Standard.Base;
using System;
using System.Collections;

namespace Root.Reports
{
    /// <summary>Base class of all report layout managers.</summary>
    public abstract class LayoutManager
    {
        #region Constructor
        /// <summary>Creates a new layout manager.</summary>
        protected LayoutManager(ReportBase report)
        {
            this._report = report;
        }
        #endregion

        #region Properties
        private readonly ReportBase _report;
        /// <summary>Gets the report object to which this layout manager belongs.</summary>
        /// <value>Report object</value>
        /// <remarks>The report object will be set in the constructor.</remarks>
        public ReportBase report
        {
            get { return _report; }
        }
        #endregion

        /// <summary>Sets variable _container to the next container.</summary>
        protected virtual void NextContainer()
        {
        }

        /// <summary>Adds a report object to the current container at the current position.</summary>
        /// <param name="al_RepObj">Result array for the report objects</param>
        /// <param name="repString">Report object to add to the container</param>
        /// <param name="rCurX"></param>
        /// <param name="rOfsX"></param>
        /// <param name="rAlignH">Horizontal allignment</param>
        /// <param name="rCurY"></param>
        /// <param name="rWidth"></param>
        internal void FormatString(ArrayList al_RepObj, RepString repString, ref Double rCurX, Double rOfsX, Double rAlignH,
          ref Double rCurY, Double rWidth)
        {
            FontProp fp = repString.fontProp;
            String sText = repString.sText;

            Int32 iLineStartIndex = 0;
            Int32 iIndex = 0;
            while (true)
            {
                Int32 iLineBreakIndex = 0;
                Double rPosX = rCurX;
                Double rLineBreakPos = 0;
                while (true)
                {
                    if (iIndex >= sText.Length)
                    {
                        iLineBreakIndex = iIndex;
                        rLineBreakPos = rPosX;
                        break;
                    }
                    Char c = sText[iIndex];
                    if (c == '\r')
                    {
                        iLineBreakIndex = iIndex;
                        iIndex++;
                        c = sText[iIndex];
                        if (c == '\n')
                        {
                            iIndex++;
                        }
                        break;
                    }
                    rPosX += fp.rGetTextWidth(Convert.ToString(c));
                    if (rPosX >= rWidth)
                    {
                        if (iLineBreakIndex == 0 && rCurX <= rOfsX + 0.01)
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
                    iIndex++;
                }

                if (iLineStartIndex == 0 && iIndex >= sText.Length)
                {  // add entire object
                    repString.matrixD.rDX = rCurX + (rWidth - rCurX) * rAlignH;
                    repString.rAlignH = rAlignH;
                    repString.matrixD.rDY = rCurY;
                    repString.rAlignV = RepObj.rAlignBottom;
                    al_RepObj.Add(repString);
                    rCurX = rLineBreakPos;
                    break;
                }
                if (iLineBreakIndex > iLineStartIndex && sText[iLineBreakIndex - 1] == ' ')
                {
                    iLineBreakIndex--;
                }
                String sLine = sText.Substring(iLineStartIndex, iLineBreakIndex - iLineStartIndex);
                RepString rs = new RepString(fp, sLine);
                rs.matrixD.rDX = rCurX + (rWidth - rCurX) * rAlignH;
                rs.rAlignH = rAlignH;
                rs.matrixD.rDY = rCurY;
                rs.rAlignV = RepObj.rAlignBottom;
                al_RepObj.Add(rs);
                if (iIndex >= sText.Length)
                {
                    rCurX = rLineBreakPos;
                    break;
                }
                rCurX = rOfsX;
                rCurY += fp.rLineFeed;
                iLineStartIndex = iIndex;
            }
        }

        /// <summary>Adds a report object to the current container at the current position.</summary>
        /// <param name="repString">Report object to add to the container</param>
        /// <param name="container">Container</param>
        /// <param name="rCurX"></param>
        /// <param name="rOfsX"></param>
        /// <param name="rCurY"></param>
        /// <param name="rWidth"></param>
        public void PrintString(RepString repString, Container container, ref Double rCurX, Double rOfsX, ref Double rCurY, Double rWidth)
        {
            FontProp fp = repString.fontProp;
            String sText = repString.sText;

            Int32 iLineStartIndex = 0;
            Int32 iIndex = 0;
            while (true)
            {
                if (rCurY > container.rHeight.Point)
                {
                    NextContainer();
                }
                Int32 iLineBreakIndex = 0;
                Double rPosX = rCurX;
                Double rLineBreakPos = 0;
                while (true)
                {
                    if (iIndex >= sText.Length)
                    {
                        iLineBreakIndex = iIndex;
                        rLineBreakPos = rPosX;
                        break;
                    }
                    Char c = sText[iIndex];
                    rPosX += fp.rGetTextWidth(Convert.ToString(c));
                    if (rPosX >= rWidth)
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
                    container.Add(new UnitModel() { Point = rOfsX + rCurX }, new UnitModel() { Point = rCurY }, repString);
                    rCurX = rLineBreakPos;
                    break;
                }
                String sLine = sText.Substring(iLineStartIndex, iLineBreakIndex - iLineStartIndex);
                container.Add(new UnitModel() { Point = rOfsX + rCurX }, new UnitModel() { Point = rCurY }, new RepString(fp, sLine));
                if (iIndex >= sText.Length)
                {
                    rCurX = rLineBreakPos;
                    break;
                }
                rCurX = 0;
                rCurY += fp.rLineFeed;
                iLineStartIndex = iIndex;
            }
        }
    }
}
