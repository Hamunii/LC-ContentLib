namespace ContentLib.API.Model.Item.Scrap;

public interface IVanillaScrap : IScrap
{
    ScrapType Type { get; }
}