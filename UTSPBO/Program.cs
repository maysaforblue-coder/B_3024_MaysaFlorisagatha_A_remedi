using System;
using System.Collections.Generic;

namespace utspbo
{
    class Sekolah
    {
        public string idSekolah;
        public string namaSekolah;
        public int jumlahPaketDiterima;

        public Sekolah(string idsekolah, string namasekolah, int jumlahpaketditerima)
        {
            idSekolah = idsekolah;
            namaSekolah = namasekolah;
            jumlahPaketDiterima = jumlahpaketditerima;
        }

        public void TampilkanInfo()
        {
            Console.WriteLine($"ID SEKOLAH : {idSekolah}, NAMA SEKOLAH : {namaSekolah}, JUMLAH PAKET DITERIMA : {jumlahPaketDiterima}.");
        }
    }

    abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
            private set 
            {
                if (value >= 0)
                    stokDapur = value;
                else
                    stokDapur = 0; 
            }
        }

        public PaketMBG(string idpaket, string namapaket, int stokdapur)
        {
            IdPaket = idpaket;
            NamaPaket = namapaket;
            StokDapur = stokdapur;
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokDapur)
            {
                stokDapur -= jumlah;
                return true;
            }
            return false;
        }

        public abstract double HitungBiaya(int jumlah);
    }

    class PaketReguler : PaketMBG
    {
        public double harga = 15000;

        public PaketReguler(string idpaket, string namapaket, int stokdapur)
            : base(idpaket, namapaket, stokdapur)
        {
        }

        public PaketReguler(string idPaket, string namaPaket, int stokDapur, double harga)
            : base(idPaket, namaPaket, stokDapur)
        {
            this.harga = harga;
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }

        public void isi()
        {
            Console.WriteLine($"nasi, ayam, sayur, dan buah");
        }
    }

    class PaketKhusus : PaketMBG
    {
        public double harga = 20000;
        public string jenisDiet;

        public PaketKhusus(string idpaket, string namapaket, int stokdapur, string jenisDiet)
            : base(idpaket, namapaket, stokdapur)
        {
            this.jenisDiet = jenisDiet;
        }

        public PaketKhusus(string idPaket, string namaPaket, int stokDapur, double harga, string jenisDiet)
            : base(idPaket, namaPaket, stokDapur)
        {
            this.harga = harga;
            this.jenisDiet = jenisDiet;
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    class DistribusiMBG : DapatDidistribusikan
    {
        public Sekolah sekolahTujuan;
        public PaketMBG paketMakanan;
        public int jumlah;

        public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paketMakanan, int jumlah)
        {
            this.sekolahTujuan = sekolahTujuan;
            this.paketMakanan = paketMakanan;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Distribusi MBG gagal! Jumlah paket yang dikirim harus lebih dari 0.");
                return;
            }

            if (jumlah > paketMakanan.StokDapur)
            {
                Console.WriteLine($"Distribusi MBG gagal! Stok dapur {paketMakanan.NamaPaket} tidak mencukupi (Stok tersedia: {paketMakanan.StokDapur}).");
                return;
            }

            if (paketMakanan.KurangiStok(jumlah))
            {
                sekolahTujuan.jumlahPaketDiterima += jumlah;
                Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
            }
        }
    }

    class PengelolaMBG
    {
       
        private List<Sekolah> daftarSekolah = new List<Sekolah>();
        private List<PaketMBG> daftarPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah sekolah)
        {
            daftarSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            daftarPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("\n--- DATA SEKOLAH PENERIMA MBG ---");
            foreach (var s in daftarSekolah)
            {
                s.TampilkanInfo();
            }

            Console.WriteLine("\n--- DATA PERSEDIAAN PAKET DAPUR ---");
            foreach (var p in daftarPaket)
            {
                Console.WriteLine($"ID PAKET : {p.IdPaket}, NAMA PAKET : {p.NamaPaket}, SISA STOK : {p.StokDapur}");
            }
        }
    }

    class program
    {
        static void Main(string[] args)
        {
            Sekolah sekolah1 = new Sekolah("1", "SD 1 JEMBER", 10);
            Sekolah sekolah2 = new Sekolah("2", "SMP 2 JEMBER", 15);

            PaketMBG reguler = new PaketReguler("PKT-REG", "paket nasi ayam, sayur, buah", 100);
            PaketMBG khusus = new PaketKhusus("PKT-KHS", "Paket Bebas Gluten", 50, "Bebas Gluten");

            Console.WriteLine("=== DATA AWAL SEKOLAH ===");
            sekolah1.TampilkanInfo();
            sekolah2.TampilkanInfo();

            Console.WriteLine("\n=== PERHITUNGAN BIAYA ===");
            Console.WriteLine($"Biaya 20 Paket Reguler: Rp {reguler.HitungBiaya(20)}");
            Console.WriteLine($"Biaya 10 Paket Khusus : Rp {khusus.HitungBiaya(10)}");

            Console.WriteLine("\n=== PROSES DISTRIBUSI MBG ===");
            DistribusiMBG dist1 = new DistribusiMBG(sekolah1, reguler, 30); 
            dist1.KirimPaket();

            DistribusiMBG dist2 = new DistribusiMBG(sekolah2, khusus, 70);
            dist2.KirimPaket();

            DistribusiMBG dist3 = new DistribusiMBG(sekolah2, khusus, 25); 
            dist3.KirimPaket();

            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(sekolah1);
            pengelola.TambahSekolah(sekolah2);
            pengelola.TambahPaket(reguler);
            pengelola.TambahPaket(khusus);
            pengelola.TampilkanDataMBG();
        }
    }
}