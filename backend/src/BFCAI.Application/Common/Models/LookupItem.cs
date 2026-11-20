namespace BFCAI.Application.Common.Models;

/// <summary>Item of a dropdown list: value + text.</summary>
public record LookupItem<TId>(TId Id, string Name);
