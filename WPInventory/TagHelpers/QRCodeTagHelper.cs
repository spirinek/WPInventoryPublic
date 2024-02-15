﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using ZXing.QrCode;
using System.Drawing.Imaging;
using System.Drawing;


// https://forums.asp.net/post/6136236.aspx честно стырено у разработчика zxing, потому как обычная выгрузка в bitmap по его же гайду в net core не поддерживается

namespace QRCodeApp
{
    [HtmlTargetElement("qrcode")]
    public class QRCodeTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var QrcodeContent = context.AllAttributes["content"].Value.ToString();
            var alt = context.AllAttributes["alt"].Value.ToString();
            var width = Convert.ToInt32(context.AllAttributes["width"].Value.ToString());
            var height = Convert.ToInt32(context.AllAttributes["height"].Value.ToString());
            var margin = 0;

            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin, CharacterSet="UTF-8" }
            };

            var pixelData = qrCodeWriter.Write(QrcodeContent);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {

                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, ImageFormat.Png);

                output.TagName = "img";
                output.Attributes.Clear();
                output.Attributes.Add("width", width);
                output.Attributes.Add("height", height);
                output.Attributes.Add("alt", alt);
                output.Attributes.Add("src",
                   String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
            }
        }
    }
}
