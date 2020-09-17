using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Innofactor.EfCoreJsonValueConverter {

  /// <summary>
  /// Extensions for <see cref="PropertyBuilder"/>.
  /// </summary>
  public static class PropertyBuilderExtensions {

    /// <summary>
    /// Serializes field as JSON blob in database.
    /// </summary>
    public static PropertyBuilder<T> HasJsonValueConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class {

      propertyBuilder
        .HasConversion(new JsonValueConverter<T>())
        .Metadata.SetValueComparer(new JsonValueComparer<T>());

      return propertyBuilder;

    }

    /// <summary>
    /// Serializes field as JSON blob in database.
    /// </summary>
    public static PropertyBuilder<T> AddJsonField<T>(this PropertyBuilder<T> propertyBuilder) where T : class {

      propertyBuilder
        .HasConversion(new JsonValueConverter<T>())
        .Metadata.SetValueComparer(new JsonValueComparer<T>());

      var modelType = typeof(T);
      var property = propertyBuilder;

      var converterType = typeof(JsonValueConverter<>).MakeGenericType(modelType);
      var converter = (ValueConverter)Activator.CreateInstance(converterType, new object[] { null });
      property.Metadata.SetValueConverter(converter);

      var valueComparer = typeof(JsonValueComparer<>).MakeGenericType(modelType);
      property.Metadata.SetValueComparer((ValueComparer)Activator.CreateInstance(valueComparer, new object[0]));

      return propertyBuilder;

    }

  }

}
