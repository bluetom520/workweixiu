//#####################################################################################
//��������           http://www.cnpopsoft.com [�������]           ��������
//��� '�������-VB��C#רҵ������Դ�����ͣ����ݿ�����ƽ̨ս�ԣ�����ҵ�����������  ���
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

        #region ���Զ�����

        private float x;
        /// <summary>
        /// X����
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;
        /// <summary>
        /// Y����
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// X����
        /// </summary>
        public float Left
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Y����
        /// </summary>
        public float Top
        {
            get { return y; }
            set { y = value; }
        }

        private float width;
        /// <summary>
        /// ���
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float height;
        /// <summary>
        /// �߶�
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// �����ұ�X�����꣨����X+Width��
        /// </summary>
        public float Right
        {
            get { return x + width; }
        }

        /// <summary>
        /// ���صײ�Y�����꣨����Y+Height��
        /// </summary>
        public float Bottom
        {
            get { return y + height; }
        }

        /// <summary>
        /// ���ؾ����е�
        /// </summary>
        public CustomPoint CenterPoint
        {
            get { return new CustomPoint(x + width / 2, y + height / 2); }
        }

        #endregion

        #region ������

        /// <summary>
        /// ����һ����¡�Ķ���
        /// </summary>
        /// <returns></returns>
        public CustomRectangle Clone()
        {
            return new CustomRectangle(x, y, width, height);
        }

        /// <summary>
        /// ת��ΪRectangle����
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        /// <summary>
        /// ת��ΪRectangleF����
        /// </summary>
        /// <returns></returns>
        public RectangleF ToRectangleF()
        {
            return new RectangleF(x, y, width, height);
        }


        /// <summary>
        /// ����һ�����Ͻ������
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// ����һ�����Ͻ������
        /// </summary>
        /// <returns></returns>
        public PointF ToPointF()
        {
            return new PointF(x, y);
        }

        /// <summary>
        /// �ж�ָ������ĵ��Ƿ��ھ����ڲ�
        /// </summary>
        /// <param name="ptX">X����</param>
        /// <param name="ptY">Y����</param>
        /// <returns></returns>
        public bool IsPointInRectangle(int ptX, int ptY)
        {
            return (ptX >= x && ptX <= (x + width) && ptY >= y && ptY <= (y + height));
        }

        /// <summary>
        /// �ж�ָ������ĵ��Ƿ��ھ����ڲ�
        /// </summary>
        /// <param name="ptX">X����</param>
        /// <param name="ptY">Y����</param>
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
        /// ����Rectangle����һ��CustomRectangle����
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CustomRectangle FromRectangle(Rectangle rect)
        {
            return new CustomRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// ����RectangleF����һ��CustomRectangle����
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static CustomRectangle FromRectangleF(RectangleF rect)
        {
            return new CustomRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// ��ȡ�߿��Բ�Ǿ���·�������ڻ�ͼ��
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
        /// ��ȡ�����λ�ͼ·��
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
        /// ��ȡ��������·��
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
        /// �������λ��ȷ����������1��2��3��4��ʾ�м�4����������Ϊ-1
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
        /// ��ȡ������������
        /// </summary>
        /// <param name="hotPosition">1,2,3,4��0��ʾȫѡ</param>
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
