using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace LogoKaresz
{
	public partial class Form1 : Form
	{
		void Odatölt(double f, double d, Color szín)
		{
			using (new Rajzol(false))
			using (new Átmenetileg(Jobbra, f))
			using (new Átmenetileg(Előre, d))
				Tölt(szín);
		}
		void Lopva_oldalaz(double m)
		{
			using (new Rajzol(false))
			using (new Átmenetileg(Jobbra, 90))
				Előre(m);
		}
		void Lopva_előre(double m)
		{
			using (new Rajzol(false))
				Előre(m);
		}
		void EJ(double d, double f) 
		{ Előre(d); Jobbra(f); }

        #region Hermann-rács
        void Fekete_négyzet(double méret)
		{
			Ismétlés(4, delegate () {
				Előre(méret);
				Jobbra(90);
			});
			
			Odatölt(45, méret, Color.Black);
		}
		/// <summary>
		/// Letesz megadott számú fekete négyzetet egymás mellé
		/// </summary>
		/// <param name="db">ennyi négyzetet tesz le egymás mellé</param>
		void Hermann_sor(int db, double d)
		{
			Ismétlés(db, delegate () {
				Fekete_négyzet(d);
				Lopva_oldalaz(d+d / 4);
			});
			Lopva_oldalaz(-db * (d + d / 4));
		}
		/// <summary>
		/// Elkészít egy N sorból és M oszlopból álló fekete négyzetrácsot.
		/// </summary>
		/// <param name="N">sorok száma</param>
		/// <param name="M">oszlopok száma</param>
		/// <param name="méret">méret</param>
		void Hermann_rács(int N, int M, double méret)
		{
			Ismétlés(N, delegate () {
				Hermann_sor(M, méret);
				Lopva_előre(méret + méret / 4);
			});
			Lopva_előre(-N * (méret + méret / 4));
		}
        #endregion

        #region sávos mozaik

        static double sav_hezag = .1;
		static double harany = (1 - 4 * sav_hezag) / 2;

		void Sávos_fehér_négyzet(double m)
		{
			Ismétlés(2, delegate ()
			{
				using (new Rajzol(false))
				{
					using (new Átmenetileg(Előre, sav_hezag * m))
					using (new Átmenetileg(Jobbra, 90))
					using (new Átmenetileg(Előre, sav_hezag * m))
					using (new Átmenetileg(Balra, 90))
					using (new Rajzol(true))
						Félnégyzet(harany*m, Color.Black);
					EJ(m, 90);
					EJ(m, 90);
				}
			});
		}

		void Félnégyzet(double d, Color c)
		{
			using (new Szín(c))
			{
				EJ(d, 135);
				EJ(d * Math.Sqrt(2), 135);
				EJ(d, 90);
				Odatölt(45, d / 3, c);
			}
		}

		void Sávos_fekete_négyzet(double m)
		{
			Fekete_négyzet(m);
			Ismétlés(2, delegate () 
			{
				using (new Átmenetileg(Előre, sav_hezag * m))
				using (new Átmenetileg(Jobbra, 90))
				using (new Átmenetileg(Előre, sav_hezag * m))
				using (new Átmenetileg(Balra, 90))
					Félnégyzet(harany * m, Color.White);
				EJ(m, 90);
				EJ(m, 90);
			});
		}

		void Sávos_sor(int db, double m, bool b)
		{
			Ismétlés(db, delegate () 
			{
				if (b)
					Sávos_fekete_négyzet(m);
				else
					Sávos_fehér_négyzet(m);
				b = !b;
				Lopva_oldalaz(m);
			});
			Lopva_oldalaz(-db*m);
		}

		void Sávos_mozaik(int N, int M, double m)
		{
			bool b = true;
			Ismétlés(N, delegate ()
			{
				Sávos_sor(M, m, b);
				b = !b;
				Lopva_előre(m);
			});
			Lopva_oldalaz(-N * m);
		}
		#endregion 

        #region Kávébab-illúzió

        void Háttér(double x, double y, Color sz)
		{
			using (new Szín(sz))
			{
				Ismétlés(2, delegate ()
				{
					EJ(y, 90);
					EJ(x, 90);
				});
				double k = x < y ? x : y;
				Odatölt(45, k, sz);
			}
		}
		void Félbab(double d, Color szín)
		{
			Tollvastagság(2);
            using (new Szín(szín))
            {
				Balra(45);
				EJ(d, 45);
				EJ(d, 45);
				EJ(d, 135);
            }
		}
		void Kávébab(double f, double d)
		{
			double ennyit = d *(1+Math.Sqrt(2))/2;
            using (new Átmenetileg(Jobbra, f))
            {
				Lopva_előre(-ennyit);			
				Félbab(d, Color.Black);
				Félbab(d, Color.White);
				Odatölt(0, d, Color.Brown);
				Lopva_előre(ennyit);
            }
		}

		/// <summary>
		/// Létrehoz egy N db sorból és M db oszlopból álló kávébab-illúziót
		/// </summary>
		/// <param name="N">sorok száma</param>
		/// <param name="M">oszlopok száma</param>
		/// <param name="m">babok mérete</param>
		void Kávébab_illúzió(int N, int M, double m)
		{
			double hézag = 2.5 * m;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
					Kávébab(-(i + j)*30, m);
					Lopva_oldalaz(hézag);
                }
				Lopva_oldalaz(-M * hézag);
				Lopva_előre(hézag);
            }
			Lopva_oldalaz(-N * hézag);
		}
		#endregion



		void FELADAT()
		{
			Teleport(20, 400, észak);
			//Háttér(700,350,Color.Lime);
			//Lopva_előre(20);
			//Lopva_oldalaz(20);
			//Kávébab_illúzió(10,20,10);
			//Sávos_fekete_négyzet(100);
			//Sávos_fehér_négyzet(100);
			Sávos_mozaik(5,10,50);
		}
	}
}
