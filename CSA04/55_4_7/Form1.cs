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

namespace _55_4_7
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, string> record = new Dictionary<string, string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Now.AddDays(1);
            dateTimePicker2.MinDate = DateTime.Now.AddDays(2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            DateTime dt2 = DateTime.Parse(dateTimePicker2.Text);
            if (comboBox1.Text != null && comboBox2.Text != null && dt < dt2)
            {
                if (!record.ContainsKey(dateTimePicker1.Text) )
                {
                    record.Add(dateTimePicker1.Text, "-" + comboBox1.Text + (radioButton1.Checked ? " 付現：是 " : " 付現：否 ") + comboBox2.Text + " 退房時間：" + dateTimePicker2.Text);
                    listBox1.Items.Add(dateTimePicker1.Text + record[dateTimePicker1.Text]);
                    comboBox1.SelectedIndex = comboBox2.SelectedIndex = -1;
                    MessageBox.Show("新增成功");
                }
                else
                {
                    MessageBox.Show("已有相同資料");
                }
            }
            else
            {
                MessageBox.Show("請選擇資料");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != null && comboBox2.Text != null)
            {
                if (record.ContainsKey(dateTimePicker1.Text))
                {
                    string[] old_rec = record[dateTimePicker1.Text].Split(' ');
                    DateTime dt = DateTime.Parse(dateTimePicker1.Text);
                    DateTime dt2 = DateTime.Parse(dateTimePicker2.Text);
                    if (old_rec.Length == 4 && dt < dt2)
                    {
                        record[dateTimePicker1.Text] = (string.IsNullOrEmpty(comboBox1.Text) ? old_rec[0] : "-" + comboBox1.Text) + (radioButton1.Checked ? " 付現：是 " : " 付現：否 ") + (string.IsNullOrEmpty(comboBox2.Text) ? old_rec[2] : comboBox2.Text) + " 退房時間：" + dateTimePicker2.Text;
                        listBox1.Items.Clear();
                        foreach (var items in record)
                        {
                            listBox1.Items.Add(items.Key + items.Value);
                        }
                        comboBox1.Text = null;
                        comboBox2.Text = null;
                        MessageBox.Show("修改成功");
                    }
                    else
                    {
                        MessageBox.Show("請選擇資料");
                    }
                }
                else
                {
                    MessageBox.Show("查無資料");
                }
            }
    }

        private void button4_Click(object sender, EventArgs e)
        {
            if (record.ContainsKey(dateTimePicker1.Text))
            {
                listBox1.Items.Clear();
                listBox1.Items.Add(dateTimePicker1.Text + record[dateTimePicker1.Text]);
                MessageBox.Show("查詢成功");
            }
            else
            {
                MessageBox.Show("查無資料");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var items in record)
            {
                listBox1.Items.Add(items.Key + items.Value);
            }
            MessageBox.Show("更新資料成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (record.ContainsKey(dateTimePicker1.Text))
            {
                record.Remove(dateTimePicker1.Text);
                listBox1.Items.Clear();
                foreach (var items in record)
                {
                    listBox1.Items.Add(items.Key + items.Value);
                }
                comboBox1.Text = comboBox2.Text = null;
                MessageBox.Show("刪除成功");
            }
            else
            {
                MessageBox.Show("查無資料");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> list_csv = File.ReadAllLines(openFileDialog1.FileName).ToList();
                list_csv.RemoveAt(0);
                foreach (var items in list_csv)
                {
                    string[] array_csv = items.Split('日');
                    array_csv[0] += "日";
                    array_csv[2] += "日";
                    array_csv[1]= array_csv[1].Remove(0,1);
                    array_csv[1] = array_csv[1].Replace("元,", "元,退房時間：");
                    if (array_csv.Length == 3)
                    {
                        DateTime time = DateTime.Parse(array_csv[0].Replace(",", ""), CultureInfo.InvariantCulture);
                        if (time >= DateTime.Now)
                        {
                            if (record.ContainsKey(array_csv[0].Replace(",", "")))
                            {
                                DialogResult result = MessageBox.Show("檢測相同資料，是否捨棄CSV檔資料", "錯誤", MessageBoxButtons.YesNo);
                                if (result != DialogResult.Yes)
                                {
                                    record[array_csv[0].Replace(",", "")] = $"-{array_csv[1].Replace(","," ")}{array_csv[2]}";
                                    listBox1.Items.Clear();
                                    foreach (var i in record)
                                    {
                                        listBox1.Items.Add(i.Key + i.Value);
                                    }
                                }
                            }
                            else
                            {
                                record.Add(array_csv[0].Replace(",", ""), $"-{array_csv[1].Replace(",", " ")}{array_csv[2]}");
                                listBox1.Items.Add(array_csv[0].Replace(",", "") + record[array_csv[0].Replace(",", "")]);
                            }
                        }
                    }
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(openFileDialog1.FileName, "入住日期,人數,支付方式,房型,退房日期\n");
                foreach (var items in record)
                {
                    File.AppendAllText(openFileDialog1.FileName, items.Key + "," + ((items.Value.Replace(" ", ",")).Replace("-", "")).Replace("退房時間：", "") + "\n");
                }
                MessageBox.Show("匯出成功");
            }
            else
            {
                MessageBox.Show("檔案選擇失敗");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var items in record)
            {
                list.Add(items.Key);
            }
            list.Sort();
            listBox1.Items.Clear();
            foreach (var items in list)
            {
                listBox1.Items.Add(items + record[items]);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
        }
    }
}
