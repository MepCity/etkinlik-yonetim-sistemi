using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Configurations
{
    // Event varlığının veritabanı sütun kısıtlamalarını ve özelliklerini tanımlayan yapılandırma sınıfı
    internal class EventConfigurations : IEntityTypeConfiguration<Event>
    {
        // Event tablosunun alan uzunluklarını, zorunluluk kurallarını ve sütun türlerini belirleyen metot
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Etkinlik adının zorunlu olduğunu ve en fazla 100 karakter olabileceğini tanımlar
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Açıklamanın zorunlu olduğunu ve en fazla 500 karakter olabileceğini tanımlar
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Konumun zorunlu olduğunu ve en fazla 200 karakter olabileceğini tanımlar
            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(200);

            // Fiyat sütununun TEXT türünde saklanacağını belirtir
            builder.Property(e => e.Price)
                .HasColumnType("TEXT");
            // CreatedAt / UpdatedAt set in code (EventService.CreateEventAsync)
        }
    }
}


