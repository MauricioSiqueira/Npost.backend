

using Microsoft.EntityFrameworkCore;
using npost.Core.Auth.Model;

namespace npost.Data;

public partial class DataContext : DbContext
{
    private string ToLowerCase(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        return input.ToLower();
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToLowerCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToLowerCase(property.GetColumnName()));

                //Se não for defino a precisão nem a escala dos decimais
                if (property.ClrType == typeof(Decimal) || property.ClrType == typeof(Decimal?))
                {

                    if (property.GetScale() == null) property.SetScale(2);

                    if (property.GetPrecision() == null) property.SetPrecision(11);
                }
            }
        }
        
        // var municipios = DataMunicipio.Data();
        // modelBuilder.Entity<Municipio>().HasData(municipios);
        
    }
}