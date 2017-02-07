using UnityEngine;
using System.Collections;
using JUMP;
using DiceRollerSample;
using UnityEngine.UI;

public class DiceRollerGameClientManager : MonoBehaviour {
    public JUMPMultiplayer OnlinePlayManager;
    public JUMPMultiplayer OfflinePlayManager;
    public Text MyScore;
    public Text TheirScore;
    public Text GameStatus;
    public Text TimeLeft;
    public Text Result;
    public Text DiceResult;
    public Button RollDice;
    DiceRollerGameStages UIStage = DiceRollerGameStages.WaitingForPlayers;

    public void OnSnapshotReceived(JUMPCommand_Snapshot data)
    {
        DiceRoller_Snapshot snap = new DiceRoller_Snapshot(data.CommandData);
		if(GameStatus)
        	GameStatus.text = snap.Stage.ToString();
		if(MyScore)
        	MyScore.text = "My score: " + snap.MyScore.ToString();
		if(TheirScore)
        	TheirScore.text = "Opponent's score: " + snap.OpponentScore.ToString();
		if(TimeLeft)
        	TimeLeft.text = "Time left: " + snap.SecondsRemaining.ToString("0.");
        UIStage = snap.Stage;
        if (UIStage == DiceRollerGameStages.Complete)
        {
			if(Result)
            	Result.text = (snap.MyScore > snap.OpponentScore) ? "You Won :)" : ((snap.MyScore == snap.OpponentScore) ? "Tied!" : "You Lost :(");
        }
    }

    public void RollADice()
    {
        int score = Random.Range(1, 7);

        if (RollDice != null)
        {
            // RollDice.GetComponent<Text>().text = "Rolled a " + score + " \nroll again.."; 
			if(DiceResult)
            	DiceResult.text = "Rolled a " + score;
        }
        JUMPMultiplayer.GameClient.SendCommandToServer(new DiceRollerCommand_RollDice(JUMPMultiplayer.PlayerID, score));
    }

    // Use this for initialization
    void Start()
    {
#if UNITY_5_4_OR_NEWER
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
#endif
#if UNITY_5_0
        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
#endif
        OfflinePlayManager.gameObject.SetActive(JUMPMultiplayer.IsOfflinePlayMode);
        OnlinePlayManager.gameObject.SetActive(!JUMPMultiplayer.IsOfflinePlayMode);
    }

    // Update is called once per frame
    void Update()
    {
		if(RollDice)
        	RollDice.interactable = (UIStage == DiceRollerGameStages.Playing);
	}
}
