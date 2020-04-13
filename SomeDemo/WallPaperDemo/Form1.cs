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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            GetImagePath().ContinueWith(e =>
            {
                Console.WriteLine(e.Result.Name + "," + e.Result.Url);
                DownloadImage(e.Result.Url, e.Result.Name).ContinueWith(file =>
                {
                    Console.WriteLine(file.Result.fullPath);
                    GenerateWallPaper(file.Result.fullPath, file.Result.imageDirectory, "hello world.");
                });
            });
            InitializeComponent();
        }

        /// <summary>
        /// 获取并解析图片路径
        /// </summary>
        /// <returns></returns>
        static async Task<(string Url, string Name)> GetImagePath()
        {
            var http = new HttpClient();
            string url = @"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-cn";
            string content = await http.GetStringAsync(url);
            JToken json = JToken.Parse(content);
            IEnumerable<JToken> images = json["images"];
            string pictureUrl = images.Select(x => x["url"].ToString())
                .Select(x => "https://cn.bing.com" + x).First();
            string hsh = images.Select(x => DateTime.Now.ToString("yyyyMMdd") + "_" + x["hsh"].ToString() + ".jpg").First();
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
            string rootPath = Directory.GetCurrentDirectory();
            string prefixPath = @"/images/";
            string fileName = name;
            if (!Directory.Exists(rootPath + prefixPath))
            {
                Directory.CreateDirectory(rootPath + prefixPath);
            }
            string fullPath = rootPath + prefixPath + fileName;
            //var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"/images/") + DateTime.Now.ToFileTimeUtc();
            File.WriteAllBytes(fullPath, await http.GetByteArrayAsync(url));
            return (fullPath, name, rootPath + prefixPath);
        }

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

            string wallpaperFileName = fileDirectory + DateTime.Now.ToString("yyyyMMdd") + "_" + "wallpaper.png";
            using var wallpaperStream = File.OpenWrite(wallpaperFileName);
            SaveD2DBitmap(wic, wicBitmap, wallpaperStream);
            wallpaperStream.Close();
            return wallpaperFileName;
        }

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
    }
}
