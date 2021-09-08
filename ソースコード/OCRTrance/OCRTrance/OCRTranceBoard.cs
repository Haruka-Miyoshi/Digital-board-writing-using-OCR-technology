using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Tesseract;          //もしこのプログラムを改造する場合はTesseractを予めインストールする必要性があります。
using Windows.Media.Ocr;  //もしこのプログラムを改造する場合はWindows.Media.Ocrを予めインストールする必要性があります。
using System.IO;          //システムIOは基本的にディレクトリなどをプログラム文で使用する際に必要です。

namespace OCRTrance
{
    public partial class OCRTranceToul : Form
    {

        //****キャプチャによるバックアップpngの保存を複数可能にするためのカウント
        private int i = 0;

        //*********************************************************
        //****exe起動時に実行する関数(8/27)************************
        //*********************************************************
        public OCRTranceToul()
        {
            InitializeComponent();

            //****前回の設定が保存されているファイル
            string SaveData = @"savedata.txt";

            //****savedata.txtが存在する場合、そのファイルを読取し、設定を確認する
            if (File.Exists(SaveData))
            {

                StreamReader sr = new StreamReader(SaveData, Encoding.GetEncoding("Shift_JIS"));

                string setting = sr.ReadToEnd();

                sr.Close();

                //****0eng0が含まれる場合はeng設定 0jpn1が含まれる場合はjpn設定
                if (setting.StartsWith("0eng0"))      //英語設定の場合の文字コードの判定
                {
                    setting = setting.Replace("0eng0", "");

                    engButton.Checked = true;

                    jpnButton.Checked = false;
                }
                else if (setting.StartsWith("0jpn1")) //日本語設定の場合の文字コードの判定
                {
                    setting = setting.Replace("0jpn1", "");

                    engButton.Checked = false;

                    jpnButton.Checked = true;
                }

                //****読み取った文字列に含まれる改行コードを削除する
                setting = setting.Replace("\r\n", "");

                //****読み取った設定をsettingに設定する
                CreateFileDirectoryBox.Text = setting;

            }
            //*****************************************************
        }
        //*********************************************************
        //*********************************************************
        //*********************************************************

        //*********************************************************
        //****ファイルシステム処理一覧(8/27)***********************
        //*********************************************************

        //****ファイル参照1****************************************
        private void ReferButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ImageFile = new OpenFileDialog();  //インスタンス作成(クラスOpenFileDialog)

            ImageFile.InitialDirectory = @"C:\";  //初期ディレクトリ

            ImageFile.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";

            ImageFile.Title = "Please select an image file.";  //説明文

            if (ImageFile.ShowDialog() == DialogResult.OK)
            {
                TranceFileNameBox.Text = ImageFile.FileName;

                MessageBox.Show("The selected folder is File Directory(" + (TranceFileNameBox.Text).ToString() + ").","Refer to Folder",MessageBoxButtons.OK,MessageBoxIcon.None);
            }
        }
        //*********************************************************

        //****ファイル参照2****************************************
        private void ReferButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Folder = new FolderBrowserDialog();  //インスタンス作成(クラスFolderBrowserDialog)

            Folder.SelectedPath = @"C:\";  //初期ディレクトリ

            Folder.Description = "Please select an Save file.";  //説明文

            Folder.ShowNewFolderButton = true;  //新しいフォルダの作成許可

            if (Folder.ShowDialog() == DialogResult.OK)
            {
                CreateFileDirectoryBox.Text = Folder.SelectedPath;

                MessageBox.Show("The selected folder is File Directory("+ (CreateFileDirectoryBox.Text).ToString() + ").","Refer to Image File",MessageBoxButtons.OK,MessageBoxIcon.None);
            }
        }
        //*********************************************************

        //*********************************************************
        //*********************************************************
        //*********************************************************

        //*********************************************************
        //****Tranceボタン イベント処理一覧(8/27)******************
        //*********************************************************

        //****Imageファイルを選択して文字認識を行う****************
        private void TranceButton_Click(object sender, EventArgs e)
        {
            try
            {
                //インスタンス生成
                OCRTranceClass OCRTC = new OCRTranceClass();

                //Imageファイルを選択して文字認識を行うClass
                OCRTC.OCRTest1(CreateFileDirectoryBox.Text, CreateFileNameBox1.Text, TranceFileNameBox.Text, engButton.Checked);

            }
            catch
            {
                //例外が発生した場合、エラーメッセージを表示する
                MessageBox.Show("Check your settings again.", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //*********************************************************

        //****全体画面のキャプチャから文字認識をするイベント*******
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                i++;  //バックアップ画像 保存のためにインクリメントする

                string Path3 = @"Backup" + (i).ToString() + ".png";  //キャプチャ画像ファイルのバックアップ

                this.Visible = false;  //フォームを非表示にする

                //Thread.Sleep(200); 
                //200us待機(キャプチャ時のフォーム非表示のため)

                OCRTranceClass OCRTC = new OCRTranceClass();

                OCRTC.OcrTransferCapture(CreateFileDirectoryBox.Text,CreateFileNameBox2.Text,Path3,engButton.Checked);

                this.Visible = true;

                TrancePictureBox.Image = System.Drawing.Image.FromFile(Path3);

                //****完了のメッセージボックス
                DialogResult result = MessageBox.Show("Read Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {
                //例外が発生した場合、エラーメッセージを表示する
                MessageBox.Show("Check your settings again.", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //*********************************************************

        //*********************************************************
        //*********************************************************
        //*********************************************************


        //*********************************************************
        //****設定保存ボタン(8/27)*********************************
        //*********************************************************
        private void SaveButton_Click(object sender, EventArgs e)
        {
            string SaveData = @"savedata.txt";  //設定を保存するdirectoryとFileName

            StreamWriter writer = new StreamWriter(SaveData, false, Encoding.GetEncoding("Shift_JIS"));

            if(engButton.Checked == true)
            {
                writer.WriteLine("0eng0" + CreateFileDirectoryBox.Text); //0eng0は文字認識による英語読取の文字コード
            }
            else
            {
                writer.WriteLine("0jpn1" + CreateFileDirectoryBox.Text); //0jpn1は文字認識による日本語読取の文字コード
            }
            
            writer.Close();

            //保存が完了したというメッセージ表示
            DialogResult result = MessageBox.Show("Save Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        //*********************************************************
        //*********************************************************
        //*********************************************************



        //*********************************************************
        //****閉じるボタン 処理一覧(8/27)**************************
        //*********************************************************

        //****閉じるボタン1****************************************
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();  //アプリケーションを閉じる
        }
        //*********************************************************

        //****閉じるボタン2****************************************
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();  //アプリケーションを閉じる
        }
        //*********************************************************

        //*********************************************************
        //*********************************************************
        //*********************************************************

    }
}
