using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.Localization
{
    public class LanguageProviderDE : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://de.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://de.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://de.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://de.pathofexile.com/");
        public string RarityUnique => "Einzigartig";
        public string RarityRare => "Selten";
        public string RarityMagic => "Magisch";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Währung";
        public string RarityGem => "Gemme";
        public string RarityDivinationCard => "Weissagungskarte";
        public string DescriptionUnidentified => "Nicht identifiziert";
        public string DescriptionQuality => "Qualität: ";
        public string DescriptionCorrupted => "Verderbt";
        public string DescriptionRarity => "Seltenheit: ";
        public string DescriptionSockets => "Fassungen: ";
        public string DescriptionItemLevel => "Gegenstandsstufe: ";
        public string DescriptionExperience => "Erfahrung: ";
        public string PrefixSuperior => "(hochwertig)";
        public string InfluenceShaper => "Schöpfer";
        public string InfluenceElder => "Ältesten";
        public string InfluenceCrusader => "Kreuzritter";
        public string InfluenceHunter => "Jägers";
        public string InfluenceRedeemer => "Erlöserin";
        public string InfluenceWarlord => "Kriegsherrn";
        public string DescriptionMapTier => "Kartenlevel: ";
        public string DescriptionItemQuantity => "Gegenstandsmenge: ";
        public string DescriptionItemRarity => "Gegenstandsseltenheit: ";
        public string DescriptionMonsterPackSize => "Monstergruppengröße: ";
        public string PrefixBlighted => "Befallene";
        public string KeywordProphecy => "Prophezeiung";
        public string KeywordVaal => "Vaal";
    }

    public class LanguageProviderEN : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://www.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://www.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://www.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "Rare";
        public string RarityMagic => "Magic";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Currency";
        public string RarityGem => "Gem";
        public string RarityDivinationCard => "Divination Card";
        public string DescriptionUnidentified => "Unidentified";
        public string DescriptionQuality => "Quality: ";
        public string DescriptionCorrupted => "Corrupted";
        public string DescriptionRarity => "Rarity: ";
        public string DescriptionSockets => "Sockets: ";
        public string DescriptionItemLevel => "Item Level: ";
        public string DescriptionExperience => "Experience: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Shaper";
        public string InfluenceElder => "Elder";
        public string InfluenceCrusader => "Crusader";
        public string InfluenceHunter => "Hunter";
        public string InfluenceRedeemer => "Redeemer";
        public string InfluenceWarlord => "Warlord";
        public string DescriptionMapTier => "Map Tier: ";
        public string DescriptionItemQuantity => "Item Quantity: ";
        public string DescriptionItemRarity => "Item Rarity: ";
        public string DescriptionMonsterPackSize => "Monster Pack Size: ";
        public string PrefixBlighted => "Blighted";
        public string KeywordProphecy => "prophecy";
        public string KeywordVaal => "Vaal";
    }

    public class LanguageProviderES : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://es.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://es.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://es.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://es.pathofexile.com/");
        public string RarityUnique => "Único";
        public string RarityRare => "Raro";
        public string RarityMagic => "Mágico";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Objetos Monetarios";
        public string RarityGem => "Gema";
        public string RarityDivinationCard => "Carta de Adivinación";
        public string DescriptionUnidentified => "Sin identificar";
        public string DescriptionQuality => "Calidad: ";
        public string DescriptionCorrupted => "Corrupto";
        public string DescriptionRarity => "Rareza: ";
        public string DescriptionSockets => "Engarces: ";
        public string DescriptionItemLevel => "Nivel de Objeto: ";
        public string DescriptionExperience => "Experiencia: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Creador";
        public string InfluenceElder => "Antiguo";
        public string InfluenceCrusader => "Cruzado";
        public string InfluenceHunter => "Cazador";
        public string InfluenceRedeemer => "Redentora";
        public string InfluenceWarlord => "Jefe de guerra";
        public string DescriptionMapTier => "Grado del Mapa: ";
        public string DescriptionItemQuantity => "Cantidad de Objetos: ";
        public string DescriptionItemRarity => "Rareza de Objetos: ";
        public string DescriptionMonsterPackSize => "Tamaño de Grupos de Monstruos: ";
        public string PrefixBlighted => "Infestado";
        public string KeywordProphecy => "profecía";
        public string KeywordVaal => "Vaal";
    }

    public class LanguageProviderFR : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://fr.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://fr.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://fr.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://fr.pathofexile.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "Rare";
        public string RarityMagic => "Magique";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Objet monétaire";
        public string RarityGem => "Gemme";
        public string RarityDivinationCard => "Carte divinatoire";
        public string DescriptionUnidentified => "Non identifié";
        public string DescriptionQuality => "Qualité: ";
        public string DescriptionCorrupted => "Corrompu";
        public string DescriptionRarity => "Rareté: ";
        public string DescriptionSockets => "Châsses: ";
        public string DescriptionItemLevel => "Niveau de l'objet: ";
        public string DescriptionExperience => "Expérience: ";
        public string PrefixSuperior => "supérieure";
        public string InfluenceShaper => "Façonneur";
        public string InfluenceElder => "l'Ancien";
        public string InfluenceCrusader => "Croisé";
        public string InfluenceHunter => "Chasseur";
        public string InfluenceRedeemer => "la Rédemptrice";
        public string InfluenceWarlord => "Seigneur de guerre";
        public string DescriptionMapTier => "Palier de Carte: ";
        public string DescriptionItemQuantity => "Quantité d'objets: ";
        public string DescriptionItemRarity => "Rareté des objets: ";
        public string DescriptionMonsterPackSize => "Taille des groupes de monstres: ";
        public string PrefixBlighted => "infestée";
        public string KeywordProphecy => "prophétie";
        public string KeywordVaal => "Vaal";
    }

    public class LanguageProviderKR : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://poe.game.daum.net/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://poe.game.daum.net/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://poe.game.daum.net/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://poe.game.daum.net/");
        public string RarityUnique => "고유";
        public string RarityRare => "희귀";
        public string RarityMagic => "마법";
        public string RarityNormal => "일반";
        public string RarityCurrency => "화폐";
        public string RarityGem => "젬";
        public string RarityDivinationCard => "점술 카드";
        public string DescriptionUnidentified => "미확인";
        public string DescriptionQuality => "퀄리티: ";
        public string DescriptionCorrupted => "타락";
        public string DescriptionRarity => "아이템 희귀도: ";
        public string DescriptionSockets => "홈: ";
        public string DescriptionItemLevel => "아이템 레벨: ";
        public string PrefixSuperior => "상";
        public string InfluenceShaper => "쉐이퍼";
        public string InfluenceElder => "엘더";
        public string InfluenceCrusader => "성전사";
        public string InfluenceHunter => "사냥꾼";
        public string InfluenceRedeemer => "대속자";
        public string InfluenceWarlord => "전쟁군주";
        public string DescriptionMapTier => "지도 등급: ";
        public string DescriptionItemQuantity => "아이템 수량: ";
        public string DescriptionItemRarity => "아이템 희귀도: ";
        public string DescriptionMonsterPackSize => "몬스터 무리 규모: ";
        public string PrefixBlighted => "역병";
        public string DescriptionExperience => "경험치: ";
        public string KeywordProphecy => "예언을";
        public string KeywordVaal => "바알";
    }

    public class LanguageProviderPT : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://br.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://br.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://br.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://br.pathofexile.com/");
        public string RarityUnique => "Único";
        public string RarityRare => "Raro";
        public string RarityMagic => "Mágico";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Moeda";
        public string RarityGem => "Gema";
        public string RarityDivinationCard => "Carta de Adivinhação";
        public string DescriptionUnidentified => "Não Identificado";
        public string DescriptionQuality => "Qualidade: ";
        public string DescriptionCorrupted => "Corrompido";
        public string DescriptionRarity => "Raridade: ";
        public string DescriptionSockets => "Encaixes: ";
        public string DescriptionItemLevel => "Nível do Item: ";
        public string DescriptionExperience => "Experiência: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Criador";
        public string InfluenceElder => "Ancião";
        public string InfluenceCrusader => "Cruzado";
        public string InfluenceHunter => "Caçador";
        public string InfluenceRedeemer => "Redentor";
        public string InfluenceWarlord => "Senhor da Guerra";
        public string DescriptionMapTier => "Tier do Mapa: ";
        public string DescriptionItemQuantity => "Quantidade de Itens: ";
        public string DescriptionItemRarity => "Raridade de Itens: ";
        public string DescriptionMonsterPackSize => "Tamanho do Grupo de Monstros: ";
        public string PrefixBlighted => "Arruinado";
        public string KeywordProphecy => "profecia";
        public string KeywordVaal => "Vaal";
    }

    public class LanguageProviderRU : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://ru.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://ru.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://ru.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://ru.pathofexile.com/");
        public string RarityUnique => "Уникальный";
        public string RarityRare => "Редкий";
        public string RarityMagic => "Волшебный";
        public string RarityNormal => "Обычный";
        public string RarityCurrency => "Валюта";
        public string RarityGem => "Камень";
        public string RarityDivinationCard => "Гадальная карта";
        public string DescriptionUnidentified => "Неопознано";
        public string DescriptionQuality => "Качество: ";
        public string DescriptionCorrupted => "Осквернено";
        public string DescriptionRarity => "Редкость: ";
        public string DescriptionSockets => "Гнезда: ";
        public string DescriptionItemLevel => "Уровень предмета: ";
        public string PrefixSuperior => "Рог";
        public string InfluenceShaper => "Создателя";
        public string InfluenceElder => "Древнего";
        public string InfluenceCrusader => "Крестоносца";
        public string InfluenceHunter => "Охотника";
        public string InfluenceRedeemer => "Избавительницы";
        public string InfluenceWarlord => "Вождя";
        public string DescriptionMapTier => "Уровень карты: ";
        public string DescriptionItemQuantity => "Количество предметов: ";
        public string DescriptionItemRarity => "Редкость предметов: ";
        public string DescriptionMonsterPackSize => "Размер групп монстров: ";
        public string PrefixBlighted => "Заражённая";
        public string DescriptionExperience => "Опыт: ";
        public string KeywordProphecy => "пророчество";
        public string KeywordVaal => "Ваал";
    }

    public class LanguageProviderTH : ILanguageProvider
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://th.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://th.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://th.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://th.pathofexile.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "แรร์";
        public string RarityMagic => "เมจิก";
        public string RarityNormal => "ปกติ";
        public string RarityCurrency => "เคอเรนซี่";
        public string RarityGem => "เจ็ม";
        public string RarityDivinationCard => "ไพ่พยากรณ์";
        public string DescriptionUnidentified => "ยังไม่ได้ตรวจสอบ";
        public string DescriptionQuality => "คุณภาพ: ";
        public string DescriptionCorrupted => "คอร์รัปต์";
        public string DescriptionRarity => "ความหายาก: ";
        public string DescriptionSockets => "ซ็อกเก็ต: ";
        public string DescriptionItemLevel => "เลเวลไอเทม: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "เชปเปอร์";
        public string InfluenceElder => "เอลเดอร์";
        public string InfluenceCrusader => "ครูเซเดอร์";
        public string InfluenceHunter => "ฮันเตอร์";
        public string InfluenceRedeemer => "รีดีมเมอร์";
        public string InfluenceWarlord => "วอร์หลอด";
        public string DescriptionMapTier => "ระดับแผนที่: ";
        public string DescriptionItemQuantity => "จำนวนไอเท็ม: ";
        public string DescriptionItemRarity => "ระดับความหายากของไอเทม: ";
        public string DescriptionMonsterPackSize => "ขนาดบรรจุมอนสเตอร์: ";
        public string PrefixBlighted => "Blighted";
        public string DescriptionExperience => "ประสบการณ์: ";
        public string KeywordProphecy => "(prophecy)";
        public string KeywordVaal => "วาล์";
    }
}
