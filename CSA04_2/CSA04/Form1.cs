using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSA04
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, string> record = new Dictionary<string, string>();
        List<DateTime> list_date = new List<DateTime>();
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now.AddDays(1);
            dateTimePicker2.MinDate = DateTime.Now.AddDays(2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!record.ContainsKey(dateTimePicker1.Value.ToString($"yyyy年MM月dd日")+"-") && dateTimePicker2.Value > dateTimePicker1.Value && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(comboBox2.Text))
            {
                bool tr_fa = true;
                foreach(var date in list_date)
                {
                    if (dateTimePicker1.Value.Date == date.Date || dateTimePicker2.Value.Date == date.Date)
                    {
                        tr_fa = false;
                        break;
                    }
                }
                if(tr_fa)
                {
                    record.Add(dateTimePicker1.Value.ToString("yyyy年MM月dd日") + "-", $"{dateTimePicker2.Value.ToString($"yyyy年MM月dd日")}-{comboBox1.Text} 付現：" + (radioButton1.Checked ? "是" : "否") + $" {comboBox2.Text}");
                    listBox1.Items.Add(dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-" + record[dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-"]);

                    DateTime tempDate = dateTimePicker1.Value;
                    while (tempDate <= dateTimePicker2.Value) 
                    {
                        list_date.Add(tempDate);
                        tempDate = tempDate.AddDays(1);
                    }
                    comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                    MessageBox.Show("新增成功");
                }
                else
                {
                    MessageBox.Show("日期錯誤");
                }
            }
            else
            {
                MessageBox.Show("資料錯誤");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (record.ContainsKey(dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-"))
            {
                listBox1.Items.Clear();
                listBox1.Items.Add(dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-" + record[dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-"]);
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                MessageBox.Show("查詢成功");
            }
            else
            {
                MessageBox.Show("無相同資料");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var item in record)
            {
                listBox1.Items.Add(item.Key + item.Value);
            }
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            MessageBox.Show("資訊重載成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (record.ContainsKey(dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-") && dateTimePicker2.Value > dateTimePicker1.Value && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(comboBox2.Text))
            {
                record.Remove(dateTimePicker1.Value.ToString($"yyyy年MM月dd日") + "-");
                listBox1.Items.Clear();
                foreach (var item in record)
                {
                    listBox1.Items.Add(item.Key + item.Value);
                }

                DateTime tempDate = dateTimePicker1.Value;
                while (tempDate <= dateTimePicker2.Value)
                {
                    list_date.Add(tempDate);
                    tempDate = tempDate.AddDays(1);
                }

                DateTime tempDate1 = dateTimePicker1.Value;
                while (tempDate1 <= dateTimePicker2.Value)
                {
                    list_date.Remove(tempDate1);
                    tempDate1 = tempDate1.AddDays(1);
                }
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                MessageBox.Show("刪除成功");
            }
            else
            {
                MessageBox.Show("資料錯誤");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            List<string> list = new List<string>();
            foreach (var item in record)
            {
                list.Add(item.Key);
            }
            list.Sort();
            foreach (var item in list)
            {
                listBox1.Items.Add(item + record[item]);
            }
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
            MessageBox.Show("排序成功");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> list = File.ReadAllLines(openFileDialog1.FileName).ToList();
                list.RemoveAt(0);
                foreach (var csv in list)
                {
                    string[] csvv = csv.Split(',');
                    DateTime one = DateTime.Parse(csvv[0]);
                    DateTime two = DateTime.Parse(csvv[1]);
                    if (csvv.Length == 5 && two > one && one > DateTime.Now)
                    {
                        bool tr_fa = true;
                        foreach (var date in list_date)
                        {
                            if (one.Date == date.Date || two.Date == date.Date)
                            {
                                tr_fa = false;
                                break;
                            }
                        }
                        if (tr_fa)
                        {
                            record.Add(csvv[0] + "-", $"{csvv[1]}-{csvv[2]} 付現：{csvv[3]} {csvv[4]}");
                            DateTime tempDate1 = one;
                            while (tempDate1 <= two)
                            {
                                list_date.Add(tempDate1);
                                tempDate1 = tempDate1.AddDays(1);
                            }
                        }
                    }
                }
                listBox1.Items.Clear();
                foreach (var item in record)
                {
                    listBox1.Items.Add(item.Key + item.Value);
                }
                MessageBox.Show("匯入成功");
            }
            else
            {
                MessageBox.Show("檔案選擇失敗");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(openFileDialog1.FileName, "入住日期,退房日期,人數,付款方式,房型\n");
                foreach(var item in record)
                {
                    File.AppendAllText(openFileDialog1.FileName,item.Key.Replace("-", ",") + item.Value.Replace(" ", ",").Replace("-", ",").Replace("付現：","")+"\n");
                }
                MessageBox.Show("匯出成功");
            }
            else
            {
                MessageBox.Show("檔案選擇失敗");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}
