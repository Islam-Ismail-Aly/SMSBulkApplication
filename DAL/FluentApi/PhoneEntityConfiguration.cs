using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FluentApi
{
    public class PhoneEntityConfiguration : IEntityTypeConfiguration<SMSNumber>
    {
        public void Configure(EntityTypeBuilder<SMSNumber> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(e => e.Campaign)
                   .WithMany(e => e.Phones)
                   .HasForeignKey(e => e.CampaignId);

            builder.Property(s => s.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(12)
                   .IsFixedLength();
        }
    }
}
