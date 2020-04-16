using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WallPaperDemo.Util
{
    public static class WinFormUtil
    {
        /// <summary>
        /// 为控件提供ToolTip
        /// </summary>
        /// <param name="control"></param>
        /// <param name="tip"></param>
        /// <param name="message"></param>
        public static void ShowToolTip(this Control control, ToolTip tip, string message)
        {
            Point _mousePoint = Control.MousePosition;
            int _x = control.PointToClient(_mousePoint).X;
            int _y = control.PointToClient(_mousePoint).Y;
            tip.Show(message, control, _x, _y);
            tip.Active = true;
        }

        /// <summary>
        /// 为控件提供ToolTip
        /// </summary>
        /// <param name="control"></param>
        /// <param name="tip"></param>
        /// <param name="message"></param>
        /// <param name="durationTime"></param>
        public static void ShowToolTip(this Control control, ToolTip tip, string message, int durationTime)
        {
            Point _mousePoint = Control.MousePosition;
            int _x = control.PointToClient(_mousePoint).X;
            int _y = control.PointToClient(_mousePoint).Y;
            tip.Show(message, control, _x, _y, durationTime);
            tip.Active = true;
        }

        /// <summary>
        /// 为控件提供ToolTip
        /// </summary>
        /// <param name="control"></param>
        /// <param name="tip"></param>
        /// <param name="message"></param>
        /// <param name="xoffset"></param>
        /// <param name="yoffset"></param>
        public static void ShowToolTip(this Control control, ToolTip tip, string message, int xoffset, int yoffset)
        {
            Point _mousePoint = Control.MousePosition;
            int _x = control.PointToClient(_mousePoint).X;
            int _y = control.PointToClient(_mousePoint).Y;
            tip.Show(message, control, _x + xoffset, _y + yoffset);
            tip.Active = true;
        }

        /// <summary>
        /// 为控件提供ToolTip
        /// </summary>
        /// <param name="control"></param>
        /// <param name="tip"></param>
        /// <param name="message"></param>
        /// <param name="xoffset"></param>
        /// <param name="yoffset"></param>
        /// <param name="durationTime"></param>
        public static void ShowToolTip(this Control control, ToolTip tip, string message, int xoffset, int yoffset, int durationTime)
        {
            Point _mousePoint = Control.MousePosition;
            int _x = control.PointToClient(_mousePoint).X;
            int _y = control.PointToClient(_mousePoint).Y;
            tip.Show(message, control, _x + xoffset, _y + yoffset, durationTime);
            tip.Active = true;
        }

    }
}
