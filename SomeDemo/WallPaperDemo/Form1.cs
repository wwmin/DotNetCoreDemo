using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WallPaperDemo.Properties;
using WallPaperDemo.Util;

namespace WallPaperDemo
{
    public partial class MainForm : Form
    {
        private static string ImageUrl = @"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-cn";
        private static string ImagePath = @"images/";
        private static string ImageOriginPath = @"origin/";
        private static string ImageWallPaperPath = @"wallpaper/";
        private static bool IsCopyToClipboard = false;
        #region 设置背景图片
        /// <summary>
        /// 获取并解析图片路径
        /// </summary>
        /// <returns></returns>
        static async Task<(string Url, string Name)> GetImagePath(string url)
        {
            var name = DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString("n") + ".jpg";
            if (!url.StartsWith("http"))
            {
                return (url, name);
            }
            var http = new HttpClient();
            string content = await http.GetStringAsync(url);
            JToken json = JToken.Parse(content);
            IEnumerable<JToken> images = json["images"];
            string pictureUrl = images.Select(x => x["url"].ToString())
                .Select(x => "https://cn.bing.com" + x).First();
            string hsh = images.Select(x => name).First();
            return (pictureUrl, hsh);
        }
        /// <summary>
        /// 下载图片到指定目录
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static async Task<(string fullPath, string name, string imageDirectory)> DownloadImage(string url, string name)
        {
            var http = new HttpClient();
            //var fileName = Path.GetTempFileName();
            string rootPath = Directory.GetCurrentDirectory() + @"/" + ImagePath;
            string prefixPath = ImageOriginPath;
            string fileName = name;
            if (!Directory.Exists(rootPath + prefixPath))
            {
                Directory.CreateDirectory(rootPath + prefixPath);
            }
            string fullPath = rootPath + prefixPath + fileName;
            //var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"/images/") + DateTime.Now.ToFileTimeUtc();
            if (url.StartsWith("http"))
            {
                File.WriteAllBytes(fullPath, await http.GetByteArrayAsync(url));
            }
            else
            {
                File.Copy(url, fullPath, true);
            }
            return (fullPath, name, rootPath);
        }

