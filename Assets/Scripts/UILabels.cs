using UnityEngine;
using UnityEngine.UIElements;

public class UILabels : MonoBehaviour
{
    private UIDocument uiDocument;
    public GameObject shipGO;

    private Label sailOpening;
    private Label sailRotation;

    public float sailOpenPercentage = 0f;
    public float sailRotationDegrees = 0f;

    void Start()
    {
        uiDocument = GetComponentInParent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        sailOpening = root.Q<Label>("SailOpening");
        sailRotation = root.Q<Label>("SailRotation");

        sailOpening.text = "Sail Opening: %";
        sailRotation.text = "Sail Rotation: *";
        
    }


    // Update is called once per frame
    void Update()
    {
        sailOpenPercentage = shipGO.GetComponent<SailsMovement>().GetSailsOpening();
        sailRotationDegrees = shipGO.GetComponent<SailsMovement>().GetSailsRotation();

        sailOpening.text = $"Sail Opening: {sailOpenPercentage}%";
        sailRotation.text = $"Sail Rotation: {sailRotationDegrees}*";
    }
}
