using ContentLib.API.Util;

namespace ContentLib.API.Model.Item.Scrap;

public class ItemType : Enumeration<string>
{
  
    public static readonly ItemType Flashlight = new ItemType("flashlight", "FlashLight");
    public static readonly ItemType ProFlashlight = new ItemType("pro-flashlight", "Pro Flashlight");

    private ItemType(string name, string displayName) : base(name, displayName){}

    public string GetItemTypeInfo() => $"Display Name {DisplayName}, In-Code Name: {Value}";
}