        /// <summary>
        /// 制作背景图片
        /// </summary>
        /// <param name="pictureFileFullPath"></param>
        /// <param name="fileDirectory"></param>
        /// <param name="textContent"></param>
        /// <returns></returns>
        static string GenerateWallPaper(string pictureFileFullPath, string fileDirectory, string textContent)
        {

            var wic = new SharpDX.WIC.ImagingFactory2();
            var d2d = new SharpDX.Direct2D1.Factory();
            float dpi = d2d.DesktopDpi.Width;
            Size2 size = new Size2(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            SharpDX.WIC.FormatConverter image = CreateWicImage(wic, pictureFileFullPath);
            using var wicBitmap = new SharpDX.WIC.Bitmap(wic, size.Width, size.Height, SharpDX.WIC.PixelFormat.Format32bppPBGRA, SharpDX.WIC.BitmapCreateCacheOption.CacheOnDemand);
            using var target = new SharpDX.Direct2D1.WicRenderTarget(d2d, wicBitmap, new SharpDX.Direct2D1.RenderTargetProperties());
            using var dc = target.QueryInterface<SharpDX.Direct2D1.DeviceContext>();
            using var bmpPicture = SharpDX.Direct2D1.Bitmap.FromWicBitmap(target, image);
            using var dwriteFactory = new SharpDX.DirectWrite.Factory();
            using var brush = new SolidColorBrush(target, SharpDX.Color.LightGoldenrodYellow);
            using var bmpLayer = new SharpDX.Direct2D1.Bitmap1(dc, target.PixelSize,
new SharpDX.Direct2D1.BitmapProperties1(new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied), dpi, dpi, SharpDX.Direct2D1.BitmapOptions.Target));
            var oldTarget = dc.Target;
            dc.Target = bmpLayer;
            target.BeginDraw();
            {
                var textFormat = new SharpDX.DirectWrite.TextFormat(dwriteFactory, "Tahoma", size.Height / 27);

                // draw textContent
                {
                    var textLayout = new SharpDX.DirectWrite.TextLayout(dwriteFactory, textContent, textFormat, target.Size.Width * 0.75f, float.MaxValue);
                    var center = new Vector2((target.Size.Width - textLayout.Metrics.Width) / 2, (target.Size.Height - textLayout.Metrics.Height) / 2);
                    target.DrawTextLayout(new Vector2(center.X, center.Y), textLayout, brush);
                }
                //{
                //    // draw otherContent
                //    var textLayout = new SharpDX.DirectWrite.TextLayout(dwriteFactory, chinese, textFormat, target.Size.Width * 0.75f, float.MaxValue);
                //    var center = new Vector2((target.Size.Width - textLayout.Metrics.Width) / 2, target.Size.Height - textLayout.Metrics.Height - size.Height / 18);
                //    target.DrawTextLayout(new Vector2(center.X, center.Y), textLayout, brush);
                //}
            }
            target.EndDraw();

            // shadow
            var shadow = new SharpDX.Direct2D1.Effects.Shadow(dc);
            shadow.SetInput(0, bmpLayer, new RawBool(false));

            dc.Target = oldTarget;
            target.BeginDraw();
            {
                target.DrawBitmap(bmpPicture, new SharpDX.RectangleF(0, 0, target.Size.Width, target.Size.Height), 1.0f, BitmapInterpolationMode.Linear);
                dc.DrawImage(shadow, new Vector2(size.Height / 150.0f, size.Height / 150.0f));
                dc.UnitMode = UnitMode.Pixels;
                target.DrawBitmap(bmpLayer, 1.0f, BitmapInterpolationMode.Linear);
            }
            target.EndDraw();
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            string wallpaperFileName = fileDirectory + DateTime.Now.ToString("yyyyMMdd") + "_" + "wallpaper.png";
            using var wallpaperStream = File.OpenWrite(wallpaperFileName);
            SaveD2DBitmap(wic, wicBitmap, wallpaperStream);
            wallpaperStream.Close();
            return wallpaperFileName;
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="wicFactory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static SharpDX.WIC.FormatConverter CreateWicImage(SharpDX.WIC.ImagingFactory wicFactory, string fileName)
        {
            using var decoder = new SharpDX.WIC.JpegBitmapDecoder(wicFactory);
            using var decodeStream = new SharpDX.WIC.WICStream(wicFactory, fileName, SharpDX.IO.NativeFileAccess.Read);
            decoder.Initialize(decodeStream, SharpDX.WIC.DecodeOptions.CacheOnLoad);
            using var decodeFrame = decoder.GetFrame(0);
            var converter = new SharpDX.WIC.FormatConverter(wicFactory);
            converter.Initialize(decodeFrame, SharpDX.WIC.PixelFormat.Format32bppPBGRA);
            return converter;

        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="wicFactory"></param>
        /// <param name="wicBitmap"></param>
        /// <param name="outputStream"></param>
        static void SaveD2DBitmap(SharpDX.WIC.ImagingFactory wicFactory, SharpDX.WIC.Bitmap wicBitmap, Stream outputStream)
        {
            using var encoder = new SharpDX.WIC.BitmapEncoder(wicFactory, SharpDX.WIC.ContainerFormatGuids.Png);
            encoder.Initialize(outputStream);
            using var frame = new SharpDX.WIC.BitmapFrameEncode(encoder);
            frame.Initialize();
            frame.SetSize(wicBitmap.Size.Width, wicBitmap.Size.Height);

            var pixelFormat = wicBitmap.PixelFormat;
            frame.SetPixelFormat(ref pixelFormat);
            frame.WriteSource(wicBitmap);
            frame.Commit();
            encoder.Commit();
        }

        public sealed class Wallpaper
        {
            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            public enum Style : int
            {
                Tiled,
                Centered,
                Stretched
            }

            public static void Set(string pictureFileName, Style style)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (style == Style.Stretched)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Centered)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Tiled)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }

                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    pictureFileName,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
        #endregion
        #region 控件相关
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.mainNotifyIcon.Visible = true;
            InitView();
        }

