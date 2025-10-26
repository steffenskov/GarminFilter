using GarminFilter.Client.Entities;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.App.Entities;

public class GarminAppTests
{
	[Fact]
	public void GarminApp_ValidJson_CanDeserialize()
	{
		// Arrange
		var json = """
		           {"id":"d51c04af-05cd-40a3-8bcf-e45c461caa37","developerId":"96cd10dd-3a10-41a3-8a6e-07e2bed45cdb","countryLimits":[],"typeId":"1","supportEmailAddress":"hidden@notrelevant.com","appLocalizations":[{"locale":"en","name":"Some watchface","description":"watch face","whatsNew":"v1.0.0 - first version"}],"status":"APPROVED","additionalIosAppUrls":[],"additionalAndroidAppUrls":[],"iconFileId":"0809d662-6e64-41dd-9756-71c06af049db","latestExternalVersion":"v0.0.3","latestInternalVersion":4,"downloadCount":10,"changedDate":1755394902000,"averageRating":5,"reviewCount":1,"categoryId":"270","compatibleDeviceTypeIds":["290","291","292","271","294","272","273","295","274","230","297","253","275","254","299","277","256","278","235","279","257","315","316","217","218","317","318","219","319","280","281","260","240","241","320","322","300","323","301","324","303","227","228","306","328","308"],"compatibleDevicePartNumbers":["006-B4105-00","006-B3704-00","006-B4260-00","006-B4426-00","006-B4233-00","006-B4587-00","006-B4472-00","006-B4315-00","006-B3943-00","006-B4432-00","006-B3949-00","006-B4574-00","006-B4079-00","006-B4222-00","006-B4314-00","006-B4312-00","006-B4125-00","006-B4257-00","006-B4180-00","006-B4570-00","006-B4536-00","006-B4666-00","006-B4534-00","006-B4261-00","006-B4775-00","006-B4234-00","006-B3703-00","006-B4175-00","006-B4586-00","006-B4588-00","006-B3944-00","006-B3950-00","006-B4565-00","006-B4433-00","006-B4171-00","006-B4656-00","006-B4542-00","006-B4223-00","006-B3851-00","006-B4313-00","006-B4625-00","006-B4556-00","006-B4017-00","006-B4181-00","006-B4124-00","006-B4258-00"],"hasTrialMode":false,"authFlowSupport":0,"permissions":["Positioning","Background","SensorHistory","UserProfile","Communications","ComplicationSubscriber"],"latestVersionAutoMigrated":false,"screenshotFileIds":["2b935df2-b3cf-4614-b3c2-eaf2dc9933d9","e3da1076-b904-48e8-a8e9-a4acfe3f7433"],"developer":{"fullName":null,"developerDisplayName":"Developer","logoUrl":null,"logoUrlDark":null,"trustedDeveloper":false},"paymentModel":1,"pricing":{"partNumber":"010-D2228-06","calculatedTax":false,"appliedPromotionGuids":[],"salePrice":{"price":23,"wholeUnitAmount":"23","currencySymbol":"kr","decimalSeparator":",","tenthDigit":0,"hundredthDigit":0,"currencyCode":"DKK","template":"{prefix}{wholeUnitAmount}{decimalSeparator}{tenthDigit}{hundredthDigit} {currencySymbol}{suffix}","formattedPrice":"23,00 kr"},"listPrice":{"price":23,"wholeUnitAmount":"23","currencySymbol":"kr","decimalSeparator":",","tenthDigit":0,"hundredthDigit":0,"currencyCode":"DKK","template":"{prefix}{wholeUnitAmount}{decimalSeparator}{tenthDigit}{hundredthDigit} {currencySymbol}{suffix}","formattedPrice":"23,00 kr"}},"fileSizeInfo":{"internalVersionNumber":4,"byteCountByDeviceTypeId":{"217":109580,"218":97356,"219":109580,"227":109580,"228":97356,"230":109580,"235":106060,"240":106060,"241":109580,"253":109580,"254":97356,"256":109580,"257":97356,"260":106060,"271":97228,"272":106060,"273":116060,"274":101708,"275":101708,"277":101708,"278":106060,"279":113340,"280":116860,"281":101708,"290":116860,"291":105228,"292":113340,"294":103388,"295":101708,"297":101708,"299":596972,"300":102956,"301":113340,"303":113340,"306":106060,"308":116060,"315":598332,"316":675212,"317":106060,"318":100940,"319":116060,"320":116060,"322":598332,"323":103388,"324":116060,"328":100940}},"settingsAvailabilityInfo":{"internalVersionNumber":4,"availabilityByDeviceTypeId":{"217":true,"218":true,"219":true,"227":true,"228":true,"230":true,"235":true,"240":true,"241":true,"253":true,"254":true,"256":true,"257":true,"260":true,"271":true,"272":true,"273":true,"274":true,"275":true,"277":true,"278":true,"279":true,"280":true,"281":true,"290":true,"291":true,"292":true,"294":true,"295":true,"297":true,"299":true,"300":true,"301":true,"303":true,"306":true,"308":true,"315":true,"316":true,"317":true,"318":true,"319":true,"320":true,"322":true,"323":true,"324":true,"328":true}},"firstApprovalDate":1738941315000,"lastApprovalDate":1738941315000,"creationDate":1738924467000,"releaseDate":1755394902000,"markedAsNew":false,"betaApp":false,"migrated":false,"hasVersionPendingScan":false,"downloadProtected":false,"childSafe":false}
		           """;

		// Act
		var app = JsonSerializer.Deserialize<GarminApp>(json, Setup.CreateJsonSerializerOptions());

		// Assert
		Assert.NotNull(app);
		Assert.Equal(AppId.Parse("d51c04af-05cd-40a3-8bcf-e45c461caa37"), app.Id);
		Assert.Equal(AppTypes.WatchFace, app.TypeId.PrimitiveEnumValue);
		Assert.Equal(6, app.Permissions.Count);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.Positioning);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.Background);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.Communications);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.SensorHistory);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.UserProfile);
		Assert.Contains(app.Permissions, permission => permission.PrimitiveEnumValue == AppPermissions.ComplicationSubscriber);
		Assert.Contains(app.CompatibleDeviceTypeIds, deviceId => deviceId.PrimitiveValue == 291);
		Assert.Equal(GarminPaymentModel.ThirdPartyPayment, app.PaymentModel);
	}
}