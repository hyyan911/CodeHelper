using MathLib.NormalMath.Decimal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

//文件处理类
namespace CodeHelper
{
    public class FileObject
    {
        /// <summary>
        /// 内容描述
        /// </summary>
        public Dictionary<string, string> Descriptions { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 文件数据
        /// </summary>
        internal List<FileData> DataAssemble { get; set; } = new List<FileData>();

        /// <summary>
        /// 从给定文件中读取，如果过程失败则抛出错误
        /// </summary>
        public static FileObject ReadFromFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("指定路径" + filepath + "的文件未找到");
            }
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(filepath)))
                {
                    FileObject obj = new FileObject();
                    string str = reader.ReadToEnd();
                    List<string> segs = str.Trim().Split('\n').ToList();
                    if (segs.Last() != "end of file") throw new FileFormatException("文件格式损坏：内容不完整");
                    segs.RemoveAt(segs.Count - 1);
                    bool IsIndata = false;
                    for (int i = 0; i < segs.Count; i++)
                    {
                        if (segs[i].Contains("data name line"))
                        {
                            List<string> datanames = segs[i].Trim().Split('★').ToList();
                            datanames.RemoveAt(0);
                            obj.DataNames = datanames;
                            for (int i2 = 0; i2 < datanames.Count; i2++)
                            {
                                obj.DataAssemble.Add(new FileData(datanames[i2], new List<string>()));
                            }
                            continue;
                        }
                        if (segs[i].Contains("userdata description ending line"))
                        {
                            IsIndata = true;
                            continue;
                        }
                        if (!IsIndata)
                        {
                            //处理描述内容
                            string[] dess = segs[i].Split('★');
                            obj.Descriptions.Add(dess[0].Trim(), dess[1].Trim());
                        }
                        if (IsIndata)
                        {
                            //处理描述内容
                            string[] dess = segs[i].Trim().Split('★');
                            if (dess.Length != obj.DataAssemble.Count) throw new FileLoadException("文件数据部分格式错误：数据不等长");
                            for (int i2 = 0; i2 < obj.DataAssemble.Count; i2++)
                            {
                                if (dess[i2].Trim() != "❤")
                                    obj.DataAssemble[i2].Data.Add(dess[i2].Trim());
                            }
                        }
                    }
                    return obj;
                }
            }
            catch (Exception exc)
            {
                throw new FileLoadException("指定文件加载失败，原因：" + exc.Message);
            }
        }

        /// <summary>
        /// 只读取文件描述
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ReadDescription(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("指定路径" + filepath + "的文件未找到");
            }
            try
            {
                Dictionary<string, string> des = new Dictionary<string, string>();
                using (StreamReader reader = new StreamReader(File.OpenRead(filepath)))
                {
                    FileObject obj = new FileObject();
                    string str = reader.ReadToEnd();
                    List<string> segs = str.Trim().Split('\n').ToList();
                    if (segs.Last() != "end of file") throw new FileFormatException("文件格式损坏：内容不完整");
                    segs.RemoveAt(segs.Count - 1);
                    bool IsIndata = false;
                    for (int i = 0; i < segs.Count; i++)
                    {
                        if (segs[i].Contains("userdata description ending line"))
                        {
                            return des;
                        }
                        if (!IsIndata)
                        {
                            //处理描述内容
                            string[] dess = segs[i].Split('★');
                            des.Add(dess[0].Trim(), dess[1].Trim());
                        }
                    }
                }
                return des;
            }
            catch (Exception exc)
            {
                throw new FileLoadException("指定文件加载失败，原因：" + exc.Message);
            }
        }

        List<string> DataNames { get; set; } = new List<string>();

        /// <summary>
        /// 获取所有数据名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataNames()
        {
            return new List<string>(DataNames.ToArray());
        }

        /// <summary>
        /// 保存到指定文件
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveToFile(string filepath)
        {
            if (!filepath.Contains(".userdat"))
            {
                filepath += ".userdat";
            }
            using (StreamWriter sw = new StreamWriter(new FileStream(filepath, FileMode.Create)))
            {
                foreach (var disc in Descriptions)
                {
                    if (disc.Key.Contains("★") || disc.Key.Contains("★"))
                    {
                        throw new FileFormatException("★为限定符，请确保文件的描述和数据中不包含★字符");
                    }
                    if (disc.Key.Contains("❤") || disc.Key.Contains("❤"))
                    {
                        throw new FileFormatException("❤为限定符，请确保文件的描述和数据中不包含❤字符");
                    }
                    if (disc.Key.Contains("☯") || disc.Key.Contains("☯"))
                    {
                        throw new FileFormatException("☯为限定符，请确保文件的描述和数据中不包含☯字符");
                    }
                    if (string.IsNullOrEmpty(disc.Key) || string.IsNullOrEmpty(disc.Key))
                    {
                        throw new FileFormatException("文件描述中不能存在空字符串");
                    }
                    sw.WriteLine(disc.Key + "★" + disc.Value);
                }
                sw.WriteLine("userdata description ending line");
                int datacount = 0;
                string datanames = "data name line" + "★";
                for (int i = 0; i < DataAssemble.Count; i++)
                {
                    if (DataAssemble[i].Name.Contains("★"))
                        throw new FileFormatException("★为限定符，请确保文件的描述和数据中不包含★字符");
                    if (DataAssemble[i].Name.Contains("❤"))
                        throw new FileFormatException("❤为限定符，请确保文件的描述和数据中不包含❤字符");
                    if (DataAssemble[i].Name.Contains("☯"))
                        throw new FileFormatException("☯为限定符，请确保文件的描述和数据中不包含☯字符");
                    if (string.IsNullOrEmpty(DataAssemble[i].Name))
                        throw new FileFormatException("数据集名称中不能存在空字符串");
                    datanames += DataAssemble[i].Name + "★";
                }
                datanames = datanames.Remove(datanames.Length - 1, 1);
                sw.WriteLine(datanames);
                List<int> maxcount = new List<int>() { 0 };
                foreach (var item in DataAssemble)
                {
                    maxcount.Add(item.Data.Count);
                }
                datacount = maxcount.Max();
                for (int i = 0; i < datacount; i++)
                {
                    string tempseg = "";
                    for (int j = 0; j < DataAssemble.Count; j++)
                    {
                        if (i > DataAssemble[j].Data.Count - 1)
                        {
                            tempseg += "❤" + "★";
                        }
                        else
                        {
                            tempseg += DataAssemble[j].Data[i] + "★";
                        }
                    }
                    tempseg = tempseg.Remove(tempseg.Length - 1, 1);
                    sw.WriteLine(tempseg);
                }

                sw.WriteLine("end of file");
            }
        }

        /// <summary>
        /// 打开文件浏览器寻找文件
        /// </summary>
        public static FileObject FindFileFromExplorer(string rootPath = "")
        {
            OpenFileDialog dia = new OpenFileDialog();
            if (rootPath != null) dia.InitialDirectory = rootPath;
            dia.Filter = "自定义文件(*.userdat)|*.userdat";
            if (DialogResult.OK == dia.ShowDialog())
            {
                return ReadFromFile(dia.FileName);
            }
            return null;
        }

        /// <summary>
        /// 打开文件浏览器保存文件
        /// </summary>
        public bool SaveFileFromExplorer(string rootPath = "", string defaultname = "")
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.FileName = defaultname;
            dia.Filter = "自定义文件(*.userdat)|*.userdat";
            if (rootPath != null) dia.InitialDirectory = rootPath;
            if (DialogResult.OK == dia.ShowDialog())
            {
                SaveToFile(dia.FileName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 写入浮点数到数据集
        /// </summary>
        /// <param name="dataname"></param>
        /// <param name="data"></param>
        public void WriteDoubleData(string dataname, List<double> data)
        {
            List<string> res = new List<string>();
            foreach (var item in data)
            {
                res.Add(item.ToString());
            }
            DataAssemble.Add(new FileData(dataname, res));
        }

        /// <summary>
        /// 写入布尔型到数据集
        /// </summary>
        /// <param name="dataname"></param>
        /// <param name="data"></param>
        public void WriteBooleanData(string dataname, List<bool> data)
        {
            List<string> res = new List<string>();
            foreach (var item in data)
            {
                res.Add(item.ToString());
            }
            DataAssemble.Add(new FileData(dataname, res));
        }

        /// <summary>
        /// 写入点数据
        /// </summary>
        public void WritePointData(string dataname, List<RealPoint> data)
        {
            List<string> res = new List<string>();
            foreach (var item in data)
            {
                string str = "";
                foreach (var value in item.Content)
                {
                    str += value.ToString() + "☯";
                }
                if (str != "")
                {
                    str = str.Remove(str.Length - 1, 1);
                }
                res.Add(str);
            }
            DataAssemble.Add(new FileData(dataname, res));
        }

        /// <summary>
        /// 写入字符串到数据集
        /// </summary>
        /// <param name="dataname"></param>
        /// <param name="data"></param>
        public void WriteStringData(string dataname, List<string> data)
        {
            List<string> res = new List<string>();
            foreach (var item in data)
            {
                res.Add(item);
            }
            DataAssemble.Add(new FileData(dataname, res));
        }

        /// <summary>
        /// 写入日期到数据集
        /// </summary>
        /// <param name="dataname"></param>
        /// <param name="data"></param>
        public void WriteDateData(string dataname, List<DateTime> data)
        {
            List<string> res = new List<string>();
            foreach (var item in data)
            {
                res.Add(item.ToString("yyyy:MMMM:dd:HH:mm:ss:FFF").ToString());
            }
            DataAssemble.Add(new FileData(dataname, res));
        }

        /// <summary>
        /// 从数据集中抽取浮点数据
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public List<double> ExtractDouble(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    List<double> res = new List<double>();
                    for (int j = 0; j < DataAssemble[i].GetCount(); j++)
                    {
                        if (DataAssemble[i].Data[j].Contains(":"))
                        {
                            throw new FormatException("指定数据集是日期格式,无法转换成double类型");
                        }
                        res.Add(double.Parse(DataAssemble[i].Data[j]));
                    }
                    return res;
                }
            }
            return new List<double>();
        }

        /// <summary>
        /// 从数据集中抽取布尔数据
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public List<bool> ExtractBoolean(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    List<bool> res = new List<bool>();
                    for (int j = 0; j < DataAssemble[i].GetCount(); j++)
                    {
                        res.Add(bool.Parse(DataAssemble[i].Data[j]));
                    }
                    return res;
                }
            }
            return new List<bool>();
        }

        /// <summary>
        /// 从数据集中抽取字符串
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public List<string> ExtractString(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    return DataAssemble[i].Data;
                }
            }
            return new List<string>();
        }


        /// <summary>
        /// 从数据集中抽取日期
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public List<DateTime> ExtractDate(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    List<DateTime> res = new List<DateTime>();
                    for (int j = 0; j < DataAssemble[i].GetCount(); j++)
                    {
                        if (DataAssemble[i].Data[j].Contains(":") == false)
                        {
                            throw new FormatException("指定数据集不是日期格式");
                        }
                        res.Add(DateTime.ParseExact(DataAssemble[i].Data[j], "yyyy:MMMM:dd:HH:mm:ss:FFF", CultureInfo.CurrentCulture));
                    }
                    return res;
                }
            }
            return new List<DateTime>();
        }


        /// <summary>
        /// 从数据集中抽取点
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public List<RealPoint> ExtractPoint(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    List<RealPoint> res = new List<RealPoint>();
                    for (int j = 0; j < DataAssemble[i].GetCount(); j++)
                    {
                        if (DataAssemble[i].Data[j].Contains("☯") == false)
                        {
                            continue;
                        }
                        string[] pointseg = DataAssemble[i].Data[j].Split('☯');
                        RealPoint p = new RealPoint();
                        foreach (var item in pointseg)
                        {
                            try
                            {
                                p.Content.Add(double.Parse(item));
                            }
                            catch (Exception) { }
                        }
                        res.Add(p);
                    }
                    return res;
                }
            }
            return new List<RealPoint>();
        }

        /// <summary>
        /// 获取数据个数
        /// </summary>
        /// <returns></returns>
        public int DataCount()
        {
            if (DataAssemble.Count == 0) return 0;
            return DataAssemble[0].GetCount();
        }

        /// <summary>
        /// 判断数据类型(找不到数据返回null)
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public Type JudgeDataType(string dataname)
        {
            for (int i = 0; i < DataAssemble.Count; i++)
            {
                if (DataAssemble[i].Name == dataname)
                {
                    if (DataAssemble[i].Data.Count == 0) return null;
                    try
                    {
                        double.Parse(DataAssemble[i].Data[0]);
                        return typeof(double);
                    }
                    catch (Exception) { }

                    try
                    {
                        bool.Parse(DataAssemble[i].Data[0]);
                        return typeof(bool);
                    }
                    catch (Exception) { }

                    if (DataAssemble[i].Data[0].Contains("☯"))
                    {
                        return typeof(RealPoint);
                    }

                    try
                    {
                        DateTime.ParseExact(DataAssemble[i].Data[0], "yyyy:MMMM:dd:HH:mm:ss:FFF", CultureInfo.CurrentCulture);
                        return typeof(DateTime);
                    }
                    catch (Exception) { }

                    return typeof(string);
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 程序数据集
    /// </summary>
    internal class FileData
    {
        /// <summary>
        /// 数据集名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 数据内容
        /// </summary>
        public List<string> Data { get; set; } = new List<string>();

        public FileData(string name, List<string> data)
        {
            Name = name;
            Data = data;
        }

        /// <summary>
        ///获取数据个数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Data.Count;
        }
    }
}
