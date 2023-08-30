using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

/* a Test to check if the MCQScene functions as intended */
public class TestPostGameMCQ
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

        //set a questType (or algorithm)
        var gameManager = altDriver.FindObject(By.NAME, "GameManager");
        gameManager.SetComponentProperty("ManagerScript", "questType", "FIFO", "Assembly-CSharp");

        //load the MCQScene
        altDriver.LoadScene("MCQScene");

        //check if the question is loaded
        var question = altDriver.FindObject(By.NAME, "Question Text").GetText();
        Assert.AreNotEqual("What is the example question?", question);

        //click one of the options
        altDriver.FindObject(By.NAME, "opt1").Tap();

        //click the "Submit" button
        altDriver.FindObject(By.NAME, "Submit").Tap();

        //check if all the other options become non-interactable
        for (int i = 1; i <= 4; i++)
        {
            Assert.IsFalse(altDriver.FindObject(By.NAME,"opt"+i.ToString()).GetComponentProperty<bool>("UnityEngine.UI.Button", "interactable", "UnityEngine.UI"));
        }

        //check if you get an outcome prompt
        var outcome = altDriver.FindObject(By.NAME, "Outcome").GetText();
        Assert.AreNotEqual("", outcome);

        //check if the "Exit" button becomes interactable
        Assert.IsTrue(altDriver.FindObject(By.NAME, "Exit").GetComponentProperty<bool>("UnityEngine.UI.Button", "interactable", "UnityEngine.UI"));
    }

}