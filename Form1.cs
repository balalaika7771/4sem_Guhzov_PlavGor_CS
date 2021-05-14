using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRAF_GORIZONT
{
    public partial class Form1 : Form
    {
		
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            timer1.Interval = 10;
            timer1.Start();
        }
		objectz plos = new objectz("C:\\Project\\a3.bmp");
		double teta = 0, phi = 0, zer = 0;
		int RRRX = 0, RRRY = 0, RRRZ = 0;
		double zoom = 1; int shag = 2;
		
		private void timer1_Tick(object sender, EventArgs e)
        {
			Graphics g = Graphics.FromImage(pictureBox1.Image);
			SolidBrush b = new SolidBrush(Color.White);
			g.FillRectangle(b, 0, 0, pictureBox1.Width, pictureBox1.Height);//каждую итерацию эта команда закрашивает окно полностью
			Pen p = new Pen(Color.Red);//создание кисти
			plos.setpov(phi, teta, zer, RRRX, RRRY, RRRZ);
			int[] line = new int[Convert.ToInt32(plos.size * plos.MNOSHITEL)];
			for (int i = 0; i < Convert.ToInt32(plos.size * plos.MNOSHITEL); i++)
			{
				line[i] = 99999999;
			}
			bool f = true;

			Point setbr = new Point();
			for (int y = Convert.ToInt32(plos.size * plos.MNOSHITEL) - 1; y >= 0; y -= shag)
			{
				setbr.X = 0;
				setbr.Y = Convert.ToInt32(y * zoom);
				for (int x = 0; x < Convert.ToInt32(plos.size * plos.MNOSHITEL); x++)
				{

                    if (line[x] > y - plos.prod[x][y].ZI)
					{

						if (!f)
						{
							setbr.X = Convert.ToInt32(x * zoom);
							setbr.Y = Convert.ToInt32(y * zoom - plos.prod[x][y].ZI * zoom);
						}

						g.DrawLine(p, setbr.X, setbr.Y, Convert.ToInt32(x * zoom), Convert.ToInt32(y * zoom - plos.prod[x][y].ZI * zoom));
						setbr.X = x * Convert.ToInt32(zoom);
						setbr.Y = Convert.ToInt32(y *zoom - plos.prod[x][y].ZI * zoom);


						f = true;
						line[x] = y - plos.prod[x][y].ZI;
					}
					else
					{
						f = false;
					}
					
                   

				}


			}
			pictureBox1.Invalidate();
		}

		void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (char)Keys.Up)//тут просто реакция на нажатие клавиш
			{
				teta += (Math.PI / (360));
			}
			if (e.KeyValue == (char)Keys.Down)
			{
				teta -= (Math.PI / (360));
			}

			if (e.KeyValue == (char)Keys.Left)
			{
				phi -= (Math.PI / (360));
			}
			if (e.KeyValue == (char)Keys.Right)
			{
				phi += (Math.PI / (360));
			}

			if (e.KeyValue == (char)Keys.M)
			{
				zer += (Math.PI / (360));
			}
			if (e.KeyValue == (char)Keys.N)
			{
				zer -= (Math.PI / (360));
			}


			if (e.KeyValue == (char)Keys.W)
			{
				RRRX += 10;
			}
			if (e.KeyValue == (char)Keys.S)
			{
				RRRX -= 10;
			}

			if (e.KeyValue == (char)Keys.D)
			{
				RRRY += 10;
			}
			if (e.KeyValue == (char)Keys.A)
			{
				RRRY -= 10;
			}

			if (e.KeyValue == (char)Keys.Q)
			{
				RRRZ -= 10;
			}
			if (e.KeyValue == (char)Keys.E)
			{
				RRRZ += 10;
			}


			if (e.KeyValue == (char)Keys.Z)
			{
				zoom += 1;
			}
			if (e.KeyValue == (char)Keys.X)
			{
				zoom -= 1;
			}


			if (e.KeyValue == (char)Keys.R)
			{
				shag += 1;
			}
			if (e.KeyValue == (char)Keys.F && shag != 1)
			{
				shag -= 1;
			}
		}

	}
	public struct boolint
	{
		public int ZI;
		public bool FLAG;
	}


	class objectz{
	public float[][] mas ;
	public int size;
	public boolint[][] prod;
	public boolint[][] prodsh;
	public double MNOSHITEL = 2;


		public objectz(string nameBMP)
        {
			Bitmap srcImage = new Bitmap(nameBMP);//"твой путь до файла");
		
			if (srcImage == null)
				throw new ArgumentNullException("srcImage");

			mas  = new float[srcImage.Height][];
			for (var y = 0; y < srcImage.Height; y++)
			{
				mas[y] = new float[srcImage.Width];
				for (var x = 0; x < srcImage.Width; x++)
				{
					Color srcPixel = srcImage.GetPixel(x, y);
					mas[y][x] = srcPixel.GetBrightness()*100 - 150;
				}
			}
			size = srcImage.Height;

			boolint br;
			br.FLAG = false;
			br.ZI = 0;
			prod = new boolint[Convert.ToInt32(size * MNOSHITEL)][];
			for(int i = 0; i < Convert.ToInt32(size * MNOSHITEL); i++)
            {
				prod[i] = new boolint[Convert.ToInt32(size * MNOSHITEL)];
				for (int j  = 0; j < Convert.ToInt32(size * MNOSHITEL); j++)
				{
					prod[i][j] = br;
				}
			}

			prodsh = new boolint[Convert.ToInt32(size * MNOSHITEL)][];
			for (int i = 0; i < Convert.ToInt32(size * MNOSHITEL); i++)
			{
				prodsh[i] = new boolint[Convert.ToInt32(size * MNOSHITEL)];
				for (int j = 0; j < Convert.ToInt32(size * MNOSHITEL); j++)
				{
					prodsh[i][j] = br;
				}
			}

		}


		public void setpov(double px, double py, double pz, int rx, int ry, int rz)
		{

			for (int x = 0; x < size*MNOSHITEL; x++)
			{
				for (int y = 0; y < size*MNOSHITEL; y++)
				{
					prod[x][y] = prodsh[x][y];
				}
			}
					for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{

					double KX1, KY1, KZ1;
					KX1 = x - (size / 2);
					KY1 = (y - (size / 2)) * Math.Cos(px) + (mas[x][y] - (size / 2)) * Math.Sin(px);
					KZ1 = -(y - (size / 2)) * Math.Sin(px) + (mas[x][y] - (size / 2)) * Math.Cos(px);

					double KX2, KY2, KZ2;
					KX2 = KX1 * Math.Cos(py) - KZ1 * Math.Sin(py);
					KY2 = KY1;
					KZ2 = KX1 * Math.Sin(py) + KZ1 * Math.Cos(py);

					double KX3, KY3, KZ3;
					KX3 = KX2 * Math.Cos(pz) + KY2 * Math.Sin(pz);
					KY3 = -KX2 * Math.Sin(pz) + KY2 * Math.Cos(pz);
					KZ3 = KZ2;

					boolint vr;
					vr.ZI = Convert.ToInt32(KZ3 + rz + ((size * MNOSHITEL) / 2));
					vr.FLAG = true;
                    prod[Convert.ToInt32(KX3 + rx + ((size * MNOSHITEL) / 2))][Convert.ToInt32(KY3 + ry + ((size * MNOSHITEL) / 2))] = vr;

				}
			}
			
			
			
		}

};

}
