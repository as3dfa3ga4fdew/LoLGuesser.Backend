using Api.Clients;
using Api.Clients.Interfaces;
using Api.Models.DDragonClasses;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http.Json;

namespace UnitTests
{
    public class DDragonCdnClientTests
    {
        [Fact]
        public async Task TryGetVerionsAsync_WhenApiCallAndDeserializeSucceeds_ShouldReturnTrueAndActionStringList()
        {
            //Arrange
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[\"13.20.1\"]")
            };
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string>? versions = null;
            bool isSuccess = await dDragonCdnClient.TryGetVersionsAsync(result => versions = result);

            //Assert
            Assert.NotNull(versions);
            Assert.True(isSuccess);
            Assert.NotEmpty(versions);
        }

        [Fact]
        public async Task TryGetVersionsAsync_WhenApiCallThrowsAnyException_ShouldReturnFalseAndActionNull()
        {
            //Arrange
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("[\"13.20.1\"]")
            };
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception());
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string>? versions = null;
            bool isSuccess = await dDragonCdnClient.TryGetVersionsAsync(result => versions = result);

            //Assert
            Assert.False(isSuccess);
            Assert.Null(versions);
        }

        [Fact]
        public async Task TryGetVersionsAsync_WhenStringContentCannotDeserialize_ShouldReturnFalseAndActionNull()
        {
            //Arrange
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            };
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string>? versions = null;
            bool isSuccess = await dDragonCdnClient.TryGetVersionsAsync(result => versions = result);

            //Assert
            Assert.False(isSuccess);
            Assert.Null(versions);
        }

        [Fact]
        public async Task TryGetDataAsync_WhenCdnCallAndDeserializeSucceeds_ShouldReturnTrueAndActionRootObject()
        {
            //Arrange
            string version = "13.20.1";
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"type\":\"champion\",\"format\":\"full\",\"version\":\"13.20.1\",\"data\":{\"Aatrox\":{\"id\":\"Aatrox\",\"key\":\"266\",\"name\":\"Aatrox\",\"title\":\"the Darkin Blade\",\"image\":{\"full\":\"Aatrox.png\",\"sprite\":\"champion0.png\",\"group\":\"champion\",\"x\":0,\"y\":0,\"w\":48,\"h\":48},\"skins\":[{\"id\":\"266000\",\"num\":0,\"name\":\"default\",\"chromas\":false},{\"id\":\"266001\",\"num\":1,\"name\":\"Justicar Aatrox\",\"chromas\":false},{\"id\":\"266002\",\"num\":2,\"name\":\"Mecha Aatrox\",\"chromas\":true},{\"id\":\"266003\",\"num\":3,\"name\":\"Sea Hunter Aatrox\",\"chromas\":false},{\"id\":\"266007\",\"num\":7,\"name\":\"Blood Moon Aatrox\",\"chromas\":false},{\"id\":\"266008\",\"num\":8,\"name\":\"Prestige Blood Moon Aatrox\",\"chromas\":false},{\"id\":\"266009\",\"num\":9,\"name\":\"Victorious Aatrox\",\"chromas\":true},{\"id\":\"266011\",\"num\":11,\"name\":\"Odyssey Aatrox\",\"chromas\":true},{\"id\":\"266020\",\"num\":20,\"name\":\"Prestige Blood Moon Aatrox (2022)\",\"chromas\":false},{\"id\":\"266021\",\"num\":21,\"name\":\"Lunar Eclipse Aatrox\",\"chromas\":true},{\"id\":\"266030\",\"num\":30,\"name\":\"DRX Aatrox\",\"chromas\":true},{\"id\":\"266031\",\"num\":31,\"name\":\"Prestige DRX Aatrox\",\"chromas\":false}],\"lore\":\"Once honored defenders of Shurima against the Void, Aatrox and his brethren would eventually become an even greater threat to Runeterra, and were defeated only by cunning mortal sorcery. But after centuries of imprisonment, Aatrox was the first to find freedom once more, corrupting and transforming those foolish enough to try and wield the magical weapon that contained his essence. Now, with stolen flesh, he walks Runeterra in a brutal approximation of his previous form, seeking an apocalyptic and long overdue vengeance.\",\"blurb\":\"Once honored defenders of Shurima against the Void, Aatrox and his brethren would eventually become an even greater threat to Runeterra, and were defeated only by cunning mortal sorcery. But after centuries of imprisonment, Aatrox was the first to find...\",\"allytips\":[\"Use Umbral Dash while casting The Darkin Blade to increase your chances of hitting the enemy.\",\"Crowd Control abilities like Infernal Chains or your allies' immobilizing effects will help you set up The Darkin Blade.\",\"Cast World Ender when you are sure you can force a fight.\"],\"enemytips\":[\"Aatrox's attacks are very telegraphed, so use the time to dodge the hit zones.\",\"Aatrox's Infernal Chains are easier to exit when running towards the sides or at Aatrox.\",\"Keep your distance when Aatrox uses his Ultimate to prevent him from reviving.\"],\"tags\":[\"Fighter\",\"Tank\"],\"partype\":\"Blood Well\",\"info\":{\"attack\":8,\"defense\":4,\"magic\":3,\"difficulty\":4},\"stats\":{\"hp\":650,\"hpperlevel\":114,\"mp\":0,\"mpperlevel\":0,\"movespeed\":345,\"armor\":38,\"armorperlevel\":4.45,\"spellblock\":32,\"spellblockperlevel\":2.05,\"attackrange\":175,\"hpregen\":3,\"hpregenperlevel\":1,\"mpregen\":0,\"mpregenperlevel\":0,\"crit\":0,\"critperlevel\":0,\"attackdamage\":60,\"attackdamageperlevel\":5,\"attackspeedperlevel\":2.5,\"attackspeed\":0.651},\"spells\":[{\"id\":\"AatroxQ\",\"name\":\"The Darkin Blade\",\"description\":\"Aatrox slams his greatsword down, dealing physical damage. He can swing three times, each with a different area of effect.\",\"tooltip\":\"Aatrox slams his greatsword, dealing <physicalDamage>{{ qdamage }} physical damage</physicalDamage>. If they are hit on the edge, they are briefly <status>Knocked Up</status> and they take <physicalDamage>{{ qedgedamage }}</physicalDamage> instead. This Ability can be <recast>Recast</recast> twice, each one changing shape and dealing 25% more damage than the previous one.{{ spellmodifierdescriptionappend }}\",\"leveltip\":{\"label\":[\"Cooldown\",\"Damage\",\"Total AD Ratio\"],\"effect\":[\"{{ cooldown }} -> {{ cooldownNL }}\",\"{{ qbasedamage }} -> {{ qbasedamageNL }}\",\"{{ qtotaladratio*100.000000 }}% -> {{ qtotaladrationl*100.000000 }}%\"]},\"maxrank\":5,\"cooldown\":[14,12,10,8,6],\"cooldownBurn\":\"14/12/10/8/6\",\"cost\":[0,0,0,0,0],\"costBurn\":\"0\",\"datavalues\":{},\"effect\":[null,[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]],\"effectBurn\":[null,\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"],\"vars\":[],\"costType\":\"No Cost\",\"maxammo\":\"-1\",\"range\":[25000,25000,25000,25000,25000],\"rangeBurn\":\"25000\",\"image\":{\"full\":\"AatroxQ.png\",\"sprite\":\"spell0.png\",\"group\":\"spell\",\"x\":384,\"y\":48,\"w\":48,\"h\":48},\"resource\":\"No Cost\"},{\"id\":\"AatroxW\",\"name\":\"Infernal Chains\",\"description\":\"Aatrox smashes the ground, dealing damage to the first enemy hit. Champions and large monsters have to leave the impact area quickly or they will be dragged to the center and take the damage again.\",\"tooltip\":\"Aatrox fires a chain, <status>Slowing</status> the first enemy hit by {{ wslowpercentage*-100 }}% for {{ wslowduration }} seconds and dealing <magicDamage>{{ wdamage }} magic damage</magicDamage>. Champions and large jungle monsters have {{ wslowduration }} seconds to leave the impact area or be <status>Pulled</status> back to the center and damaged again for the same amount.{{ spellmodifierdescriptionappend }}\",\"leveltip\":{\"label\":[\"Cooldown\",\"Damage\",\"Slow\"],\"effect\":[\"{{ cooldown }} -> {{ cooldownNL }}\",\"{{ wbasedamage }} -> {{ wbasedamageNL }}\",\"{{ wslowpercentage*-100.000000 }}% -> {{ wslowpercentagenl*-100.000000 }}%\"]},\"maxrank\":5,\"cooldown\":[20,18,16,14,12],\"cooldownBurn\":\"20/18/16/14/12\",\"cost\":[0,0,0,0,0],\"costBurn\":\"0\",\"datavalues\":{},\"effect\":[null,[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]],\"effectBurn\":[null,\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"],\"vars\":[],\"costType\":\"No Cost\",\"maxammo\":\"-1\",\"range\":[825,825,825,825,825],\"rangeBurn\":\"825\",\"image\":{\"full\":\"AatroxW.png\",\"sprite\":\"spell0.png\",\"group\":\"spell\",\"x\":432,\"y\":48,\"w\":48,\"h\":48},\"resource\":\"No Cost\"},{\"id\":\"AatroxE\",\"name\":\"Umbral Dash\",\"description\":\"Passively, Aatrox heals when damaging enemy champions. On activation, he dashes in a direction.\",\"tooltip\":\"<spellPassive>Passive:</spellPassive> Aatrox gains <lifeSteal>{{ espellvamp }}% Omnivamp</lifeSteal> against champions, increased to <lifeSteal>{{ espellvampempowered }}% Omnivamp</lifeSteal> during <keywordMajor>World Ender</keywordMajor>.<br /><br /><spellActive>Active:</spellActive> Aatrox dashes. He can use this Ability while winding up his other Abilities.{{ spellmodifierdescriptionappend }}\",\"leveltip\":{\"label\":[\"Cooldown\",\"Healing %\",\"Healing % during World Ender\"],\"effect\":[\"{{ cooldown }} -> {{ cooldownNL }}\",\"{{ espellvamp }}% -> {{ espellvampNL }}%\",\"{{ espellvampempowered }}% -> {{ espellvampempoweredNL }}%\"]},\"maxrank\":5,\"cooldown\":[9,8,7,6,5],\"cooldownBurn\":\"9/8/7/6/5\",\"cost\":[0,0,0,0,0],\"costBurn\":\"0\",\"datavalues\":{},\"effect\":[null,[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]],\"effectBurn\":[null,\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"],\"vars\":[],\"costType\":\"No Cost\",\"maxammo\":\"1\",\"range\":[25000,25000,25000,25000,25000],\"rangeBurn\":\"25000\",\"image\":{\"full\":\"AatroxE.png\",\"sprite\":\"spell0.png\",\"group\":\"spell\",\"x\":0,\"y\":96,\"w\":48,\"h\":48},\"resource\":\"No Cost\"},{\"id\":\"AatroxR\",\"name\":\"World Ender\",\"description\":\"Aatrox unleashes his demonic form, fearing nearby enemy minions and gaining attack damage, increased healing, and Move Speed. If he gets a takedown, this effect is extended.\",\"tooltip\":\"Aatrox reveals his true demonic form, <status>Fearing</status> nearby minions for {{ rminionfearduration }} seconds and gaining <speed>{{ rmovementspeedbonus*100 }}% Move Speed</speed> decaying over {{ rduration }} seconds. He also gains <scaleAD>{{ rtotaladamp*100 }}% Attack Damage</scaleAD> and increases <healing>self-healing by {{ rhealingamp*100 }}%</healing> for the duration.<br /><br />Champion takedowns extend the duration of this effect by {{ rextension }} seconds and refresh the <speed>Move Speed</speed> effect.{{ spellmodifierdescriptionappend }}\",\"leveltip\":{\"label\":[\"Total Attack Damage Increase\",\"Healing Increase\",\"Move Speed\",\"Cooldown\"],\"effect\":[\"{{ rtotaladamp*100.000000 }}% -> {{ rtotaladampnl*100.000000 }}%\",\"{{ rhealingamp*100.000000 }}% -> {{ rhealingampnl*100.000000 }}%\",\"{{ rmovementspeedbonus*100.000000 }}% -> {{ rmovementspeedbonusnl*100.000000 }}%\",\"{{ cooldown }} -> {{ cooldownNL }}\"]},\"maxrank\":3,\"cooldown\":[120,100,80],\"cooldownBurn\":\"120/100/80\",\"cost\":[0,0,0],\"costBurn\":\"0\",\"datavalues\":{},\"effect\":[null,[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]],\"effectBurn\":[null,\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"],\"vars\":[],\"costType\":\"No Cost\",\"maxammo\":\"-1\",\"range\":[25000,25000,25000],\"rangeBurn\":\"25000\",\"image\":{\"full\":\"AatroxR.png\",\"sprite\":\"spell0.png\",\"group\":\"spell\",\"x\":48,\"y\":96,\"w\":48,\"h\":48},\"resource\":\"No Cost\"}],\"passive\":{\"name\":\"Deathbringer Stance\",\"description\":\"Periodically, Aatrox's next basic attack deals bonus <physicalDamage>physical damage</physicalDamage> and heals him, based on the target's max health. \",\"image\":{\"full\":\"Aatrox_Passive.png\",\"sprite\":\"passive0.png\",\"group\":\"passive\",\"x\":0,\"y\":0,\"w\":48,\"h\":48}},\"recommended\":[]}}}")
            };
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            Root? root = null;
            bool isSuccess = await dDragonCdnClient.TryGetDataAsync(version, result => root = result);

            //Assert
            Assert.NotNull(root);
            Assert.True(isSuccess);
            Assert.Equal(version, root.Version);
            Assert.NotNull(root.Data);
        }

        [Fact]
        public async Task TryGetDataAsync_WhenCdnCallThrowsAnyException_ShouldReturnFalseAndActionNull()
        {
            //Arrange
            string version = "13.20.1";
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception());
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            Root? root = null;
            bool isSuccess = await dDragonCdnClient.TryGetDataAsync(version, result => root = result);

            //Assert
            Assert.False(isSuccess);
            Assert.Null(root);
        }

        [Fact]
        public async Task TryGetDataAsync_WhenStringContentCannotDeserialize_ShouldReturnFalseAndActionNull()
        {
            //Arrange
            string version = "13.20.1";
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("")
            };
            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            HttpClient httpClient = new HttpClient(httpMessageHandlerMock.Object);

            DDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            Root? root = null;
            bool isSuccess = await dDragonCdnClient.TryGetDataAsync(version, result => root = result);

            //Assert
            Assert.False(isSuccess);
            Assert.Null(root);
        }
    }
}