
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

        #region ����

        private float x;
        /// <summary>
        /// ���X����
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;
        /// <summary>
        /// ���Y����
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private Color backColor = Color.FromArgb(0, 255, 0);
        /// <summary>
        /// ����ɫ
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        private bool selected = false;
        /// <summary>
        /// �Ƿ�ѡ��
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// �õ��Ӧ�ľ�������+-4���أ�
        /// </summary>
        public CustomRectangle PositionRect
        {
            get { return new CustomRectangle(x - 3, y - 3, 6, 6); }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����һ����¡�Ķ���
        /// </summary>
        /// <returns></returns>
        public CustomPoint Clone()
        {
            CustomPoint pt = new CustomPoint(x, y);
            pt.backColor = backColor;
            return pt;
        }

        /// <summary>
        /// ���Ƹÿ��Ƶ�
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            DrawBorderHandler(g, PositionRect, backColor);
            if (selected)
            {
                //ѡ��ʱ�߿���
                CustomRectangle rect = new CustomRectangle(x - 4, y - 4, 8, 8);
                g.DrawRectangle(new Pen(Color.Black), rect.ToRectangle());
            }
        }

        /// <summary>
        /// �������ο��Ƶ�
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
        /// ���Ʊ߿���
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void DrawBorderHandler(Graphics g, CustomRectangle rect, Color clr)
        {
            g.FillRectangle(new SolidBrush(clr), rect.ToRectangleF());
            g.DrawRectangle(new Pen(Color.Black), rect.ToRectangle());
        }

        /// <summary>
        /// ��������ԣ����Ƿ��ڵ㸽�����
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool MouseHitTest(PointF pt)
        {
            CustomRectangle rect = new CustomRectangle(x - 5, y - 5, 10, 10);   //�ж�ѡ��ʱ�ӵ���ʹѡ�и���˳��
            return rect.IsPointFInRectangle(pt.X, pt.Y);
        }

        /// <summary>
        /// ��һ��Point������CustomPoint����
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static CustomPoint FromPoint(Point pt)
        {
            return new CustomPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// ��һ��Point������CustomPoint����
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static CustomPoint FromPointF(PointF pt)
        {
            return new CustomPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// ת��ΪPoint����
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// ת��ΪPointF����
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF(x, y);
        }

        #endregion
    }
}
