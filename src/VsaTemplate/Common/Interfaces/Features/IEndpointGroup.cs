namespace VsaTemplate.Common.Interfaces.Features;

public interface IEndpointGroup
{
    static abstract string Prefix { get; }
    static abstract string[] Tags { get; }

    static abstract void Map(IEndpointRouteBuilder builder);
}
