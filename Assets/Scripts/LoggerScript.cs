using UnityEngine;
using System.Collections;

public class LoggerScript : MonoBehaviour {

    string playerScoreWithoutMagnetism = ""; // Setting up the playerScoreWithoutMagnetism 
    string playerScoreWithoutAssistance = ""; // Setting up the playerScoreWithoutMagnetism 
    string successWithoutAnyTypeOfAssist = "";
    string prePoleNumber = "", prePower = "", preDistanceToTheNearestPole = "";
    bool LogData;
    private MainScript mainScript;
    private GameObject gameEngine;

    public void loggerMethodFromRealTrail(string successWithoutAnyAssist)
    {
        successWithoutAnyTypeOfAssist = successWithoutAnyAssist;
        //if(mainScript.MagnetismOn)
        //    playerScoreWithoutMagnetism = successWithoutAnyAssist;
        //else if (mainScript.TrajectoryAssistanceOn)
        //    playerScoreWithoutAssistance = successWithoutAnyAssist;
        //else
        //{
        //    playerScoreWithoutMagnetism = "null";
        //    playerScoreWithoutAssistance = "null";
        //}
    }

    public void loggerMethod(int currentSuccess, float poleDistance, float poleHeight, string currentPoleName, string currentPoleMag, string currentPoleFric, int getScore, string currentPoleMagAr, float distanceOfNearestPoleXPosition, float assistanceLevel, float hindranceLevel)
    {
        GameObject powerObject = GameObject.FindWithTag("Power");                       // get the game object tagged as "Power"
        PowerScript script = powerObject.GetComponent("PowerScript") as PowerScript;    // inside the object tagged as "Power", get PowerScript script

        // We actually do not need this section -- but it's still here ! What a waste of energy and time
        // Since everything from scripts are getting here without a proper serialization as we are updating the project quite a few times 
        // this section makes everything in a serial order for the log file
        string PoleNumber = currentPoleName;                // the current pole number where the player is      
        string PoleDistance = poleDistance.ToString();      // the current pole X position
        string PoleHeight = poleHeight.ToString();          // the current pole Y position
        string PoleMagnetism = currentPoleMag;              // the current pole's magnetism
        string PoleFriction = currentPoleFric;              // the current pole's materials friction
        string Power = script.mouseDepressPower.ToString(); // the power that is set to the      
        string PoleMagnetismArea = currentPoleMagAr;        // Magnetism Area of the pole
        string DistanceToTheNearestPole = distanceOfNearestPoleXPosition.ToString();
        string trajectoryAssistance = assistanceLevel.ToString();
        string trajectoryHindrance = hindranceLevel.ToString();
        int currentScore = getScore;        // the current score of the player
        int Success = currentSuccess;       // success ?
        int Failure = 0;
        if (currentSuccess == 0) Failure = 1;      // if not success then failed


        // Check for duplicate log 
        // It looks like due to duplicate hit there might be more than one log for a single pole

        if (prePoleNumber == PoleNumber &&
            prePower == Power &&
            preDistanceToTheNearestPole == DistanceToTheNearestPole)
        {
            LogData = false;
        }
        else
            LogData = true;


        prePoleNumber = PoleNumber;                             // set pre for this data
        prePower = Power;                                       // set Power equal to prePower 
        preDistanceToTheNearestPole = DistanceToTheNearestPole; // set DistanceToTheNearestPole equal to the preDistanceToTheNearestPole

        if (LogData)
        {
            string url = MainScript.server_url + "/loggame";

            WWWForm form = new WWWForm();
            form.AddField("poleNumber", PoleNumber);
            form.AddField("poleDistance", PoleDistance);
            form.AddField("poleHeight", PoleHeight);
            form.AddField("distanceToTheNearestPole", DistanceToTheNearestPole);
            form.AddField("magnetismLevel", PoleMagnetism);
            form.AddField("magnetismArea", PoleMagnetismArea);
            form.AddField("trajectoryAssistance", trajectoryAssistance);
            form.AddField("trajectoryHindrance", trajectoryHindrance);
            form.AddField("friction", PoleFriction);
            form.AddField("power", Power);
            form.AddField("score", currentScore);
            form.AddField("success", Success);
            form.AddField("failure", Failure);
            form.AddField("successWithoutAnyAssist", successWithoutAnyTypeOfAssist);
            // Has the game has been initialised or not. Restarting after death will be set to 0
            form.AddField("gameInitialised", StartScreenScript.gameInitialised);


            //Log data in the unity inspector
            // Un-Comment the next section for showing it into the unity inspector
            //Debug.Log("PoleNumber: " + PoleNumber + "\n" +
            //"Pole Distance: " + PoleDistance + "\n" +
            //"PoleHeight: " + PoleHeight + "\n" +
            //"DistanceToTheNearestPole: " + DistanceToTheNearestPole + "\n" +
            //"PoleMagnetism: " + PoleMagnetism + "\n" +
            //"Magnetism Area: " + currentPoleMagAr + "\n" +
            //"Trajectory Assistance: " + trajectoryAssistance + "\n" +
            //"Trajectory Hindrance: " + trajectoryHindrance + "\n" +
            //"Pole Friction: " + PoleFriction + "\n" +
            //"Power: " + Power + "\n" +
            //"Current Score: " + currentScore + "\n" +
            //"Success: " + Success + "\n" +
            //"Failure: " + Failure + "\n" +
            //"Player Score Without Magnetism: " + playerScoreWithoutMagnetism + "\n" +
            //"Game Initialised: " + StartScreenScript.gameInitialised);


            WWW www = new WWW(url, form);

            StartCoroutine(WaitForRequest(www));
        }
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            //Debug.Log("WWW Error: " + www.error);
        }
    }
}
