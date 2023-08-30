using NUnit.Framework;
using AltTester.AltTesterUnitySDK.Driver;

/* a Test to check that for Level 1, 2 & 3, the algorithm we get is FIFO, BFS & DFS respectively */
public class TestLevelDifficulty
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

        //check Level 1
        var gameManager = altDriver.FindObject(By.NAME, "GameManager");
        gameManager.SetComponentProperty("ManagerScript", "level", 1, "Assembly-CSharp");
        gameManager.CallComponentMethod<string>("ManagerScript", "SetQuest", "Assembly-CSharp", new object[] { });
        Assert.AreEqual("FIFO", gameManager.GetComponentProperty<string>("ManagerScript", "questType", "Assembly-CSharp"));

        //check Level 2
        gameManager.SetComponentProperty("ManagerScript", "level", 2, "Assembly-CSharp");
        gameManager.CallComponentMethod<string>("ManagerScript", "SetQuest", "Assembly-CSharp", new object[] { });
        Assert.AreEqual("BFS", gameManager.GetComponentProperty<string>("ManagerScript", "questType", "Assembly-CSharp"));

        //check Level 3
        gameManager.SetComponentProperty("ManagerScript", "level", 3, "Assembly-CSharp");
        gameManager.CallComponentMethod<string>("ManagerScript", "SetQuest", "Assembly-CSharp", new object[] { });
        Assert.AreEqual("DFS", gameManager.GetComponentProperty<string>("ManagerScript", "questType", "Assembly-CSharp"));
    }


}