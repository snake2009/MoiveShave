using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace 老司机影片整理
{
    internal class ImageTools
    {
        private Config config;

        public ImageTools(Config config)
        {
            this.config = config;
        }

        class Person
        {
            public double Left { set; get; }
            public double Top { set; get; }
            public double Width { set; get; }
            public double Height { set; get; }
            public double Score { set; get; }
            public double NoseX { set; get; }
        }

        /// <summary>
        /// 百度AI识别从背景图截取出封面
        /// </summary>
        /// <param name="backdropPath">背景图</param>
        /// <param name="config">用户配置</param>
        /// <returns>处理完的新图像</returns>
        private void GetCoverByAI(string backdropPath, string coverPath)
        {
            var image = File.ReadAllBytes(backdropPath);
            var baseImg = Image.FromFile(backdropPath);
            var width = (int)(baseImg.Height * 0.7029702970297);
            var tempImage = new Bitmap(width, baseImg.Height);
            try
            {
                var left = 0;
                var client = new Baidu.Aip.BodyAnalysis.Body(config.BD_API_KEY, config.BD_SECRET_KEY)
                {
                    Timeout = 60000
                };
                var result = client.BodyAnalysis(image);
                List<Person> personList = new List<Person>();
                if (result["person_num"] != null)
                {
                    var person_num = int.Parse(result["person_num"].ToString());
                    for (int i = 0; i < person_num; i++)
                    {
                        var person = new Person();
                        person.Left = double.Parse(result["person_info"][i]["location"]["left"].ToString());
                        person.Top = double.Parse(result["person_info"][i]["location"]["top"].ToString());
                        person.Width = double.Parse(result["person_info"][i]["location"]["width"].ToString());
                        person.Height = double.Parse(result["person_info"][i]["location"]["height"].ToString());
                        person.Score = double.Parse(result["person_info"][i]["location"]["score"].ToString());
                        //过滤哪些不靠谱的人物
                        if (person.Width >= 100 && person.Height >= 100 && person.Score > config.AIScore)
                        {
                            person.NoseX = double.Parse(result["person_info"][i]["body_parts"]["nose"]["x"].ToString());
                            personList.Add(person);
                        }
                    }
                }
                //没有找到人脸
                if (personList.Count == 0)
                {
                    left = 0;
                }
                else if (personList.Count == 1)
                {
                    //左边距离够
                    if (personList[0].NoseX - width / 2 >= 0)
                    {
                        if (personList[0].NoseX - width / 2 + width <= baseImg.Width)
                        {
                            left = (int)(personList[0].NoseX - width / 2);
                        }
                        else
                        {
                            left = baseImg.Width - width;
                        }
                    }
                    else
                    {
                        left = 0;
                    }
                }else if (personList.Count == 2)
                {
                    //两个人物离的近
                    if (width - Math.Abs(personList[1].NoseX - personList[0].NoseX) >= width / 2)
                    {
                        left = (baseImg.Width - width) / 2;
                    }
                    else
                    {
                        double noseX = 0;
                        //比较谁的面积大
                        if (personList[1].Width * personList[1].Height > personList[0].Width * personList[0].Height)
                        {
                            noseX = personList[1].NoseX;
                        }
                        else
                        {
                            noseX = personList[0].NoseX;
                        }
                        //左边距离够
                        if (noseX - width / 2 >= 0)
                        {
                            if (noseX - width / 2 + width <= baseImg.Width)
                            {
                                left = (int)(noseX - width / 2);
                            }
                            else
                            {
                                left = baseImg.Width - width;
                            }
                        }
                        else
                        {
                            left = 0;
                        }
                    }
                }
                else if (personList.Count > 2)
                {
                    //如果多于2个人
                    if (personList.Count > 10)
                    {
                        //多数情况下是影片左边缩略详情图，还是取右边做封面
                        if (baseImg.Width / 2 > width)
                        {
                            left = baseImg.Width / 2 + (baseImg.Width / 2 - width) / 2;
                        }
                        else
                        {
                            left = (baseImg.Width - width) / 2;
                        }
                    }
                    else
                    {
                        personList.Sort((x, y) => x.Score.CompareTo(y.Score));
                        //排序后取中间人脸的坐标计算
                        var noseX = personList[personList.Count - 1].NoseX;
                        //左边距离够
                        if (noseX - width / 2 >= 0)
                        {
                            if (noseX - width / 2 + width <= baseImg.Width)
                            {
                                left = (int)(noseX - width / 2);
                            }
                            else
                            {
                                left = baseImg.Width - width;
                            }
                        }
                        else
                        {
                            left = 0;
                        }
                    }
                }

                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.Clear(Color.White);
                    g.DrawImage(baseImg, new Rectangle(0, 0, tempImage.Width, tempImage.Height), new Rectangle(left, 0, width, baseImg.Height), GraphicsUnit.Pixel);
                    tempImage.Save(coverPath, ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                baseImg.Dispose();
                tempImage.Dispose();
            }
        }

        /// <summary>
        /// 从背景图截取出封面
        /// </summary>
        /// <param name="backdropPath">背景图</param>
        /// <returns>处理完的新图像</returns>
        private void GetCover(string backdropPath, string coverPath)
        {
            var baseImg = Image.FromFile(backdropPath);
            var width = (int)(baseImg.Height * 0.7029702970297);
            var tempImage = new Bitmap(width, baseImg.Height);
            try
            {
                using (Graphics templateG = Graphics.FromImage(tempImage)) {
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(baseImg, new Rectangle(0, 0, tempImage.Width, tempImage.Height), new Rectangle(baseImg.Width - width, 0, width, baseImg.Height), GraphicsUnit.Pixel);
                    tempImage.Save(coverPath, ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                baseImg.Dispose(); 
                tempImage.Dispose();
            }
        }

        /// <summary>
        /// 裁剪封面
        /// </summary>
        /// <param name="videoPath">视频文件夹</param>
        /// <param name="backdropPath">背景图</param>
        /// <param name="config">用户配置</param>
        /// <returns></returns>
        internal void ClipCover(string videoPath, string backdropPath, bool noai = true)
        {
            var coverPath = $"{videoPath}{Path.DirectorySeparatorChar}Cover.png";
            try
            {
                if (!noai && config.AIClip)
                {
                    Log.Save($"从背景图使用百度AI智能裁剪封面");
                    //从背景图识别智能裁剪封面
                    GetCoverByAI(backdropPath, coverPath);
                }
                else
                {
                    Log.Save($"从背景图固定裁剪封面");
                    //从背景图固定裁剪封面
                    GetCover(backdropPath, coverPath);
                }
            }
            catch (Exception e)
            {
                Log.Save($"裁剪封面出错 [{backdropPath}] {e.Message}");
                throw e;
            }
        }
    }
}