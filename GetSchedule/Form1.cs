using System;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;

namespace GetSchedule
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetSchedule_Click(object sender, EventArgs e)
        {
            output.Clear();

            string fdate = dateFrom.Value.Date.ToShortDateString();
            string tdate = dateTo.Value.Date.ToShortDateString();

            // 日付チェック
            if (fdate.CompareTo(tdate).Equals(1))
            {
                MessageBox.Show("To日付はFrom日付より新しい日付でなければなりません。");
                return;
            } 

            // スケジュール取得
            Microsoft.Office.Interop.Outlook.Application outlook = new Microsoft.Office.Interop.Outlook.Application();
            NameSpace ns = outlook.GetNamespace("MAPI");
            MAPIFolder oFolder = ns.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);
            //output.Text += oFolder.Name + "\r\n\r\n---\r\n";
            output.Text += "作業日報" + "\r\n\r\n-----\r\n";

            Items oItems = oFolder.Items;
            AppointmentItem oAppoint = oItems.GetFirst();

            string startDate;
            string endDate;
            bool schedule = false;

            while (oAppoint != null)
            {
                startDate = oAppoint.Start.ToShortDateString();
                endDate = oAppoint.End.ToShortDateString();

                // 指定した日付の範囲の予定表を抽出
                if ((startDate.CompareTo(fdate).Equals(0) || startDate.CompareTo(fdate).Equals(1))
                    && (tdate.CompareTo(endDate).Equals(0) || tdate.CompareTo(endDate).Equals(1)))
                {
                    output.Text += "件名    ：  " + oAppoint.Subject + "\r\n";
                    output.Text += "開始時刻：  " + oAppoint.Start.ToString("yyyy/MM/dd hh:mm:ss") + "\r\n";
                    output.Text += "終了時刻：  " + oAppoint.End.ToString("yyyy/MM/dd hh:mm:ss") + "\r\n";
                    output.Text += "内容　　：　\r\n" + oAppoint.Body + "\r\n";
                    output.Text += "\r\n-----\r\n";
                    schedule = true;
                }
                oAppoint = oItems.GetNext();
            }

            // スケジュール登録がない場合
            if (!schedule)
            {
                output.Text += "指定した日付の日報はありません。";
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

    }
}
