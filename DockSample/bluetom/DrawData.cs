using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DockSample
{
    class DrawData
    {
        //绘制饼形图
        static public Image DrawEllipse(int imageWidth,string DataName, string DataValue)
        {
            //string dataName = "a,b,c,d,e";
            //string dataValue = "6,2,3,9,3";
            string dataName = DataName;
            string dataValue = DataValue;
            int dataValueSum = 0;
            int dataCnt = dataName.Split(',').Length;
            float[] arrDataPercentage = new float[dataCnt];

            string[] arrDataName = new string[dataCnt];
            int[] arrDataValue = new int[dataCnt];

            Random rand = new Random();
            arrDataName = dataName.Split(',');
            for (int i = 0; i < dataCnt; i++)
            {
                arrDataValue[i] = Convert.ToInt32(dataValue.Split(',')[i]);
                dataValueSum += arrDataValue[i];
            }

            for (int i = 0; i < dataCnt; i++)//计算百分率
            {
                arrDataPercentage[i] = Convert.ToSingle(arrDataValue[i]) / dataValueSum;
            }

            //int imgWidth = 400, imgHeight = 600;
            int imgWidth = imageWidth;
            Image image = new Bitmap(imgWidth, imgWidth + 20 * (dataCnt+1) + 65);
            //BorderStyle
            Rectangle rectBorder = new Rectangle(1, 1, imgWidth - 3, imgWidth - 3);
            Rectangle rectBorder2 = new Rectangle(1, imgWidth - 3, imgWidth - 3, 20 * (dataCnt+1) + 65);
            Pen borderColor = new Pen(Color.Blue);
            //PieStyle
            SolidBrush[] arrbrush = new SolidBrush[dataCnt];
            for (int i = 0; i < dataCnt; i++)
            {
                arrbrush[i] = new SolidBrush(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            }

            //startGraphics
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //GraphicsLine
            g.DrawRectangle(borderColor, rectBorder);
             g.DrawRectangle(borderColor, rectBorder2);
            //GraphicsPie
            float startPosition = 0.0f;
            Rectangle rectPie = new Rectangle(rectBorder.Location.X + 2, rectBorder.Location.Y + 2, rectBorder.Width - 4, rectBorder.Height - 4);
            for (int i = 0; i < dataCnt; i++)
            {
                g.FillPie(arrbrush[i], rectPie, startPosition, arrDataPercentage[i] * 360);
                startPosition += arrDataPercentage[i] * 360;
            }
            //GraphicsString
            int sum1=0;
            int p;
            for ( p= 0; p < dataCnt; p++)
            {
                g.FillRectangle(arrbrush[p], new Rectangle(20, p * 20 + rectBorder.Width + 5, 15, 15));
                string str = string.Format("{0}——{1:p}    {2}", arrDataName[p], arrDataPercentage[p],arrDataValue[p]);
                g.DrawString(str, new Font("", 9), arrbrush[p], new Point(40, p* 20 + rectBorder.Width + 5));
                sum1 += arrDataValue[p];
            }
            g.FillRectangle(Brushes.Red, new Rectangle(20, p * 20 + rectBorder.Width + 5, 15, 15));
            string str1 = string.Format("{0}——{1}", "二局二处", sum1);
            g.DrawString(str1, new Font("", 9), Brushes.Red, new Point(40, p * 20 + rectBorder.Width + 5));

            return image;
        }

        //绘制折线图
        //例如DrawData.DrawPolyonLine(pictureBox1.Width,pictureBox1.Height ,"2000-3000年比例","6,8,10,12,14,16,18,20,22","个","1,2,3,4,5,6,7,8,9,10,11,12","年","12,6,8,10,12,14,16,18,20,2,11,11");
        static public Image DrawPolyonLine(int ImgWidth, int ImgHeight, string Title, string LeftName,int LeftMinNum,int LeftAvgNum, string BottomName, string Data)
        {
            int imgWidth = 600, imgHeight = 400;
            string title = "2000-3000年比例";
            string leftName = "6个,8个,10个,12个,14个,16个,18个,20个,22个";

            string bottomName = "1年,2年,3年,4年,5年,6年,7年,8年,9年,10年,11年,12年";

            string data = "12,6,8,10,12,14,16,18,20,2,11,11";
            int leftMinNum = 6;
            int leftAvgNum = 2;

            imgWidth = ImgWidth;
            imgHeight = ImgHeight;
            title = Title;
            leftName = LeftName;
            leftMinNum = LeftMinNum;
            leftAvgNum = LeftAvgNum;
            bottomName = BottomName;

            data = Data;

            int horizontalLineCnt = leftName.Split(',').Length + 1;
            int verticalLineCnt = bottomName.Split(',').Length;

            int[] xPosition = new int[verticalLineCnt];
            int[] yPosition = new int[horizontalLineCnt];

            string[] arrLeftName = leftName.Split(',');
            string[] arrBottomName = bottomName.Split(',');
            string[] arrdata = data.Split(',');

            Image image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //标题
            SolidBrush brushStr = new SolidBrush(Color.Blue);
            Font fontTitle = new Font("Arial", 20);
            SizeF sf = g.MeasureString(title, fontTitle);
            g.DrawString(title, new Font("Arial", 20), brushStr, new Point((int)(image.Width - sf.Width) / 2, 0));
            //确定背景线的位置
            int lineWidth = image.Width * 8 / 10;
            int lineHeight = image.Height * 8 / 10;
            Point startPosition = new Point(image.Width / 10, image.Height / 10);
            Point endPostion = new Point(image.Width * 9 / 10, image.Height * 9 / 10);
            int horizontalBetween = lineHeight / horizontalLineCnt;
            int verticalBetween = lineWidth / verticalLineCnt;
            //确定x点做标
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                yPosition[h] = (h + 1) * horizontalBetween + startPosition.Y;
            }
            for (int v = 0; v < verticalLineCnt; v++)
            {
                xPosition[v] = (v) * verticalBetween + startPosition.X;
            }
            //画背景线
            Pen pen = new Pen(Color.Blue);
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + lineWidth, yPosition[h]);
                lineHeight = (h + 1) * horizontalBetween;
            }

            for (int v = 0; v < verticalLineCnt; v++)
            {
                g.DrawLine(pen, xPosition[v], startPosition.Y, xPosition[v], startPosition.Y + lineHeight);
            }
            //画汉字
            //float fdataHeight = horizontalBetween / leftMinNum;
            //float fdataAverageHeight = horizontalBetween / leftAvgNum;
            Point[] arrDataPoint = new Point[arrdata.Length];
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata[l]) > LeftMinNum)
                {

                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                }
            }


            Font font = new Font("arial", 9);
            for (int s = 0; s < arrBottomName.Length; s++)
            {
                string str = arrBottomName[s];
                string str2 = arrdata[s];
                g.DrawString(str, font, Brushes.Blue, new Point(xPosition[s] -5 + verticalBetween / 2, endPostion.Y + 5));
                //g.DrawString(str2, font, Brushes.Red, new Point(xPosition[s] + 5, arrDataPoint[s].Y));

            }
            for (int s = 0; s < arrLeftName.Length; s++)
            {
                int cnt = arrLeftName.Length - 1;
                string str = arrLeftName[cnt - s];
                int ix = startPosition.X - 5 - (int)g.MeasureString(str, font).Width;
                int iy = yPosition[s] - 15;

                g.DrawString(str, font, Brushes.Blue, new Point(ix, iy));
            }

            //画折线效果
            //数据每一个为x像素


            g.DrawLines(Pens.Red, arrDataPoint);
            return image;
        }
        //绘制柱形图
        //例如: DrawData.DrawPillar(pictureBox1.Width, pictureBox1.Height, "2000-3000年比例", "1,2,3,4,5", "23,11,44,22,11");
        static public Image DrawPillar(int ImgWidth, int ImgHeight, string Title, string BottomName, string Data)
        {
            int imgWidth = 600, imgHeight = 400;
            string title = "2000-3000年比例";
            string leftName;
            int avgstr;
            string bottomName = "孙炎娜,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
            string data = "12,6,8,10,12,14,16,18,20,2,11,11,3,6,9,6";


            imgWidth = ImgWidth;
            imgHeight = ImgHeight;
            title = Title;
            bottomName = BottomName;
            data = Data;

            int dataSum = 0;


            float[] arrDataPercentage = new float[data.Split(',').Length];

            int verticalLineCnt = data.Split(',').Length;
            if (verticalLineCnt <= 5)
            {
                leftName = "0%,10%,20%,30%,40%,50%,60%,70%,80%,90%,100%";
                avgstr = 10;
            }
            else if (verticalLineCnt <= 15)
            {
                leftName = "0%,2%,4%,6%,8%,10%,12%,14%,16%,18%,20%";
                avgstr = 2;
            }
            else
            {
                leftName = "0%,1%,2%,3%,4%,5%,6%,7%,8%,9%,10%";
                avgstr = 1;

            }
            int horizontalLineCnt = leftName.Split(',').Length;


            int[] xPosition = new int[verticalLineCnt];
            int[] yPosition = new int[horizontalLineCnt];

            string[] arrLeftName = leftName.Split(',');
            string[] arrBottomName = bottomName.Split(',');
            string[] arrdata = data.Split(',');


            foreach (string s in arrdata)
            {
                dataSum += Convert.ToInt32(s);

            }
            for (int d = 0; d < arrdata.Length; d++)
            {
                arrDataPercentage[d] = Convert.ToSingle(arrdata[d]) / dataSum;

            }



            Image image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //标题
            SolidBrush brushStr = new SolidBrush(Color.Blue);
            Font fontTitle = new Font("Arial", 20);
            SizeF sf = g.MeasureString(title, fontTitle);
            g.DrawString(title, new Font("Arial", 20), brushStr, new Point((int)(image.Width - sf.Width) / 2, 0));
            //确定背景线的位置
            int lineWidth = image.Width * 8 / 10;
            int lineHeight = image.Height * 8 / 10;
            Point startPosition = new Point(image.Width / 10, image.Height / 10);
            Point endPostion = new Point(image.Width * 9 / 10, image.Height * 9 / 10);
            int horizontalBetween = lineHeight / horizontalLineCnt;
            int verticalBetween = lineWidth / verticalLineCnt;
            //确定x点做标
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                yPosition[h] = (h + 1) * horizontalBetween + startPosition.Y;
            }
            for (int v = 0; v < verticalLineCnt; v++)
            {
                xPosition[v] = (v) * verticalBetween + startPosition.X;
            }
            //画背景线
            Pen pen = new Pen(Color.Blue);
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + lineWidth, yPosition[h]);
                lineHeight = (h + 1) * horizontalBetween;
            }

            for (int v = 0; v < verticalLineCnt; v++)
            {
                g.DrawLine(pen, xPosition[v], startPosition.Y, xPosition[v], startPosition.Y + lineHeight);
            }
            //画汉字
            Font font = new Font("arial", 9);
            for (int s = 0; s < verticalLineCnt; s++)
            {
                string str = arrBottomName[s];
                string str1 = arrdata[s];
                g.DrawString(str, font, Brushes.Blue, new Point(xPosition[s] + verticalBetween / 2-10, endPostion.Y + 5));
                g.DrawString(str1, font, Brushes.Blue, new Point(xPosition[s] + verticalBetween /2 -10, yPosition[horizontalLineCnt - 1] - (int)(arrDataPercentage[s] * 100 * horizontalBetween / avgstr) - 20));

            }

            for (int s = 0; s < arrLeftName.Length; s++)
            {
                int cnt = arrLeftName.Length - 1;
                string str = arrLeftName[cnt - s];
                int ix = startPosition.X - 5 - (int)g.MeasureString(str, font).Width;
                int iy = yPosition[s] - 10;

                g.DrawString(str, font, Brushes.Blue, new Point(ix, iy));
            }

            //画柱形图
            //数据每一个为x像素
            float fdataWidth = verticalBetween / 2;
            for (int i = 0; i < verticalLineCnt; i++)
            {
                
                int rectHeight = (int)(arrDataPercentage[i] * 100 * horizontalBetween / avgstr);
                if (rectHeight == 0)
                    rectHeight = 1;
                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, (int)fdataWidth, (int)rectHeight), Color.White, Color.OrangeRed, 0.0f);
                g.FillRectangle(lgb, new Rectangle(xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, (int)fdataWidth, (int)rectHeight));

                //g.FillRectangle(Brushes.Blue, xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, fdataWidth, rectHeight);
            }
            return image;
        }

        static public Image DrawPillar(int ImgWidth, int ImgHeight, string Title, string LeftName, int LeftMinNum, int LeftAvgNum, string BottomName, string Data,Color c)
        {
            int imgWidth = 600, imgHeight = 400;
            string title = "2000-3000年比例";

            string bottomName = "1年,2年,3年,4年,5年,6年,7年,8年,9年,10年,11年,12年";

            string data = "12,6,8,10,12,14,16,18,20,2,11,11";
            int leftMinNum = 6;
            int leftAvgNum = 2;
            string leftName = "6个,8个,10个,12个,14个,16个,18个,20个,22个";

            imgWidth = ImgWidth;
            imgHeight = ImgHeight;
            title = Title;
            leftName = LeftName;
            leftMinNum = LeftMinNum;
            leftAvgNum = LeftAvgNum;
            bottomName = BottomName;
            int dataSum = 0;
            data = Data;


            float[] arrDataPercentage = new float[data.Split(',').Length];


            int horizontalLineCnt = leftName.Split(',').Length + 1;
            int verticalLineCnt = bottomName.Split(',').Length;

            int[] xPosition = new int[verticalLineCnt];
            int[] yPosition = new int[horizontalLineCnt];

            string[] arrLeftName = leftName.Split(',');
            string[] arrBottomName = bottomName.Split(',');
            string[] arrdata = data.Split(',');
            foreach (string s in arrdata)
            {
                dataSum += Convert.ToInt32(s);

            }

            Image image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //标题
            SolidBrush brushStr = new SolidBrush(Color.Blue);
            Font fontTitle = new Font("Arial", 20);
            SizeF sf = g.MeasureString(title, fontTitle);
            g.DrawString(title, new Font("Arial", 20), brushStr, new Point((int)(image.Width - sf.Width) / 2, 0));
            //确定背景线的位置
            int lineWidth = image.Width * 8 / 10;
            int lineHeight = image.Height * 8 / 10;
            Point startPosition = new Point(image.Width / 10, image.Height / 10);
            Point endPostion = new Point(image.Width * 9 / 10, image.Height * 9 / 10);
            int horizontalBetween = lineHeight / horizontalLineCnt;
            int verticalBetween = lineWidth / verticalLineCnt;
            //确定x点做标
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                yPosition[h] = (h + 1) * horizontalBetween + startPosition.Y;
            }
            for (int v = 0; v < verticalLineCnt; v++)
            {
                xPosition[v] = (v) * verticalBetween + startPosition.X;
            }
            //画背景线
            Pen pen = new Pen(Color.Blue);
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                if (h < horizontalLineCnt - 1)
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + 5, yPosition[h]);
                }
                else
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + lineWidth, yPosition[h]);

                }
                lineHeight = (h + 1) * horizontalBetween;
            }

            for (int v = 0; v < verticalLineCnt; v++)
            {
                if (v == 0)
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y, xPosition[v], startPosition.Y + lineHeight);
                }
                else
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y + lineHeight - 5, xPosition[v], startPosition.Y + lineHeight);

                }
            }
            //画汉字
            //double fdataHeight = horizontalBetween / leftMinNum;
            //double fdataAverageHeight = horizontalBetween / leftAvgNum;
            Point[] arrDataPoint = new Point[arrdata.Length];
            int[] rectHeight = new int[arrdata.Length];
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata[l]) > LeftMinNum)
                {
                    rectHeight[l] = (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    rectHeight[l] = (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                }
            }
            Font font = new Font("arial", 9); ;
               for (int s = 0; s < arrBottomName.Length; s++)
                {
                    string str = arrBottomName[s];
                    string str2 = arrdata[s];
                    g.DrawString(str, font, Brushes.Blue, new Point(xPosition[s] + verticalBetween / 2 - 20, endPostion.Y + 5));
                    g.DrawString(str2, font, Brushes.Red, new Point(xPosition[s] + verticalBetween / 2 - 10, arrDataPoint[s].Y - 20));

                }
            for (int s = 0; s < arrLeftName.Length; s++)
            {
                int cnt = arrLeftName.Length - 1;
                string str = arrLeftName[cnt - s];
                int ix = startPosition.X - 5 - (int)g.MeasureString(str, font).Width;
                int iy = yPosition[s] - 15;

                g.DrawString(str, font, Brushes.Blue, new Point(ix, iy));
            }


            //画柱形图
            //数据每一个为x像素
            float fdataWidth = verticalBetween / 2;
            for (int i = 0; i < verticalLineCnt; i++)
            {

                if (rectHeight[i] == 0)
                    rectHeight[i] = 1;
                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(arrDataPoint[i].X + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]), Color.White,c, 0.0f);
                g.FillRectangle(lgb, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]));
                //LinearGradientBrush lgb2 = new LinearGradientBrush(new Rectangle(arrDataPoint[i].X + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]), Color.White, Color.DarkOrchid, 0.0f);

                //g.FillRectangle(lgb2, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, 10));
                //g.FillRectangle(Brushes.Blue, xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, fdataWidth, rectHeight);
            }
            return image;
        }
        static public Image DrawPillar(int ImgWidth, int ImgHeight, string Title, string LeftName, int LeftMinNum, int LeftAvgNum, string BottomName, string[] Data, Color c)
        {
            int imgWidth = 600, imgHeight = 400;
            string title = "2000-3000年比例";

            string bottomName = "1年,2年,3年,4年,5年,6年,7年,8年,9年,10年,11年,12年";

            string[] data;
            int leftMinNum = 6;
            int leftAvgNum = 2;
            string leftName = "6个,8个,10个,12个,14个,16个,18个,20个,22个";

            imgWidth = ImgWidth;
            imgHeight = ImgHeight;
            title = Title;
            leftName = LeftName;
            leftMinNum = LeftMinNum;
            leftAvgNum = LeftAvgNum;
            bottomName = BottomName;
            //int dataSum = 0;
            data = Data;


            float[] arrDataPercentage = new float[data[0].Split(',').Length];


            int horizontalLineCnt = leftName.Split(',').Length + 1;
            int verticalLineCnt = bottomName.Split(',').Length;

            int[] xPosition = new int[verticalLineCnt];
            int[] yPosition = new int[horizontalLineCnt];

            string[] arrLeftName = leftName.Split(',');
            string[] arrBottomName = bottomName.Split(',');
            int sum_zs = data.Length;
            string[] arrdata;
            string[] arrdata2;

               arrdata = data[0].Split(',');
               arrdata2 = data[1].Split(',');


            //foreach (string s in arrdata)
            //{
            //    dataSum += Convert.ToInt32(s);

            //}

            Image image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //标题
            SolidBrush brushStr = new SolidBrush(Color.Blue);
            Font fontTitle = new Font("Arial", 20);
            SizeF sf = g.MeasureString(title, fontTitle);
            g.DrawString(title, new Font("Arial", 20), brushStr, new Point((int)(image.Width - sf.Width) / 2, 0));
            //确定背景线的位置
            int lineWidth = image.Width * 8 / 10;
            int lineHeight = image.Height * 8 / 10;
            Point startPosition = new Point(image.Width / 10, image.Height / 10);
            Point endPostion = new Point(image.Width * 9 / 10, image.Height * 9 / 10);
            int horizontalBetween = lineHeight / horizontalLineCnt;
            int verticalBetween = lineWidth / verticalLineCnt;
            //确定x点做标
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                yPosition[h] = (h + 1) * horizontalBetween + startPosition.Y;
            }
            for (int v = 0; v < verticalLineCnt; v++)
            {
                xPosition[v] = (v) * verticalBetween + startPosition.X;
            }
            //画背景线
            Pen pen = new Pen(Color.Blue);
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                if (h < horizontalLineCnt - 1)
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + 5, yPosition[h]);
                }
                else
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + lineWidth, yPosition[h]);

                }
                lineHeight = (h + 1) * horizontalBetween;
            }

            for (int v = 0; v < verticalLineCnt; v++)
            {
                if (v == 0)
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y, xPosition[v], startPosition.Y + lineHeight);
                }
                else
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y + lineHeight - 5, xPosition[v], startPosition.Y + lineHeight);

                }
            }
            //画汉字
            //double fdataHeight = horizontalBetween / leftMinNum;
            //double fdataAverageHeight = horizontalBetween / leftAvgNum;

            Point[] arrDataPoint = new Point[arrdata.Length];
            int[] rectHeight = new int[arrdata.Length];
            Point[] arrDataPoint2 = new Point[arrdata2.Length];
            int[] rectHeight2 = new int[arrdata2.Length];
            //Point[] arrDataPoint3 = new Point[arrdata3.Length];
            //int[] rectHeight3 = new int[arrdata3.Length];
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata[l]) > LeftMinNum)
                {
                    rectHeight[l] = (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    rectHeight[l] = (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                }
            }
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint2[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata2[l]) > LeftMinNum)
                {
                    rectHeight2[l] = (int)((Convert.ToInt32(arrdata2[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                    arrDataPoint2[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata2[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    rectHeight2[l] = (int)(Convert.ToInt32(arrdata2[l]) * horizontalBetween / leftMinNum);
                    arrDataPoint2[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata2[l]) * horizontalBetween / leftMinNum);
                }
            }
            //画柱形图
            //数据每一个为x像素
            float fdataWidth = verticalBetween / 2;
            for (int i = 0; i < verticalLineCnt; i++)
            {

                if (rectHeight[i] == 0)
                    rectHeight[i] = 1;
                if (rectHeight2[i] == 0)
                    rectHeight2[i] = 1;
                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(arrDataPoint[i].X + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]), Color.White, c, 0.0f);
                g.FillRectangle(lgb, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]));
               
                LinearGradientBrush lgb2 = new LinearGradientBrush(new Rectangle(arrDataPoint2[i].X + (int)fdataWidth / 2, arrDataPoint2[i].Y, (int)fdataWidth, rectHeight2[i]), Color.White, Color.DarkOrchid, 0.0f);

                g.FillRectangle(lgb2, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint2[i].Y, (int)fdataWidth, rectHeight2[i]));
                //g.FillRectangle(Brushes.Blue, xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, fdataWidth, rectHeight);
            }
            Font font = new Font("arial", 9); ;
            for (int s = 0; s < arrBottomName.Length; s++)
            {
                string str = arrBottomName[s];
                string str2 = arrdata[s];
                string str3 = arrdata2[s];
                g.DrawString(str, font, Brushes.Blue, new Point(xPosition[s] + verticalBetween / 2 - 20, endPostion.Y + 5));
                g.DrawString(str2, font, Brushes.Red, new Point(xPosition[s] + verticalBetween / 2 - 10, arrDataPoint[s].Y - 20));
                g.DrawString(str3, font, Brushes.Black, new Point(xPosition[s] + verticalBetween / 2 - 10, arrDataPoint2[s].Y - 15));

            }
            for (int s = 0; s < arrLeftName.Length; s++)
            {
                int cnt = arrLeftName.Length - 1;
                string str = arrLeftName[cnt - s];
                int ix = startPosition.X - 5 - (int)g.MeasureString(str, font).Width;
                int iy = yPosition[s] - 15;

                g.DrawString(str, font, Brushes.Blue, new Point(ix, iy));
            }



            return image;
        }
