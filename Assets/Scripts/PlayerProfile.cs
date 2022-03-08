using Mirror;
using TMPro;
using UnityEngine;

public class PlayerProfile : NetworkBehaviour {
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private GameObject floatingInfo;

    private Material playerMaterial;

    [SyncVar(hook = nameof(OnNameChange))]
    public string PlayerName;

    [SyncVar(hook = nameof(OnColourChange))]
    public Color PlayerColour = Color.white;

    private void OnNameChange(string oldName, string newName) {
        playerNameText.text = newName;
    }

    private void OnColourChange(Color oldColour, Color newColour) {
        playerNameText.color = newColour;
        playerMaterial = new Material(GetComponent<Renderer>().material);
        playerMaterial.color = newColour;
        GetComponent<Renderer>().material = playerMaterial;
    }

    public override void OnStartLocalPlayer() {
        floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = $"Player {Random.Range(100, 9999)}";
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string name, Color col) {
        PlayerName = name;
        PlayerColour = col;
    }

    void Update() {
        if (!isLocalPlayer) {
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }
    }
}

