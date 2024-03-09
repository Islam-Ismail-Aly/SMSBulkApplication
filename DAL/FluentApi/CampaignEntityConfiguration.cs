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
    public class CampaignEntityConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.CampaignName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.SenderName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.ContentMessage).IsRequired().HasMaxLength(300).IsFixedLength();
        }
    }
}
