//#####################################################################################
//★★★★★★★           http://www.cnpopsoft.com [华普软件]           ★★★★★★★
//★★ '华普软件-VB、C#专业论文与源码荟萃，敏捷开发，平台战略，让商业软件靓起来！  ★★
//#####################################################################################

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WZTB
{
    [Serializable]
    public class CustomRectangle
    {
        public CustomRectangle()
        {
        }

        public CustomRectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        #region 属性定义区

        private float x;
        /// <summary>
        /// X坐标
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;
        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// X坐标
        /// </summary>
        public float Left
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Y坐标
        /// </summary>
        public float Top
        {
            get { return y; }
            set { y = value; }
        }

        private float width;
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float height;
        /// <summary>
        /// 高度
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 返回右边X轴坐标（等于X+Width）
        /// </summary>
        public float Right
        {
            get { return x + width; }
        }

        /// <summary>
        /// 返回底部Y轴坐标（等于Y+Height）
        /// </summary>
        public float Bottom
        {
            get { return y + height; }
        }

        /// <summary>
        /// 返回矩形中点
        /// </summary>
        public CustomPoint CenterPoint
        {
            get { return new CustomPoint(x + width / 2, y + height / 2); }
        }

        #endregion

        #region 方法区

        /// <summary>
        /// 返回一个克隆的对象
        /// </summary>
        /// <returns></returns>
        public CustomRectangle Clone()
        {
            return new CustomRectangle(x, y, width, height);
        }

        /// <summary>
        /// 转化为Rectangle对象
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        /// <summary>
        /// 转化为RectangleF对象
        /// </summary>
        /// <returns></returns>
        public RectangleF ToRectangleF()
        {
            return new RectangleF(x, y, width, height);
        }


        /// <summary>
        /// 返回一个左上角坐标点
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// 返回一个左上角坐标点
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF(x, y);
        }

        /// <summary>
        /// 判断指定坐标的点是否在矩形内部
        /// </summary>
        /// <param name="ptX">X坐标</param>
        /// <param name="ptY">Y坐标</param>
        /// <returns></returns>
        public bool IsPointInRectangle(int ptX, int ptY)
        {
            return (ptX >= x && ptX <= (x + width) && ptY >= y && ptY <= (y + height));
        }

        /// <summary>
        /// 判断指定坐标的点是否在矩形内部
        /// </summary>
        /// <param name="ptX">X坐标</param>
        /// <param name="ptY">Y坐标</param>
        /// <returns></returns>
        public bool IsPointFInRectangle(float ptX, float ptY)
        {
            return (ptX >= x && ptX <= (x + width) && ptY >= y && ptY <= (y + height));
        }

        public static CustomRectangle ToCustomRectangle(RectangleF re)
        {
            CustomRectangle cus = new CustomRectangle();
            cus.X = re.X;
            cus.Y = re.Y;
            cus.Width = re.Width;
            cus.Height = re.Height;
            return cus;
        }

        /// <summary>
        /// 根据Rectangle产生一个CustomRectangle对象
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CustomRectangle FromRectangle(Rectangle rect)
        {
            return new CustomRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// 根据RectangleF产生一个CustomRectangle对象
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CustomRectangle FromRectangleF(RectangleF rect)
        {
            return new CustomRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// 获取边框的圆角矩形路径（用于绘图）
        /// </summary>
        /// <returns></returns>
        public GraphicsPath GetRoundRectBorderPath(float radus)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radus * 2, radus * 2, 180, 90);
            path.AddArc(Right - radus * 2, y, radus * 2, radus * 2, 270, 90);
            path.AddArc(Right - radus * 2, Bottom - radus * 2, radus * 2, radus * 2, 0, 90);
            path.AddArc(x, Bottom - radus * 2, radus * 2, radus * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 获取六边形绘图路径
        /// </summary>
        /// <returns></returns>
        public GraphicsPath GetHexagonBorderPath()
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> pts = new List<PointF>();
            pts.Add(new PointF(x + width / 2f, y));
            pts.Add(new PointF(Right, y + height / 4f));
            pts.Add(new PointF(Right, y + height * 3f / 4f));
            pts.Add(new PointF(x + width / 2f, Bottom));
            pts.Add(new PointF(x, y + height * 3f / 4f));
            pts.Add(new PointF(x, y + height / 4f));
            path.AddPolygon(pts.ToArray());
            return path;
        }

        /// <summary>
        /// 获取倒三角形路径
        /// </summary>
        /// <returns></returns>
        public GraphicsPath GetTrianglePath()
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> pts = new List<PointF>();
            pts.Add(new PointF(x, y));
            pts.Add(new PointF(Right, y));
            pts.Add(new PointF(x + width / 2f, Bottom));
            path.AddPolygon(pts.ToArray());
            return path;
        }

        /// <summary>
        /// 根据鼠标位置确定高亮区域，1、2、3、4表示中间4个区域，其他为-1
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public int GetHotPosition(Point pt)
        {
            Point pt1, pt2, pt3, pt4, pt0;
            pt1 = new Point((int)(x + width / 6), (int)(y + height / 6));
            pt2 = new Point((int)(Right - width / 6), (int)(y + height / 6));
            pt3 = new Point((int)(Right - width / 6), (int)(Bottom - height / 6));
            pt4 = new Point((int)(x + width / 6), (int)(Bottom - height / 6));
            pt0 = CenterPoint.ToPoint();

            GraphicsPath path1 = new GraphicsPath();
            path1.AddPolygon(new Point[] { pt1, pt2, pt0 });
            if (path1.IsVisible(pt)) return 1;

            GraphicsPath path2 = new GraphicsPath();
            path2.AddPolygon(new Point[] { pt2, pt3, pt0 });
            if (path2.IsVisible(pt)) return 2;

            GraphicsPath path3 = new GraphicsPath();
            path3.AddPolygon(new Point[] { pt3, pt4, pt0 });
            if (path3.IsVisible(pt)) return 3;

            GraphicsPath path4 = new GraphicsPath();
            path4.AddPolygon(new Point[] { pt4, pt1, pt0 });
            if (path4.IsVisible(pt)) return 4;

            return -1;
        }

        /// <summary>
        /// 获取高亮矩形区域
        /// </summary>
        /// <param name="hotPosition">1,2,3,4；0表示全选</param>
        /// <returns></returns>
        public Rectangle GetSubRectangle(int hotPosition)
        {
            if (hotPosition == 1)
                return new Rectangle((int)x, (int)y, (int)width, (int)(height / 2));
            else if (hotPosition == 2)
                return new Rectangle((int)(x + width / 2), (int)y, (int)(width / 2), (int)height);
            else if (hotPosition == 3)
                return new Rectangle((int)x, (int)(y + height / 2), (int)width, (int)(height / 2));
            else if (hotPosition == 4)
                return new Rectangle((int)x, (int)y, (int)(width / 2), (int)height);
            else
                return ToRectangle();
        }

        #endregion

    }
}
