using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

/* a Test to check if the information displayed in the note UI is relevant to the algorithm being dealt with */
public class TestAlgoInfo
{   //Important! If your test file is inside a folder that contains an .asmdef file, please make sure that the assembly definition references NUnit.
    public AltDriver altDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        altDriver =new AltDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altDriver.Stop();
    }

    [Test]
    public void TestFunc()
    {
        //load the StartScene
        altDriver.LoadScene("StartScene");

        //load the FridgeScene
        altDriver.LoadScene("FridgeScene");

        //click the "Info" button
        altDriver.FindObject(By.NAME, "Info").Tap();

        //check if Note text is equal to actual algorithm information
        var noteText = altDriver.FindObject(By.NAME, "NoteText").GetText();
        var actualInfo = altDriver.FindObject(By.NAME, "Canvas").GetComponentProperty<string>("StoreIngredients", "infoText", "Assembly-CSharp");
        Assert.AreEqual(noteText, actualInfo);

        //load the CookingScene
        altDriver.LoadScene("CookingScene");

        //click the "Info" button
        altDriver.FindObject(By.NAME, "Info").Tap();

        //check if Note text is equal to actual algorithm information
        noteText = altDriver.FindObject(By.NAME, "NoteText").GetText();
        actualInfo = altDriver.FindObject(By.NAME, "Canvas").GetComponentProperty<string>("SpawnIngredient", "infoText", "Assembly-CSharp");
        Assert.AreEqual(noteText, actualInfo);
    }

}