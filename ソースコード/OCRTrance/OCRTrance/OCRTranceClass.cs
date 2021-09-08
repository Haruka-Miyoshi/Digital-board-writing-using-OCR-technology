using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Tesseract;          //もしこのプログラムを改造する場合はTesseractを予めインストールする必要性があります。
using Windows.Media.Ocr;  //もしこのプログラムを改造する場合はWindows.Media.Ocrを予めインストールする必要性があります。
using System.IO;          //システムIOは基本的にディレクトリなどをプログラム文で使用する際に必要です。

namespace OCRTrance
{
    class OCRTranceClass
    {
        //*********************************************************
        //****OCRライブラリによる文字認識処理**********************
        //*********************************************************

        //****ImageFileをOCRにより文字認識する*********************
        public void OCRTest1(string CreateFileDirectoryBox, string CreateFileNameBox1,string TranceFileNameBox,bool Check)
        {
            OCRTranceToul OCRTT = new OCRTranceToul();

            string Path1 = @"tessdata";                                                     //tessdataファイルディレクトリ

            string Path2 = CreateFileDirectoryBox + "\\" + CreateFileNameBox1 + ".txt";    //txt生成ディレクトリ

            var image = new Bitmap(TranceFileNameBox);                                     //選択した画像ファイル形式をbitmapに変換する

            //****文字認識設定engのとき
            if (Check == true)
            {
                using (var tesseract = new Tesseract.TesseractEngine(Path1, "eng"))
                {
                    Tesseract.Page page = tesseract.Process(image);  //OCRにより画像ファイルから文字を読み取る

                    File.WriteAllText(Path2, page.GetText());  //文字読取を行った文字列をPath2の生成と同時に書き込む
                }
            }

            //****文字認識設定jpnのとき
            else
            {
                using (var tesseract = new Tesseract.TesseractEngine(Path1, "jpn"))
                {
                    Tesseract.Page page = tesseract.Process(image);  //OCRにより画像ファイルから文字を読み取る

                    File.WriteAllText(Path2, page.GetText());  //文字読取を行った文字列をPath2の生成と同時に書き込む
                }
            }

            //****完了のメッセージボックス
            DialogResult result = MessageBox.Show("Complete", "Read Complete", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        //*********************************************************

        //****その場で全体画面をキャプチャして変換する場合*********
        public void OcrTransferCapture(string CreateFileDirectoryBox,string CreateFileNameBox2,string Path3,bool Check)
        { 

            OCRTranceToul OCRT = new OCRTranceToul();

            Thread.Sleep(200);  //200us待機(キャプチャ時のフォーム非表示のため)

            string Path1 = @"tessdata";                                                    //tessdataファイルディレクトリ

            string Path2 = CreateFileDirectoryBox + "\\" + CreateFileNameBox2 + ".txt";    //txt生成ディレクトリ

            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics screen = Graphics.FromImage(image);

            screen.CopyFromScreen(new Point(0, 0), new Point(0, 0), image.Size);

            //****文字認識設定engのとき
            if (Check == true)
            {
                using (var tesseract = new Tesseract.TesseractEngine(Path1, "eng"))
                {
                    Tesseract.Page page = tesseract.Process(image);  //OCRにより画像ファイルから文字を読み取る

                    File.WriteAllText(Path2, page.GetText());  //文字読取を行った文字列をPath2の生成と同時に書き込む
                }
            }

            //****文字認識設定jpnのとき
            else
            {
                using (var tesseract = new Tesseract.TesseractEngine(Path1, "jpn"))
                {
                    Tesseract.Page page = tesseract.Process(image);  //OCRにより画像ファイルから文字を読み取る

                    File.WriteAllText(Path2, page.GetText());  //文字読取を行った文字列をPath2の生成と同時に書き込む
                }
            }

            //OCRT.TrancePictureBox.Image = null;

            image.Save(Path3, System.Drawing.Imaging.ImageFormat.Png);

            screen.Dispose();

        }
        //*********************************************************

        //*********************************************************
        //*********************************************************
        //*********************************************************
    }
}
