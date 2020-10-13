using UnityEngine;
using System;
using System.Linq;

public class SpriteManager :
    MonoBehaviour,
    IGameManager
{
    public EquipmentSprite[] EquipmentSprites = Enum.GetValues(typeof(EquipmentType))
        .OfType<EquipmentType>()
        .Select(x=>new EquipmentSprite { Type = x })
        .ToArray();
    public CharacterStatSprite[] CharacterStatSprites = new CharacterStats()
        .Select(x=>new CharacterStatSprite{ Name = x.Key })
        .ToArray();
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Status = ManagerStatus.Started;
        Debug.Log("Sprite manager is started.");
    }
    public Sprite GetEquipmentSprite(EquipmentType equipmentType)
    {
        return EquipmentSprites.FirstOrDefault(x => x.Type == equipmentType).Sprite;
    }
    public Sprite GetCharacterStatSprite(string characterStatName)
    {
        return CharacterStatSprites.FirstOrDefault(x => x.Name == characterStatName).Sprite;
    }

}

[Serializable]
public class EquipmentSprite
{
    public EquipmentType Type;
    public Sprite Sprite;
}

[Serializable]
public class CharacterStatSprite
{
    public string Name;
    public Sprite Sprite;
}