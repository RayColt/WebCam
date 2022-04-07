using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace ImgProcessing
{
	/// <summary>
	/// Summary description for ImageProcessing.
	/// </summary>
	public unsafe sealed class ImageProcessing
	{
		Bitmap flag, flag2, flag3;
		int width, width2, width3;
		BitmapData bitmapData = null, bitmapData2 = null, bitmapData3 = null;
		Byte* pBase = null, pBase2 = null, pBase3 = null;

		#region ImageProcessing Constructor
		public ImageProcessing(Bitmap picOld, Bitmap picNew, Bitmap target)
		{
			this.flag = picOld;
			this.flag2 = picNew;
			this.flag3 = target;
		}

		public ImageProcessing(Bitmap source, Bitmap target)
		{
			this.flag = source;
			this.flag2 = target;
		}
		#endregion

		#region internal methods
		#region PixelSize
		public Point PixelSize
		{
			get
			{
				GraphicsUnit unit = GraphicsUnit.Pixel;
				RectangleF bounds = flag.GetBounds(ref unit);
				return new Point((int)bounds.Width, (int)bounds.Height);
			}
		}

		public Point PixelSize2
		{
			get
			{
				GraphicsUnit unit = GraphicsUnit.Pixel;
				RectangleF bounds = flag2.GetBounds(ref unit);
				return new Point((int)bounds.Width, (int)bounds.Height);
			}
		}

		public Point PixelSize3
		{
			get
			{
				GraphicsUnit unit = GraphicsUnit.Pixel;
				RectangleF bounds = flag3.GetBounds(ref unit);
				return new Point((int)bounds.Width, (int)bounds.Height);
			}
		}
		#endregion

		#region PixelAt
		public Pixel* PixelAt(int x, int y)
		{
			return (Pixel*)(pBase + y * width + x * sizeof(Pixel));
		}
		public Pixel* PixelAt2(int x, int y)
		{
			return (Pixel*)(pBase2 + y * width2 + x * sizeof(Pixel));
		}
		public Pixel* PixelAt3(int x, int y)
		{
			return (Pixel*)(pBase3 + y * width3 + x * sizeof(Pixel));
		}
		#endregion

		#region LockBitMap
		public void LockBitmap()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = flag.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int)boundsF.X,
				(int)boundsF.Y,
				(int)boundsF.Width,
				(int)boundsF.Height);

			width = (int)boundsF.Width * sizeof(Pixel);
			if (width % 4 != 0)
			{
				width = 4 * (width / 4 + 1);
			}
			bitmapData = flag.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			pBase = (Byte*)bitmapData.Scan0.ToPointer();

		}

		public void UnlockBitmap()
		{
			flag.UnlockBits(bitmapData);
			bitmapData = null;
			pBase = null;
		}

		public void LockBitmap2()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = flag2.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int)boundsF.X,
				(int)boundsF.Y,
				(int)boundsF.Width,
				(int)boundsF.Height);

			width2 = (int)boundsF.Width * sizeof(Pixel);
			if (width2 % 4 != 0)
			{
				width2 = 4 * (width2 / 4 + 1);
			}

			bitmapData2 = flag2.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			pBase2 = (Byte*)bitmapData2.Scan0.ToPointer();
		}

		public void UnlockBitmap2()
		{
			flag2.UnlockBits(bitmapData2);
			bitmapData2 = null;
			pBase2 = null;
		}

		public void LockBitmap3()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = flag3.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int)boundsF.X,
				(int)boundsF.Y,
				(int)boundsF.Width,
				(int)boundsF.Height);

			width3 = (int)boundsF.Width * sizeof(Pixel);
			if (width3 % 4 != 0)
			{
				width3 = 4 * (width3 / 4 + 1);
			}

			bitmapData3 = flag3.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			pBase3 = (Byte*)bitmapData3.Scan0.ToPointer();
		}

		public void UnlockBitmap3()
		{
			flag3.UnlockBits(bitmapData3);
			bitmapData3 = null;
			pBase3 = null;
		}
		#endregion

		#region Save
		public void Save(string filename)
		{
			flag3.Save(filename, ImageFormat.Jpeg);
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
			flag = null;
			flag2 = null;
			flag3 = null;
		}
		#endregion

		public Bitmap bitmap
		{
			get
			{
				return (this.flag3);
			}
		}
		#endregion

		#region Magic code for CompareUnsafeFaster
		public void CompareUnsafeFaster(out Int32 percent)
		{
			Point size = PixelSize;
			percent = 0;
			LockBitmap();
			LockBitmap2();
			LockBitmap3();

			for (int y = 0; y < size.Y; y++)
			{
				Pixel* pPixel = PixelAt(0, y);
				Pixel* pPixel2 = PixelAt2(0, y);
				Pixel* pPixel3 = PixelAt3(0, y);
				for (int x = 0; x < size.X; x++)
				{
					// TODO: go for one color if you like, depends on the light
					if (!(pPixel->red == pPixel2->red) && !(pPixel->green == pPixel2->green) && !(pPixel->blue == pPixel2->blue))
					{
						pPixel3->red = pPixel2->red;
						pPixel3->green = pPixel2->green;
						pPixel3->blue = pPixel2->blue;
						percent++;
					}
					pPixel++;
					pPixel2++;
					pPixel3++;
				}
			}
			UnlockBitmap3();
			UnlockBitmap2();
			UnlockBitmap();
		}
		#endregion

		#region Magic code for ComplementUnsafeFaster
		public void ComplementUnsafeFaster()
		{
			Point size = PixelSize;
			LockBitmap();
			LockBitmap2();
			for (int y = 0; y < size.Y; y++)
			{
				Pixel* pPixel = PixelAt(0, y);
				Pixel* pPixel2 = PixelAt2(0, y);
				for (int x = 0; x < size.X; x++)
				{
					pPixel2->red = (byte)(255 - (int)pPixel->red);
					pPixel2->green = (byte)(255 - (int)pPixel->green);
					pPixel2->blue = (byte)(255 - (int)pPixel->blue);
					pPixel++;
					pPixel2++;
				}
			}
			UnlockBitmap2();
			UnlockBitmap();
			flag3 = new Bitmap(flag2);
		}
		#endregion

		#region Magic code for ColorBallUnsafeFaster
		public void ColorBallUnsafeFaster(double red, double green, double blue)
		{
			Point size = PixelSize;
			double fr, fg, fb;
			if (red < 0)
			{
				fr = red / 100 + 1;
			}
			else
			{
				fr = red / 100;
			}
			if (green < 0)
			{
				fg = green / 100 + 1;
			}
			else
			{
				fg = green / 100;
			}
			if (blue < 0)
			{
				fb = blue / 100 + 1;
			}
			else
			{
				fb = blue / 100;
			}
			LockBitmap();
			LockBitmap2();

			for (int y = 0; y < size.Y; y++)
			{
				Pixel* pPixel = PixelAt(0, y);
				Pixel* pPixel2 = PixelAt2(0, y);

				for (int x = 0; x < size.X; x++)
				{
					if (red < 0)
					{
						pPixel2->red = (byte)((int)pPixel->red * fr);
					}
					else
					{
						pPixel2->red = (byte)((int)pPixel->red + (255 - (int)pPixel->red) * fr);
					}
					if (green < 0)
					{
						pPixel2->green = (byte)((int)pPixel->green * fg);
					}
					else
					{
						pPixel2->green = (byte)((int)pPixel->green + (255 - (int)pPixel->green) * fg);
					}
					if (blue < 0)
					{
						pPixel2->blue = (byte)((int)pPixel->blue * fb);
					}
					else
					{
						pPixel2->blue = (byte)((int)pPixel->blue + (255 - (int)pPixel->blue) * fb);
					}
					pPixel++;
					pPixel2++;
				}
			}
			UnlockBitmap2();
			UnlockBitmap();
			flag3 = new Bitmap(flag2);
		}
		#endregion

		#region Magic code for Brightness
		public void Brightness(double brightness)
		{
			Point size = PixelSize;
			double f;
			if (brightness < 0)
			{
				f = brightness / 100 + 1;
			}
			else
			{
				f = brightness / 100;
			}
			LockBitmap();
			LockBitmap2();
			for (int y = 0; y < size.Y; y++)
			{
				Pixel* pPixel = PixelAt(0, y);
				Pixel* pPixel2 = PixelAt2(0, y);

				for (int x = 0; x < size.X; x++)
				{
					if (brightness < 0)
					{
						pPixel2->red = (byte)((int)pPixel->red * f);
						pPixel2->green = (byte)((int)pPixel->green * f);
						pPixel2->blue = (byte)((int)pPixel->blue * f);
					}
					else
					{
						pPixel2->green = (byte)((int)pPixel->red + (255 - (int)pPixel->red) * f);
						pPixel2->green = (byte)((int)pPixel->green + (255 - (int)pPixel->green) * f);
						pPixel2->blue = (byte)((int)pPixel->blue + (255 - (int)pPixel->blue) * f);
					}
					pPixel++;
					pPixel2++;
				}
			}
			UnlockBitmap2();
			UnlockBitmap();
			flag3 = new Bitmap(flag2);

		}
		#endregion

		#region MakeGreyUnsafeFaster
		public void MakeGreyUnsafeFaster()
		{
			Point size = PixelSize;
			LockBitmap();
			LockBitmap2();
			for (int y = 0; y < size.Y; y++)
			{
				Pixel* pPixel = PixelAt(0, y);
				Pixel* pPixel2 = PixelAt2(0, y);
				for (int x = 0; x < size.X; x++)
				{
					byte value = (byte)((pPixel->red + pPixel->green + pPixel->blue) / 3);
					pPixel2->red = value;
					pPixel2->green = value;
					pPixel2->blue = value;
					pPixel++;
					pPixel2++;
				}
			}
			UnlockBitmap2();
			UnlockBitmap();
			flag3 = new Bitmap(flag2);
		}
		#endregion
	}

	#region Pixel struct
	public struct Pixel
	{
		public byte blue;
		public byte green;
		public byte red;
	}
	#endregion
}