/// <summary>
/// //////////////////////////////////////////////////////
/// </summary>
/// <param name="ImgWidth"></param>
/// <param name="ImgHeight"></param>
/// <param name="Title"></param>
/// <param name="LeftName"></param>
/// <param name="LeftMinNum"></param>
/// <param name="LeftAvgNum"></param>
/// <param name="BottomName"></param>
/// <param name="Data"></param>
/// <param name="c"></param>
/// <returns></returns>
        static public Image DrawPillar(int ImgWidth, int ImgHeight, string Title, string LeftName, int LeftMinNum, int LeftAvgNum, string BottomName, string[] Data, Color c,int[] str_num)
        {
            int imgWidth = 600, imgHeight = 400;
            string title = "2000-3000年比例";

            string bottomName = "1年,2年,3年,4年,5年,6年,7年,8年,9年,10年,11年,12年";

            string[] data;
            int leftMinNum = 6;
            int leftAvgNum = 2;
            string leftName = "6个,8个,10个,12个,14个,16个,18个,20个,22个";
            int[] cq_num = str_num;
            
            imgWidth = ImgWidth;
            imgHeight = ImgHeight;
            title = Title;
            leftName = LeftName;
            leftMinNum = LeftMinNum;
            leftAvgNum = LeftAvgNum;
            bottomName = BottomName;
            //int dataSum = 0;
            data = Data;


            float[] arrDataPercentage = new float[data[0].Split(',').Length];


            int horizontalLineCnt = leftName.Split(',').Length + 1;
            int verticalLineCnt = bottomName.Split(',').Length;

            int[] xPosition = new int[verticalLineCnt];
            int[] yPosition = new int[horizontalLineCnt];

            string[] arrLeftName = leftName.Split(',');
            string[] arrBottomName = bottomName.Split(',');
            int sum_zs = data.Length;
            string[] arrdata;
            string[] arrdata2;

            arrdata = data[0].Split(',');
            arrdata2 = data[1].Split(',');


            //foreach (string s in arrdata)
            //{
            //    dataSum += Convert.ToInt32(s);

            //}

            Image image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //标题
            SolidBrush brushStr = new SolidBrush(Color.Blue);
            Font fontTitle = new Font("Arial", 20);
            SizeF sf = g.MeasureString(title, fontTitle);
            g.DrawString(title, new Font("Arial", 20), brushStr, new Point((int)(image.Width - sf.Width) / 2, 0));
            //确定背景线的位置
            int lineWidth = image.Width * 8 / 10;
            int lineHeight = image.Height * 8 / 10;
            Point startPosition = new Point(image.Width / 10, image.Height / 10);
            Point endPostion = new Point(image.Width * 9 / 10, image.Height * 9 / 10);
            int horizontalBetween = lineHeight / horizontalLineCnt;
            int verticalBetween = lineWidth / verticalLineCnt;
            //确定x点做标
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                yPosition[h] = (h + 1) * horizontalBetween + startPosition.Y;
            }
            for (int v = 0; v < verticalLineCnt; v++)
            {
                xPosition[v] = (v) * verticalBetween + startPosition.X;
            }
            //画背景线
            Pen pen = new Pen(Color.Blue);
            for (int h = 0; h < horizontalLineCnt; h++)
            {
                if (h < horizontalLineCnt - 1)
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + 5, yPosition[h]);
                }
                else
                {
                    g.DrawLine(pen, startPosition.X, yPosition[h], startPosition.X + lineWidth, yPosition[h]);

                }
                lineHeight = (h + 1) * horizontalBetween;
            }

            for (int v = 0; v < verticalLineCnt; v++)
            {
                if (v == 0)
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y, xPosition[v], startPosition.Y + lineHeight);
                }
                else
                {
                    g.DrawLine(pen, xPosition[v], startPosition.Y + lineHeight - 5, xPosition[v], startPosition.Y + lineHeight);

                }
            }
            //画汉字
            //double fdataHeight = horizontalBetween / leftMinNum;
            //double fdataAverageHeight = horizontalBetween / leftAvgNum;

            Point[] arrDataPoint = new Point[arrdata.Length];
            int[] rectHeight = new int[arrdata.Length];
            Point[] arrDataPoint2 = new Point[arrdata2.Length];
            int[] rectHeight2 = new int[arrdata2.Length];
            //Point[] arrDataPoint3 = new Point[arrdata3.Length];
            //int[] rectHeight3 = new int[arrdata3.Length];
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata[l]) > LeftMinNum)
                {
                    rectHeight[l] = (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    rectHeight[l] = (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                    arrDataPoint[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata[l]) * horizontalBetween / leftMinNum);
                }
            }
            for (int l = 0; l < arrBottomName.Length; l++)
            {
                arrDataPoint2[l].X = xPosition[l];
                if (Convert.ToInt32(arrdata2[l]) > LeftMinNum)
                {
                    rectHeight2[l] = (int)((Convert.ToInt32(arrdata2[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                    arrDataPoint2[l].Y = yPosition[horizontalLineCnt - 1] - (int)((Convert.ToInt32(arrdata2[l]) - leftMinNum) * horizontalBetween / leftAvgNum + horizontalBetween);
                }
                else
                {
                    rectHeight2[l] = (int)(Convert.ToInt32(arrdata2[l]) * horizontalBetween / leftMinNum);
                    arrDataPoint2[l].Y = yPosition[horizontalLineCnt - 1] - (int)(Convert.ToInt32(arrdata2[l]) * horizontalBetween / leftMinNum);
                }
            }
            //画柱形图
            //数据每一个为x像素
            float fdataWidth = verticalBetween / 2;
            for (int i = 0; i < verticalLineCnt; i++)
            {

                if (rectHeight[i] == 0)
                    rectHeight[i] = 1;
                if (rectHeight2[i] == 0)
                    rectHeight2[i] = 1;
                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(arrDataPoint[i].X + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]), Color.White, c, 0.0f);
                g.FillRectangle(lgb, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint[i].Y, (int)fdataWidth, rectHeight[i]));

                LinearGradientBrush lgb2 = new LinearGradientBrush(new Rectangle(arrDataPoint2[i].X + (int)fdataWidth / 2, arrDataPoint2[i].Y, (int)fdataWidth, rectHeight2[i]), Color.White, Color.DarkOrchid, 0.0f);

                g.FillRectangle(lgb2, new Rectangle(xPosition[i] + (int)fdataWidth / 2, arrDataPoint2[i].Y, (int)fdataWidth, rectHeight2[i]));
                //g.FillRectangle(Brushes.Blue, xPosition[i] + (int)fdataWidth / 2, yPosition[horizontalLineCnt - 1] - rectHeight, fdataWidth, rectHeight);
            }
            Font font = new Font("arial", 9); ;
            for (int s = 0; s < arrBottomName.Length; s++)
            {
                string str = arrBottomName[s];
                string str2 = arrdata[s];
                string str3 = arrdata2[s];
                g.DrawString(str, font, Brushes.Blue, new Point(xPosition[s] + verticalBetween / 2 - 20, endPostion.Y + 5));
                g.DrawString(str2, font, Brushes.Red, new Point(xPosition[s] + verticalBetween / 2 - 10, arrDataPoint[s].Y - 20));
                g.DrawString(str3, font, Brushes.Black, new Point(xPosition[s] + verticalBetween / 2 - 10, arrDataPoint2[s].Y - 15));

            }
            for (int s = 0; s < arrLeftName.Length; s++)
            {
                int cnt = arrLeftName.Length - 1;
                string str = arrLeftName[cnt - s];
                int ix = startPosition.X - 5 - (int)g.MeasureString(str, font).Width;
                int iy = yPosition[s] - 15;

                g.DrawString(str, font, Brushes.Blue, new Point(ix, iy));
            }
            int t=0;
            for(int i=0;i<cq_num.Length;i++)
            {
                t+=cq_num[i];
            }
            if (t>0)
            {
                int i = 0;
                for (int p = 0; p < cq_num.Length; p++)
                {

                    switch (p)
                    {
                        case 0: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "南阳错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 1: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "广州错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 2: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "昆明错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 3: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "城阳错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 4: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "绥化错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 5: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "陵水错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                                i++;
                            }
                            break;
                        case 6: if (cq_num[p] > 0)
                            {
                                string str_wt = string.Format("{0}：{1}", "崇明错情", Convert.ToString(cq_num[p]));
                                g.DrawString(str_wt, font, Brushes.Red, new Point(image.Width - 100, i * 20 + 5));
                            }
                            break;
                    }
                }
            }


            return image;
        }

    }
}

