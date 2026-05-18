namespace VsaTemplate.Common.Interfaces;

// credit: Jason Taylor
public interface IEndpointGroup
{
    static virtual string? RoutePrefix => null;

    static abstract void Map(RouteGroupBuilder groupBuilder);
}
