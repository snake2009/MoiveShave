using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace 老司机影片整理
{
    internal class ImageProcess
    {

        /// <summary>
        /// 百度AI识别从背景图截取出封面
        /// </summary>
        /// <param name="backdropPath">背景图</param>
        /// <param name="config">用户配置</param>
        /// <returns></returns>
        internal static Image GetCoverByAI(string backdropPath, Config config)
        {
            var image = File.ReadAllBytes(backdropPath);
            var baseImg = Image.FromFile(backdropPath);
            var width = (int)(baseImg.Height * 0.7029702970297);
            var tempImage = new Bitmap(width, baseImg.Height);
            try
            {
                var left = 0;
                var client = new Baidu.Aip.BodyAnalysis.Body(config.BD_API_KEY, config.BD_SECRET_KEY);
                client.Timeout = 60000;
                var result = client.BodyAnalysis(image);
                var person_num = int.Parse(result["person_num"].ToString());
                if (person_num > 1)
                {
                    double[] x_arr = new double[person_num];
                    for (int i = 0; i < person_num; i++)
                    {
                        x_arr[i] = double.Parse(result["person_info"][i]["body_parts"]["nose"]["x"].ToString());
                    }
                    //如果多于2个人
                    if (person_num > 2)
                    {
                        if (person_num > 10)
                        {
                            //多数情况下是影片左边缩略详情图，还是取右边做封面
                            left = baseImg.Width - width;
                        }
                        else
                        {
                            var list = new List<double>(x_arr);
                            list.Sort();
                            //排序后取中间人的坐标计算
                            left = (int)(list[list.Count / 2] - width / 2.00);
                        }
                    }
                    else
                    {
                        //2号在先识别出来
                        if(x_arr[1] > x_arr[0])
                        {
                            //取两人中心值计算起点
                            left = (int)(x_arr[0] + (x_arr[1] - x_arr[0]) / 2 - width / 2.00);
                        }
                        else
                        {
                            left = (int)(x_arr[1] + (x_arr[0] - x_arr[1]) / 2 - width / 2.00);
                        }
                    }
                }
                else
                {
                    var noseX = result["person_info"][0]["body_parts"]["nose"]["x"];
                    double nx = double.Parse(noseX.ToString());
                    left = (int)(nx - width / 2.00);
                }
                Graphics templateG = Graphics.FromImage(tempImage);
                templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                templateG.Clear(Color.White);
                templateG.DrawImage(baseImg, new Rectangle(0, 0, tempImage.Width, tempImage.Height), new Rectangle(left, 0, width, baseImg.Height), GraphicsUnit.Pixel);
                templateG.Dispose();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                baseImg.Dispose();
            }
            return tempImage;
        }

        /// <summary>
        /// 从背景图截取出封面
        /// </summary>
        /// <param name="backdropPath">背景图</param>
        /// <param name="config">用户配置</param>
        /// <returns></returns>
        internal static Image GetCover(string backdropPath, Config config)
        {
            var baseImg = Image.FromFile(backdropPath);
            var width = (int)(baseImg.Height * 0.7029702970297);
            var tempImage = new Bitmap(width, baseImg.Height);
            try
            {
                Graphics templateG = Graphics.FromImage(tempImage);
                templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                templateG.Clear(Color.White);
                templateG.DrawImage(baseImg, new Rectangle(0, 0, tempImage.Width, tempImage.Height), new Rectangle(baseImg.Width - width, 0, width, baseImg.Height), GraphicsUnit.Pixel);
                templateG.Dispose();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                baseImg.Dispose();
            }
            return tempImage;
        }
    }
}