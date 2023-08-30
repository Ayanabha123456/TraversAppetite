using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

/* a Test to check that if a wrong ingredient is dropped in the cell, the 'Revert' button shows up and the cell color changes to red */
public class TestImmediateFeedback
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
        //load the Start Scene
        altDriver.LoadScene("StartScene");

        //transition to the Cooking Scene
        var gameManager = altDriver.FindObject(By.NAME, "GameManager");
        gameManager.SetComponentProperty("ManagerScript", "haveIngredients", true, "Assembly-CSharp");
        var stoveObject = altDriver.FindObject(By.NAME, "kitchen stove");
        stoveObject.SetComponentProperty("StoveInteraction", "key", AltKeyCode.E, "Assembly-CSharp");
        altDriver.KeyDown(AltKeyCode.E, 1);

        //close the Note first
        altDriver.FindObject(By.NAME, "CloseText").Tap();

        //Test drag and drop a wrong ingredient in the cell
        var wrongIngredient = altDriver.FindObjectsWhichContain(By.COMPONENT, "DragAndDrop")[5];
        var cell = altDriver.FindObject(By.NAME, "10");
        altDriver.Swipe(new AltVector2(wrongIngredient.x, wrongIngredient.y), new AltVector2(cell.x, cell.y), 1);

        //check if the 'Revert' button shows up
        Assert.IsTrue(altDriver.FindObject(By.NAME, "Revert").enabled);

        //check if the cell color changes to red
        Assert.AreEqual(UnityEngine.Color.red, cell.GetComponentProperty<UnityEngine.Color>("UnityEngine.UI.Image", "color", "UnityEngine.UI"));

    }

}