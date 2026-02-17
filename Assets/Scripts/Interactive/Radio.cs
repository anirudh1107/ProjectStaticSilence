using UnityEngine;

public class Radio : MonoBehaviour, IInteractable
{
    public bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerManager player)
    {
        Debug.Log("Interacted with radio!");
        isActive = true;
    }
}
