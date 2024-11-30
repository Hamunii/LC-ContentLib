namespace ContentLib.API.Model.Item.Tools.Types;

public class FunnyNameDecorator(IGameItem gameItem) : GameItemDecorator(gameItem)
{
    public override string Name { get => "FunnyName" + base.Name; }
}