        private void InitView()
        {
            textBox_imageUrl.Text = ImageUrl;
            ToolTip tip = new ToolTip();
            //textBox_imageUrl.ShowToolTip(tip, "imageUrl");
            textBox_imageUrl.MouseHover += (object sender, EventArgs e) => textBox_imageUrl.ShowToolTip(tip, "图片路径可以为网络图片url, 也可以时本地图片路径,可拖拽图片到此输入框,并使用此图片", 4, 15, 5000);

            textBox_imageText.Text = "Hello,World!";
            checkBox_copyToClipBoard.MouseHover += (object sender, EventArgs e) => checkBox_copyToClipBoard.ShowToolTip(tip, "勾选上之后可将渲染好的图片拷贝到剪切板", 2, 10, 5000);
            this.checkBox_copyToClipBoard.Checked = Settings.Default.IsCopyToClipboard;

            button_setwall.MouseHover += (object sender, EventArgs e) => button_setwall.ShowToolTip(tip, "将图片加上文字之后设置为桌面背景,图片可在此应用的image文件夹中找到", 4, 10, 5000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var url = textBox_imageUrl.Text;
            var text = textBox_imageText.Text;
            SetWallPaper(url, text);
        }

        private async void SetWallPaper(string imagePath, string fileContent)
        {
            var image = await GetImagePath(imagePath);
            var file = await DownloadImage(image.Url, image.Name);
            var wallPaperFileName = GenerateWallPaper(file.fullPath, file.imageDirectory + ImageWallPaperPath, !string.IsNullOrWhiteSpace(fileContent) ? fileContent : "hello world.");
            CopyToClipboard(wallPaperFileName);
            Wallpaper.Set(wallPaperFileName, Wallpaper.Style.Centered);
        }

        private static void CopyToClipboard(string fullPath)
        {
            if (IsCopyToClipboard)
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(fullPath);
                Clipboard.SetImage(image);
            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_setwall.Focus();
                this.button1_Click(sender, e);//触发button事件
            }
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxExt1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxExt1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            textBox_imageUrl.Text = path;
        }
        //  只有Form_Closing事件中 e.Cancel可以用。
        //  你的是Form_Closed事件。 Form_Closed事件时窗口已关了 ，Cancel没用了；
        //  Form_Closing是窗口即将关闭时询问你是不是真的关闭才有Cancel事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true;

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.mainNotifyIcon.Visible = true;
                this.Hide();
                return;
            }
        }

        /// <summary>
        /// 双击托盘 打开主窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.mainNotifyIcon.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void ToolStripMenuItemMaximize_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //this.mainNotifyIcon.Visible = true;
            //this.Show();
        }

        private void ToolStripMenuItemMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.mainNotifyIcon.Visible = true;
            this.Show();
        }

        private void ToolStripMenuItemNormal_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.mainNotifyIcon.Visible = true;
            this.Show();
        }

        private void ToolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.mainNotifyIcon.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }

        private void checkBox_copyToClipBoard_CheckedChanged(object sender, EventArgs e)
        {
            IsCopyToClipboard = checkBox_copyToClipBoard.Checked;
            Settings.Default.IsCopyToClipboard = IsCopyToClipboard;
            Settings.Default.Save();
        }
    }

    public class TextBoxExt : TextBox
    {
        public string PlaceHolder { get; set; }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0xF || m.Msg == 0x133)
            {
                WmPaint(ref m);
            }
        }

        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            if (!string.IsNullOrEmpty(this.PlaceHolder))
            {
                g.DrawString(this.PlaceHolder, this.Font, new SolidBrush(System.Drawing.Color.LightGray), 0, 0);
            }
        }
    }
    #endregion
}
