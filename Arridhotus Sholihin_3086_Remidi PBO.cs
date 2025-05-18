using System;
using System.Collections.Generic;

namespace BankPelitaApp
{
    // Kelas Rekening merepresentasikan satu akun nasabah
    public class Rekening
    {
        public string NomorRekening { get; private set; }
        public string NamaPemilik { get; private set; }
        public decimal Saldo { get; private set; }

        public Rekening(string nomorRekening, string namaPemilik, decimal saldoAwal = 0)
        {
            NomorRekening = nomorRekening;
            NamaPemilik = namaPemilik;
            Saldo = saldoAwal;
        }

        // Method untuk menarik dana dengan validasi saldo
        public bool TarikDana(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah tarik harus lebih besar dari 0.");
                return false;
            }
            if (Saldo < jumlah)
            {
                Console.WriteLine("Saldo tidak mencukupi.");
                return false;
            }
            Saldo -= jumlah;
            return true;
        }

        // Method untuk setor tunai
        public bool SetorTunai(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah setor harus lebih besar dari 0.");
                return false;
            }
            Saldo += jumlah;
            return true;
        }

        // Method transfer ke rekening lain
        public bool Transfer(Rekening tujuan, decimal jumlah)
        {
            if (tujuan == null)
            {
                Console.WriteLine("Rekening tujuan tidak ditemukan.");
                return false;
            }
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah transfer harus lebih besar dari 0.");
                return false;
            }
            if (Saldo < jumlah)
            {
                Console.WriteLine("Saldo tidak mencukupi.");
                return false;
            }
            Saldo -= jumlah;
            tujuan.Saldo += jumlah;
            return true;
        }
    }

    // Kelas BankPelita untuk mengelola daftar rekening
    public class BankPelita
    {
        private List<Rekening> daftarRekening = new List<Rekening>();

        // Tambah rekening baru
        public void TambahRekening(string nomor, string nama, decimal saldoAwal)
        {
            // Cek nomor unik
            if (FindRekening(nomor) != null)
            {
                Console.WriteLine("Nomor rekening sudah terdaftar.");
                return;
            }
            daftarRekening.Add(new Rekening(nomor, nama, saldoAwal));
            Console.WriteLine("Rekening berhasil ditambahkan.");
        }

        // Cari rekening berdasarkan nomor
        public Rekening FindRekening(string nomor)
        {
            return daftarRekening.Find(r => r.NomorRekening == nomor);
        }

        // Tampilkan semua rekening
        public void TampilkanSemuaRekening()
        {
            if (daftarRekening.Count == 0)
            {
                Console.WriteLine("Belum ada data nasabah.");
                return;
            }
            Console.WriteLine("Daftar Nasabah:");
            foreach (var r in daftarRekening)
            {
                Console.WriteLine($"{r.NomorRekening} - {r.NamaPemilik} - Saldo: {r.Saldo:C}");
            }
        }
    }

    // Program utama dengan menu console
    class Program
    {
        static void Main(string[] args)
        {
            BankPelita bank = new BankPelita();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Aplikasi Bank Pelita ===");
                Console.WriteLine("1. Tambah Rekening");
                Console.WriteLine("2. Tampilkan Rekening");
                Console.WriteLine("3. Tarik Dana");
                Console.WriteLine("4. Setor Tunai");
                Console.WriteLine("5. Transfer Antar Rekening");
                Console.WriteLine("6. Keluar");
                Console.Write("Pilih menu: ");

                string pilihan = Console.ReadLine();
                Console.WriteLine();
                switch (pilihan)
                {
                    case "1":
                        Console.Write("Nomor Rekening: ");
                        string nomorBaru = Console.ReadLine();
                        Console.Write("Nama Pemilik: ");
                        string namaBaru = Console.ReadLine();
                        Console.Write("Saldo Awal: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal saldoAwal))
                        {
                            bank.TambahRekening(nomorBaru, namaBaru, saldoAwal);
                        }
                        else
                        {
                            Console.WriteLine("Input saldo tidak valid.");
                        }
                        break;
                    case "2":
                        bank.TampilkanSemuaRekening();
                        break;
                    case "3":
                        Console.Write("Nomor Rekening: ");
                        string nomorTarik = Console.ReadLine();
                        Rekening rekTarik = bank.FindRekening(nomorTarik);
                        if (rekTarik != null)
                        {
                            Console.Write("Jumlah Tarik: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal jumlahTarik))
                            {
                                if (rekTarik.TarikDana(jumlahTarik))
                                    Console.WriteLine("Penarikan berhasil.");
                            }
                            else
                            {
                                Console.WriteLine("Input jumlah tidak valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Rekening tidak ditemukan.");
                        }
                        break;
                    case "4":
                        Console.Write("Nomor Rekening: ");
                        string nomorSetor = Console.ReadLine();
                        Rekening rekSetor = bank.FindRekening(nomorSetor);
                        if (rekSetor != null)
                        {
                            Console.Write("Jumlah Setor: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal jumlahSetor))
                            {
                                if (rekSetor.SetorTunai(jumlahSetor))
                                    Console.WriteLine("Setoran berhasil.");
                            }
                            else
                            {
                                Console.WriteLine("Input jumlah tidak valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Rekening tidak ditemukan.");
                        }
                        break;
                    case "5":
                        Console.Write("Nomor Rekening Asal: ");
                        string nomorAsal = Console.ReadLine();
                        Rekening rekAsal = bank.FindRekening(nomorAsal);
                        Console.Write("Nomor Rekening Tujuan: ");
                        string nomorTujuan = Console.ReadLine();
                        Rekening rekTujuan = bank.FindRekening(nomorTujuan);
                        if (rekAsal != null && rekTujuan != null)
                        {
                            Console.Write("Jumlah Transfer: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal jumlahTransfer))
                            {
                                if (rekAsal.Transfer(rekTujuan, jumlahTransfer))
                                    Console.WriteLine("Transfer berhasil.");
                            }
                            else
                            {
                                Console.WriteLine("Input jumlah tidak valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Salah satu rekening tidak ditemukan.");
                        }
                        break;
                    case "6":
                        running = false;
                        Console.WriteLine("Terima kasih telah menggunakan Aplikasi Bank Pelita.");
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
            }
        }
    }
}
