using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ozo.Models
{
    public partial class PI01Context : DbContext
    {
        public PI01Context()
        {
        }

        public PI01Context(DbContextOptions<PI01Context> options)
            : base(options)
        {
        }

        public virtual DbSet<KategorijaPosla> KategorijaPosla { get; set; }
        public virtual DbSet<LokacijaOpreme> LokacijaOpreme { get; set; }
        public virtual DbSet<LokacijaPosla> LokacijaPosla { get; set; }
        public virtual DbSet<Najam> Najam { get; set; }
        public virtual DbSet<Natjecaj> Natjecaj { get; set; }
       // public virtual DbSet<NatjecajFirma> NatjecajFirma { get; set; }
        public virtual DbSet<Oprema> Oprema { get; set; }
        public virtual DbSet<OpremaStavka> OpremaStavka { get; set; }
        public virtual DbSet<Osoba> Osoba { get; set; }
        public virtual DbSet<OsobaCertifikat> OsobaCertifikat { get; set; }
        public virtual DbSet<Posao> Posao { get; set; }
        public virtual DbSet<PosaoOprema> PosaoOprema { get; set; }
        public virtual DbSet<PosaoRadnik> PosaoRadnik { get; set; }
        public virtual DbSet<Radnik> Radnik { get; set; }
        public virtual DbSet<ReferentniTipOpreme> ReferentniTipOpreme { get; set; }
        public virtual DbSet<Registar> Registar { get; set; }
        public virtual DbSet<Servis> Servis { get; set; }
        public virtual DbSet<Skaldiste> Skaldiste { get; set; }
        public virtual DbSet<TipRegistra> TipRegistra { get; set; }
        public virtual DbSet<Usluga> Usluga { get; set; }
        public virtual DbSet<UslugaLjudi> UslugaLjudi { get; set; }
        public virtual DbSet<UslugaOprema> UslugaOprema { get; set; }
        public virtual DbSet<Zanimanje> Zanimanje { get; set; }
        //public virtual DbSet<ViewPosao> Vw_PO { get; set; }
        public virtual DbSet<ViewUsluga> Vw_Usluga { get; set; }
        public virtual DbSet<ViewOprema> Vw_Oprema { get; set; }
        public virtual DbSet<ViewOsoba> Vw_Osoba { get; set; }
        public virtual DbSet<ViewNajam> Vw_Najam { get; set; }
        public virtual DbSet<ViewPosao> Vw_Posao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=rppp.fer.hr,3000;Database=PI-01;User Id=pi01; Password=ladara.01;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ViewNajam>(entity => {

                entity.HasKey(e => e.NajamId);

            });

            modelBuilder.Entity<ViewPosao>(entity => {

                entity.HasKey(e => e.PosaoId);

            });

            modelBuilder.Entity<ViewOprema>(entity => {

                entity.HasKey(e => e.OpremaId);

            });

            modelBuilder.Entity<ViewOsoba>(entity => {

                entity.HasKey(e => e.OsobaId);


            });

            //modelBuilder.Entity<ViewPosao>(entity => {

            //    entity.HasKey(e => e.PosaoId);

            //});

            modelBuilder.Entity<ViewUsluga>(entity => {

                entity.HasKey(e => e.UslugaId);

            });



            modelBuilder.Entity<KategorijaPosla>(entity =>
            {
                entity.Property(e => e.KategorijaPoslaId)
                    .HasColumnName("kategorijaPoslaID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(200);
            });
        

                modelBuilder.Entity<LokacijaOpreme>(entity =>
                {
                    entity.Property(e => e.LokacijaOpremeId)
                        .HasColumnName("lokacijaOpremeId")
                        .ValueGeneratedNever();

                    entity.Property(e => e.NazivLokacije)
                        .HasColumnName("nazivLokacije")
                        .HasMaxLength(50);

                    entity.Property(e => e.SkladisteId).HasColumnName("skladisteId");

                    entity.HasOne(d => d.Skladiste)
                        .WithMany(p => p.LokacijaOpreme)
                        .HasForeignKey(d => d.SkladisteId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_LokacijaOpreme_Skaldiste");
                });



                modelBuilder.Entity<LokacijaOpreme>(entity =>
            {
                entity.Property(e => e.LokacijaOpremeId)
                    .HasColumnName("lokacijaOpremeId")
                    .ValueGeneratedNever();

                entity.Property(e => e.NazivLokacije)
                    .HasColumnName("nazivLokacije")
                    .HasMaxLength(50);

                entity.Property(e => e.SkladisteId).HasColumnName("skladisteId");

                entity.HasOne(d => d.Skladiste)
                    .WithMany(p => p.LokacijaOpreme)
                    .HasForeignKey(d => d.SkladisteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LokacijaOpreme_Skaldiste");
            });

            modelBuilder.Entity<LokacijaPosla>(entity =>
            {
                entity.Property(e => e.LokacijaPoslaId)
                    .HasColumnName("lokacijaPoslaId")
                    .ValueGeneratedNever();

                entity.Property(e => e.GradId).HasColumnName("gradId");

                entity.Property(e => e.NazivLokacije)
                    .IsRequired()
                    .HasColumnName("nazivLokacije")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Grad)
                    .WithMany(p => p.LokacijaPosla)
                    .HasForeignKey(d => d.GradId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LokacijaPosla_Registar");
            });

            modelBuilder.Entity<Najam>(entity =>
            {
                entity.Property(e => e.NajamId).HasColumnName("najamId");

                entity.Property(e => e.DatumDo)
                    .HasColumnName("datumDo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DatumOd)
                    .HasColumnName("datumOd")
                    .HasColumnType("datetime");

                entity.Property(e => e.FimraId).HasColumnName("fimraId");

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(200);

                entity.Property(e => e.VrstaNajmaId).HasColumnName("vrstaNajmaId");

                entity.HasOne(d => d.Firma)
                    .WithMany(p => p.NajamFimra)
                    .HasForeignKey(d => d.FimraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Najam_Registar");

                entity.HasOne(d => d.VrstaNajma)
                    .WithMany(p => p.NajamVrstaNajma)
                    .HasForeignKey(d => d.VrstaNajmaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Najam_Registar1");
            });

            modelBuilder.Entity<Natjecaj>(entity =>
            {
               
                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);
                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasMaxLength(200);
                entity.Property(e => e.PobiednikId).HasColumnName("pobiednikId");
                entity.Property(e => e.RaspisateljId).HasColumnName("raspisateljId");
                entity.Property(e => e.Vrijednost).HasColumnName("vrijednost");
                entity.Property(e => e.VrijemeDo)
                    .HasColumnName("vrijemeDo")
                    .HasColumnType("datetime");
                entity.Property(e => e.VrijemeOd)
                    .HasColumnName("vrijemeOd")
                    .HasColumnType("datetime");
                entity.Property(e => e.VrstaNatjecajaId).HasColumnName("vrstaNatjecajaId");
                entity.HasOne(d => d.Pobiednik)
                    .WithMany(p => p.NatjecajPobiednik)
                    .HasForeignKey(d => d.PobiednikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natjecaj_Registar");
                entity.HasOne(d => d.Raspisatelj)
                    .WithMany(p => p.NatjecajRaspisatelj)
                    .HasForeignKey(d => d.RaspisateljId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natjecaj_Registar1");
                entity.HasOne(d => d.VrstaNatjecaja)
                    .WithMany(p => p.NatjecajVrstaNatjecaja)
                    .HasForeignKey(d => d.VrstaNatjecajaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natjecaj_Registar2");
            });

            modelBuilder.Entity<Oprema>(entity =>
            {
                

                entity.Property(e => e.InventarniBroj).HasColumnName("inventarniBroj");

                entity.Property(e => e.KnjigovostvenaVrijednost).HasColumnName("knjigovostvenaVrijednost");
                entity.Property(e => e.NabavnaCijena).HasColumnName("nabavnaCijena");

                entity.Property(e => e.LokacijaOpremeId).HasColumnName("lokacijaOpremeId");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);



                entity.Property(e => e.ReferentniTipOpremeId).HasColumnName("referentniTipOpremeId");

                entity.Property(e => e.StatusId).HasColumnName("statusId");
               // entity.Property(e => e.NabavnaCijena).HasColumnName("nabavnaCijena");

                entity.HasOne(d => d.LokacijaOpreme)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.LokacijaOpremeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_LokacijaOpreme");

                entity.HasOne(d => d.ReferentniTipOpreme)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.ReferentniTipOpremeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_ReferentniTipOpreme");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Registar");
            });

            modelBuilder.Entity<OpremaStavka>(entity =>
            {
                entity.Property(e => e.OpremaStavkaId).HasColumnName("opremaStavkaID");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Kolicina).HasColumnName("kolicina");

                entity.Property(e => e.NajamId).HasColumnName("najamId");

                entity.Property(e => e.OpremaId).HasColumnName("opremaId");

                entity.HasOne(d => d.Najam)
                    .WithMany(p => p.OpremaStavka)
                    .HasForeignKey(d => d.NajamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OpremaStavka_Najam");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.OpremaStavka)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OpremaStavka_Oprema");
            });

            modelBuilder.Entity<Osoba>(entity =>
            {
              

                entity.Property(e => e.GodRodjenja)
                    .HasColumnName("god_rodjenja")
                    .HasColumnType("date");

                entity.Property(e => e.Ime)
                    .HasColumnName("ime")
                    .HasMaxLength(50);

                entity.Property(e => e.Prezime)
                    .HasColumnName("prezime")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OsobaCertifikat>(entity =>
            {
               

              

                entity.Property(e => e.CertifikatId).HasColumnName("certifikatId");

                entity.Property(e => e.OsobaId).HasColumnName("osobaId");

                entity.HasOne(d => d.Certifikat)
                    .WithMany(p => p.OsobaCertifikat)
                    .HasForeignKey(d => d.CertifikatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OsobaCertifikat_Registar");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.OsobaCertifikat)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OsobaCertifikat_Osoba");
            });

            modelBuilder.Entity<Posao>(entity =>
            {
                

                

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.DodatniTrosak).HasColumnName("dodatniTrosak");

                entity.Property(e => e.LokacijaPoslaId).HasColumnName("lokacijaPoslaId");

                entity.Property(e => e.NatjecajId).HasColumnName("natjecajId");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasMaxLength(50);

                entity.Property(e => e.UslugaId).HasColumnName("uslugaId");
               

                entity.Property(e => e.VrijemeDo)
                    .HasColumnName("vrijemeDo")
                    .HasColumnType("datetime");

                entity.Property(e => e.VrijemeOd)
                    .HasColumnName("vrijemeOd")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.LokacijaPosla)
                    .WithMany(p => p.Posao)
                    .HasForeignKey(d => d.LokacijaPoslaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posao_LokacijaPosla");

               entity.HasOne(d => d.Natjecaj)
                    .WithMany(p => p.Posao)
                    .HasForeignKey(d => d.NatjecajId)
                    .HasConstraintName("FK_Posao_Natjecaj");
                    
                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.Posao)
                    .HasForeignKey(d => d.UslugaId)
                    .HasConstraintName("FK_Posao_Usluga");
            });

            modelBuilder.Entity<PosaoOprema>(entity =>
            {
                

                entity.Property(e => e.OpremaId).HasColumnName("opremaId");

                entity.Property(e => e.PosaoId).HasColumnName("posaoId");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PosaoOprema_Oprema");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PosaoOprema_Posao");
            });

            modelBuilder.Entity<PosaoRadnik>(entity =>
            {
                entity.HasKey(e => e.RadnikPosaoId);

                

                entity.Property(e => e.PosaoId).HasColumnName("posaoId");

                entity.Property(e => e.RadnikId).HasColumnName("radnikId");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.PosaoRadnik)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PosaoRadnik_Posao");

                entity.HasOne(d => d.Radnik)
                    .WithMany(p => p.PosaoRadnik)
                    .HasForeignKey(d => d.RadnikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PosaoRadnik_Radnik");
            });

            modelBuilder.Entity<Radnik>(entity =>
            {
               

              
                    

                entity.Property(e => e.KategorijaId).HasColumnName("kategorijaId");

                entity.Property(e => e.OsobaId).HasColumnName("osobaId");

                entity.HasOne(d => d.Kategorija)
                    .WithMany(p => p.Radnik)
                    .HasForeignKey(d => d.KategorijaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Radnik_Kategorija");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.Radnik)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Radnik_Osoba");
            });

            modelBuilder.Entity<ReferentniTipOpreme>(entity =>
            {
                entity.Property(e => e.ReferentniTipOpremeId)
                    .HasColumnName("referentniTipOpremeId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(200);

                entity.Property(e => e.TipOpremeId).HasColumnName("tipOpremeId");

                entity.HasOne(d => d.TipOpreme)
                    .WithMany(p => p.ReferentniTipOpreme)
                    .HasForeignKey(d => d.TipOpremeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReferentniTipOpreme_Registar");
            });

            modelBuilder.Entity<Registar>(entity =>
            {
                entity.Property(e => e.RegistarId).HasColumnName("registarId");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);

                entity.Property(e => e.TipRegistraId).HasColumnName("tipRegistraId");

                entity.HasOne(d => d.TipRegistra)
                    .WithMany(p => p.Registar)
                    .HasForeignKey(d => d.TipRegistraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registar_TipRegistra");
            });

            modelBuilder.Entity<Servis>(entity =>
            {
               

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasMaxLength(50);

                entity.Property(e => e.OpremaId).HasColumnName("opremaId");

                entity.Property(e => e.OsobaId).HasColumnName("osobaId");

                entity.Property(e => e.ServiserId).HasColumnName("serviserId");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.Servis)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servis_Oprema");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.Servis)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servis_Osoba");

                entity.HasOne(d => d.Serviser)
                    .WithMany(p => p.Servis)
                    .HasForeignKey(d => d.ServiserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servis_Registar");
            });

            modelBuilder.Entity<Skaldiste>(entity =>
            {
                entity.HasKey(e => e.SkladisteId);

                entity.Property(e => e.SkladisteId)
                    .HasColumnName("skladisteId")
                    .ValueGeneratedNever();

                entity.Property(e => e.GradId).HasColumnName("gradId");

                entity.Property(e => e.NazivSkladista)
                    .IsRequired()
                    .HasColumnName("nazivSkladista")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Grad)
                    .WithMany(p => p.Skaldiste)
                    .HasForeignKey(d => d.GradId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Skaldiste_Registar");
            });

            modelBuilder.Entity<TipRegistra>(entity =>
            {
                entity.Property(e => e.TipRegistraId).HasColumnName("tipRegistraId");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usluga>(entity =>
            {
                //entity.Property(e => e.UslugaId).HasColumnName("uslugaId");

                entity.Property(e => e.KategorijaPoslaId).HasColumnName("kategorijaPoslaId");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(50);

                entity.HasOne(d => d.KategorijaPosla)
                    .WithMany(p => p.Usluga)
                    .HasForeignKey(d => d.KategorijaPoslaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usluga_KategorijaPosla");
            });

            modelBuilder.Entity<UslugaLjudi>(entity =>
            {
               

             
            

                entity.Property(e => e.ZanimanjeId).HasColumnName("zanimanjeId");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.UslugaLjudi)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UslugaLjudi_Usluga");

                entity.HasOne(d => d.Zanimanje)
                    .WithMany(p => p.UslugaLjudi)
                    .HasForeignKey(d => d.ZanimanjeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UslugaLjudi_Kategorija");
            });

            modelBuilder.Entity<UslugaOprema>(entity =>
            {
               

               

                entity.Property(e => e.UslugaId).HasColumnName("uslugaId");

                entity.HasOne(d => d.ReferentniTipOpreme)
                    .WithMany(p => p.UslugaOprema)
                    .HasForeignKey(d => d.ReferentniTipOpremeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UslugaOprema_ReferentniTipOpreme");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.UslugaOprema)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UslugaOprema_Usluga");
            });

            modelBuilder.Entity<Zanimanje>(entity =>
            {
                entity.Property(e => e.ZanimanjeId)
                    .HasColumnName("zanimanjeId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("naziv")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(50);

                entity.Property(e => e.Placa).HasColumnName("placa");
            });
        }
    }
}
