using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

/* a Test to check if the UI Navigation takes the player to the screens as intended and produce correct responses */
public class TestUINavigation
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
        //load the MenuScene
        altDriver.LoadScene("MenuScene");

        //Test 1 - after pressing the "Control" button, does the Note element appear
        altDriver.FindObject(By.NAME, "Controls").Tap();
        var noteElement = altDriver.WaitForObject(By.NAME, "Note");
        Assert.IsTrue(noteElement.enabled);

        //Test 2 - after pressing the close button on the bottom of the note, does the note disappear
        altDriver.FindObject(By.NAME, "CloseNote").Tap();
        var disabledObjects = altDriver.GetAllElements(enabled: false);
        Assert.IsNotEmpty(disabledObjects);

        //Test 3 - after pressing the "Play" button, does the StartScene loads
        altDriver.FindObject(By.NAME, "Play").Tap();
        Assert.AreEqual("StartScene", altDriver.GetCurrentScene());

        //Test 4 - Pressing E to interact with fridge loads the FridgeScene
        var fridgeObject = altDriver.FindObject(By.NAME, "fridge");
        fridgeObject.SetComponentProperty("FridgeInteraction", "key", AltKeyCode.E, "Assembly-CSharp");
        altDriver.KeyDown(AltKeyCode.E, 1);
        Assert.AreEqual("FridgeScene", altDriver.GetCurrentScene());

        
        //Test 5 - Pressing the "Finish" button loads the MCQScene
        disabledObjects = altDriver.GetAllElements(enabled: false);
        var finishButton = disabledObjects[0];
        foreach (var disabledObject in disabledObjects)
        {
            if (disabledObject.name == "Finish")
            {
                disabledObject.enabled = true;
                finishButton = disabledObject;
            }
        }
        finishButton.CallComponentMethod<string>("Finish", "OnButtonClick", "Assembly-CSharp", new object[] { });
        Assert.AreEqual("MCQScene",altDriver.GetCurrentScene());

        //Test 6 - Pressing the "Exit" button brings player back to the StartScene
        var exitbutton = altDriver.FindObject(By.NAME, "Exit");
        exitbutton.SetComponentProperty("UnityEngine.UI.Button", "interactable", true, "UnityEngine.UI");
        exitbutton.Tap();
        Assert.AreEqual("StartScene", altDriver.GetCurrentScene());

        //Test 7 - Pressing E to interact with stove loads the CookingScene
        altDriver.FindObject(By.NAME, "fridge").SetComponentProperty("FridgeInteraction", "key", AltKeyCode.A, "Assembly-CSharp");
        var stoveObject = altDriver.FindObject(By.NAME, "kitchen stove");
        stoveObject.SetComponentProperty("StoveInteraction", "key", AltKeyCode.E, "Assembly-CSharp");
        altDriver.KeyDown(AltKeyCode.E, 1);
        Assert.AreEqual("CookingScene", altDriver.GetCurrentScene());

        //Test 8 - Pressing the "Finish" button loads the MCQScene
        disabledObjects = altDriver.GetAllElements(enabled: false);
        finishButton = disabledObjects[0];
        foreach (var disabledObject in disabledObjects)
        {
            if (disabledObject.name == "Serve Dish")
            {
                disabledObject.enabled = true;
                finishButton = disabledObject;
            }
        }
        finishButton.CallComponentMethod<string>("Finish", "OnButtonClick", "Assembly-CSharp", new object[] { });
        Assert.AreEqual("MCQScene", altDriver.GetCurrentScene());

        //Test 9 - Pressing the "Exit" button brings player back to the StartScene
        exitbutton = altDriver.FindObject(By.NAME, "Exit");
        exitbutton.SetComponentProperty("UnityEngine.UI.Button", "interactable", true, "UnityEngine.UI");
        exitbutton.Tap();
        Assert.AreEqual("StartScene", altDriver.GetCurrentScene());

        //Test 10 - Pressing the "Go Back" button brings the player back to the MenuScene
        altDriver.FindObject(By.NAME, "Go Back").Tap();
        Assert.AreEqual("MenuScene", altDriver.GetCurrentScene());

    }

}