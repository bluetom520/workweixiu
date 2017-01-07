
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WZTB
{
    [Serializable]
    public class CustomPoint
    {
        public CustomPoint()
        {
            this.x = 0;
            this.y = 0;
        }

        public CustomPoint(int x, int y)
        {
            this.x = (float)x;
            this.y = (float)y;
        }

        public CustomPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        #region 属性

        private float x;
        /// <summary>
        /// 点的X坐标
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;
        /// <summary>
        /// 点的Y坐标
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private Color backColor = Color.FromArgb(0, 255, 0);
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        private bool selected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// 该点对应的矩形区域（+-4像素）
        /// </summary>
        public CustomRectangle PositionRect
        {
            get { return new CustomRectangle(x - 3, y - 3, 6, 6); }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 返回一个克隆的对象
        /// </summary>
        /// <returns></returns>
        public CustomPoint Clone()
        {
            CustomPoint pt = new CustomPoint(x, y);
            pt.backColor = backColor;
            return pt;
        }

        /// <summary>
        /// 绘制该控制点
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            DrawBorderHandler(g, PositionRect, backColor);
            if (selected)
            {
                //选中时边框变粗
                CustomRectangle rect = new CustomRectangle(x - 4, y - 4, 8, 8);
                g.DrawRectangle(new Pen(Color.Black), rect.ToRectangle());
            }
        }

        /// <summary>
        /// 绘制菱形控制点
        /// </summary>
        /// <param name="g"></param>
        public void DrawDiamond(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.Default;
            PointF[] pts = new PointF[] { new PointF(x, y - 4), new PointF(x - 4, y), new PointF(x, y + 4), new PointF(x + 4, y) };
            g.FillPolygon(new SolidBrush(backColor), pts);
            g.DrawPolygon(new Pen(Color.Black), pts);
            g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <summary>
        /// 绘制边框句柄
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void DrawBorderHandler(Graphics g, CustomRectangle rect, Color clr)
        {
            g.FillRectangle(new SolidBrush(clr), rect.ToRectangleF());
            g.DrawRectangle(new Pen(Color.Black), rect.ToRectangle());
        }

        /// <summary>
        /// 鼠标点击测试，看是否在点附近点击
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool MouseHitTest(PointF pt)
        {
            CustomRectangle rect = new CustomRectangle(x - 5, y - 5, 10, 10);   //判断选中时加点误差，使选中更加顺利
            return rect.IsPointFInRectangle(pt.X, pt.Y);
        }

        /// <summary>
        /// 从一个Point对象构造CustomPoint对象
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static CustomPoint FromPoint(Point pt)
        {
            return new CustomPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// 从一个Point对象构造CustomPoint对象
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static CustomPoint FromPointF(PointF pt)
        {
            return new CustomPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// 转化为Point对象
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// 转换为PointF对象
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF(x, y);
        }

        #endregion
    }
}
